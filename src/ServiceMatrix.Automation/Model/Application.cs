namespace NServiceBusStudio
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using AbstractEndpoint;
    using Automation.Commands;
    using Automation.CustomSolutionBuilder;
    using Automation.Extensions;
    using Automation.Infrastructure;
    using Automation.Licensing;
    using Automation.Model;
    using EnvDTE;
    using Microsoft.VisualStudio.ExtensionManager;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.Win32;
    using NuGet.VisualStudio;
    using NuPattern;
    using NuPattern.Runtime;
    using NuPattern.VisualStudio;
    using NuPattern.VisualStudio.Solution;
    using Process = System.Diagnostics.Process;

    partial interface IApplication
    {
        string ContractsProjectName { get; }
        string InternalMessagesProjectName { get; }
        InfrastructureManager InfrastructureManager { get; }
        string GetTargetNsbVersion(IProductElement element);
    }

    partial class Application : IRenameRefactoringNotSupported
    {
        [Import]
        public IPatternManager PatternManager { get; set; }

        [Import]
        public IPatternWindows PatternWindows { get; set; }

        [Import]
        private ISolution Solution { get; set; }

        [Import]
        public CustomSolutionBuilder CustomSolutionBuilder { get; set; }

        [Import]
        public StatisticsManager StatisticsManager { get; set; }

        [Import]
        public RemoveEmptyAddMenus RemoveEmptyAddMenus { get; set; }

        [Import]
        public IVsPackageInstaller VsPackageInstaller { get; set; }

        [Import]
        public IVsPackageInstallerServices VsPackageInstallerServices { get; set; }

        [Import]
        public IStatusBar StatusBar { get; set; }

        partial void Initialize()
        {
            InfrastructureManager = new InfrastructureManager(this, ServiceProvider, PatternManager);

            if (currentApplication == null)
            {
                currentApplication = this;
                // Force initialization of NserviceBusVersion from file
                InitializeExtensionDependentData();

                CustomSolutionBuilder.Initialize(ServiceProvider);

                PatternManager.ElementDeleting += (s, e) =>
                {
                    // Delete all related artifacts links
                    var element = e.Value.As<IProductElement>();
                    element.RemoveArtifactLinks(UriService, Solution);

                    // If it's a Component Link, remove all links into Endpoints (they're not artifact links)
                    var componentLink = element.As<IAbstractComponentLink>();

                    if (componentLink != null &&
                        componentLink.ComponentReference.Value != null)
                    {
                        var endpoint = element.Parent.Parent.As<IAbstractEndpoint>();
                        componentLink.ComponentReference.Value.RemoveLinks(endpoint);
                    }
                };
            }

            // Set/Get static values
            currentApplication = this;
            NServiceBusVersion = nserviceBusVersion;
            ServiceControlEndpointPluginVersion = serviceControlEndpointPluginVersion;
            ExtensionPath = extensionPath;

            CheckLicense();
            SetNuGetPackagesVersion();
            SetOptionSettings();
            SetPropagationHandlers();
            SetDomainSpecifiLogging();
            SetRemoveEmptyAddMenus();
            SetF5Experience();

            new ShowNewDiagramCommand { ServiceProvider = ServiceProvider }.Execute();

            Dispatcher.CurrentDispatcher.BeginInvoke(
                new Action(AddNugetFiles), null);
        }

        private void SetNuGetPackagesVersion()
        {
            StatusBar.DisplayMessage("Obtaining NuGet packages versions...");
            // Clear the cached versions, so for every new solution we create, we'll check Nuget for latest versions and then use that version
            // for all projects in the solution.
            NugetPackageVersionManager.ClearCache();

            if (String.IsNullOrEmpty(NuGetPackageVersionNServiceBus))
            {
                NuGetPackageVersionNServiceBus = null;
                NuGetPackageVersionNServiceBusActiveMQ = null;
                NuGetPackageVersionNServiceBusRabbitMQ = null;
                NuGetPackageVersionNServiceBusSqlServer = null;
                NuGetPackageVersionNServiceBusAzureQueues = null;
                NuGetPackageVersionNServiceBusAzureServiceBus = null;
                NuGetPackageVersionServiceControlPlugins = null;
            }
            StatusBar.DisplayMessage(" ");
        }

        private void SetOptionSettings()
        {
            var envdte = ServiceProvider.TryGetService<DTE>();
            var properties = envdte.get_Properties("ServiceMatrix", "General");

            ProjectNameInternalMessages = properties.Item("ProjectNameInternalMessages").Value.ToString();
            ProjectNameContracts = properties.Item("ProjectNameContracts").Value.ToString();
            ProjectNameCode = properties.Item("ProjectNameCode").Value.ToString();
        }

        private void SetF5Experience()
        {
            var envdte = ServiceProvider.TryGetService<DTE>();
            DebuggerEvents = envdte.DTE.Events.DebuggerEvents;
            DebuggerEvents.OnEnterRunMode += DebuggerEvents_OnEnterRunMode;
        }

        private void DebuggerEvents_OnEnterRunMode(dbgEventReason Reason)
        {
            if (String.IsNullOrEmpty(ServiceControlInstanceURI) ||
                Reason != dbgEventReason.dbgEventReasonLaunchProgram)
            {
                return;
            }

            // Write DebugSessionId on Endpoints Bin folder
            var debugSessionId = String.Format("{0}@{1}@{2}", Environment.MachineName, InstanceName, DateTime.Now.ToUniversalTime().ToString("s")).Replace(" ", "_");
            foreach (var endpoint in Design.Endpoints.GetAll())
            {
                var activeConfiguration = endpoint.Project.As<Project>().ConfigurationManager.ActiveConfiguration;
                var outputPath = activeConfiguration.Properties.Item("OutputPath").Value.ToString();

                var binFolder =
                    Path.IsPathRooted(outputPath)
                        ? outputPath
                        : Path.Combine(Path.GetDirectoryName(endpoint.Project.PhysicalPath), outputPath);

                if (!Directory.Exists(binFolder))
                {
                    Directory.CreateDirectory(binFolder);
                }

                File.WriteAllText(Path.Combine(binFolder, "ServiceControl.DebugSessionId.txt"), debugSessionId);
            }

            // If ServiceInsight is installed and invocation URI registerd
            if (LaunchServiceInsightOnDebug &&
                Registry.ClassesRoot.OpenSubKey("si") != null)
            {
                var url = String.Format("si://{0}?EndpointName={1}.{2}&Search={3}&AutoRefresh={4}",
                                            ServiceControlInstanceURI.Replace("http://", ""),
                                            InstanceName,
                                            Design.Endpoints.GetAll().First().InstanceName,
                                            debugSessionId,
                                            1);

                // Start ServiceInsight with parameters
                Process.Start(url);
            }
        }

        private void SetPropagationHandlers()
        {
            OnInstantiatedEndpoint += (s, e) =>
            {
                var ep = s as IAbstractEndpoint;
                ep.ErrorQueue = ErrorQueue;
                ep.ForwardReceivedMessagesTo = ForwardReceivedMessagesTo;
            };

            ForwardReceivedMessagesToChanged += (s, e) =>
            {
                foreach (var ep in this.GetAbstractEndpoints())
                {
                    if (!ep.OverridenProperties.Contains("ForwardReceivedMessagesTo"))
                    {
                        ep.ForwardReceivedMessagesTo = ForwardReceivedMessagesTo;
                    }
                }
            };

            ErrorQueueChanged += (s, e) =>
            {
                foreach (var ep in this.GetAbstractEndpoints())
                {
                    if (!ep.OverridenProperties.Contains("ErrorQueue"))
                    {
                        ep.ErrorQueue = ErrorQueue;
                    }
                }
            };

            TransportChanged += (s, e) =>
            {
                if (Transport == TransportType.Msmq.ToString())
                {
                    TransportConnectionString = @"";
                }
                else if (Transport == TransportType.RabbitMQ.ToString())
                {
                    TransportConnectionString = @"host=localhost";
                    Design.Endpoints.GetAll().ForEach(x => x.Project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.RabbitMQ", GetTargetNsbVersion(x.As<IProductElement>())));
                }
                else if (Transport == TransportType.SqlServer.ToString())
                {
                    TransportConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;Integrated Security=True";
                    Design.Endpoints.GetAll().ForEach(x => x.Project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.SqlServer", GetTargetNsbVersion(x.As<IProductElement>())));
                }
                else if (Transport == TransportType.AzureQueues.ToString())
                {
                    TransportConnectionString = @"UseDevelopmentStorage=True;";
                    Design.Endpoints.GetAll().ForEach(x => x.Project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Azure.Transports.WindowsAzureStorageQueues", GetTargetNsbVersion(x.As<IProductElement>())));
                }
                else if (Transport == TransportType.AzureServiceBus.ToString())
                {
                    TransportConnectionString = @"UseDevelopmentStorage=True;";
                    Design.Endpoints.GetAll().ForEach(x => x.Project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Azure.Transports.WindowsAzureServiceBus", GetTargetNsbVersion(x.As<IProductElement>())));
                }
            };
        }

        private void SetDomainSpecifiLogging()
        {
            PatternManager.ElementCreated += (s, e) => { if (!(e.Value is ICollection)) { tracer.TraceStatistics("{0} created with name: {1}", e.Value.DefinitionName, e.Value.InstanceName); } };
            PatternManager.ElementDeleted += (s, e) => { if (!(e.Value is ICollection)) { tracer.TraceStatistics("{0} deleted with name: {1}", e.Value.DefinitionName, e.Value.InstanceName); } };
        }

        private void SetRemoveEmptyAddMenus()
        {
            RemoveEmptyAddMenus.WireSolution(ServiceProvider);
        }

        private void AddNugetFiles()
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                         new Action(delegate { }));

                if (currentApplication.Design.ContractsProject == null)
                {
                    currentApplication.Design.CreateContractsProject(string.Format("{0}.{1}", currentApplication.InstanceName, currentApplication.ProjectNameContracts));
                    currentApplication.Design.CreateInternalMessagesProject(string.Format("{0}.{1}", currentApplication.InstanceName, currentApplication.ProjectNameInternalMessages));
                }
                var solution = currentApplication.ServiceProvider.TryGetService<ISolution>();

                if (!solution.Find(".nuget").Any())
                {
                    //var folder = solution.CreateSolutionFolder(".nuget");
                    //folder.Add(Path.Combine(currentApplication.ExtensionPath, @".nuget\NuGet.exe"), @".nuget\NuGet.exe");
                    //folder.Add(Path.Combine(currentApplication.ExtensionPath, @".nuget\NuGet.targets"), @".nuget\NuGet.targets");

                    var solutionItems = solution.SolutionFolders.FirstOrDefault(x => x.Name == "Solution Items");
                    if (solutionItems == null)
                    {
                        solutionItems = solutionItems.CreateSolutionFolder("Solution Items");
                    }
                    solutionItems.Add(Path.Combine(currentApplication.ExtensionPath, @"ServiceMatrixVersion.txt"), @"ServiceMatrixVersion.txt");
                }
                solution.Select();
            }
            catch { }
        }

        private void CheckLicense()
        {
            if (LicenseManager.Instance.HasLicenseExpired())
            {
                LicensedVersion = LicenseManager.Instance.PromptUserForLicenseIfTrialHasExpired();
                if (LicensedVersion)
                {
                    GlobalSettings.Instance.IsLicenseValid = true;
                    EnableSolutionBuilder();
                    MessageBox.Show("Your license has been verified. Please restart Visual Studio for the licensing changes to take effect", "Service Matrix - License Verified");
                    return;
                }

                GlobalSettings.Instance.IsLicenseValid = false;
                DisableSolutionBuilder();
                if (!AsProduct().IsSerializing) // is creating
                {
                    CustomSolutionBuilder.ShowNoSolutionState();
                    throw new Exception("Trial period for ServiceMatrix has Expired. A new NServiceBus solution cannot be created.");
                }
            }
            else
            {
                GlobalSettings.Instance.IsLicenseValid = true;
                EnableSolutionBuilder();
            }
        }

        private void EnableSolutionBuilder()
        {
            IsValidLicensed = true;
            CustomSolutionBuilder.EnableSolutionBuilder();
        }

        private void DisableSolutionBuilder()
        {
            IsValidLicensed = false;
            CustomSolutionBuilder.DisableSolutionBuilder();
        }

        public InfrastructureManager InfrastructureManager { get; private set; }

        public string ContractsProjectName
        {
            get
            {
                return Design.ContractsProject.InstanceName;
            }
        }

        public string InternalMessagesProjectName
        {
            get
            {
                return Design.InternalMessagesProject.InstanceName;
            }
        }

        static Application currentApplication;
        static string extensionPath;
        static string nserviceBusVersion;
        static string serviceControlEndpointPluginVersion;

        [Import(typeof(SVsServiceProvider))]
        public IServiceProvider VsServiceProvider { get; set; }

        public void InitializeExtensionDependentData()
        {
            var extensionManager = (IVsExtensionManager)VsServiceProvider.TryGetService<SVsExtensionManager>();
            var extension = extensionManager.GetInstalledExtension("23795EC3-3DEA-4F04-9044-4056CF91A2ED");

            //var resolver = this.ServiceProvider.TryGetService<IUriReferenceService>();
            //var extension = resolver.ResolveUri<Microsoft.VisualStudio.ExtensionManager.IInstalledExtension>(new Uri(@"vsix://23795EC3-3DEA-4F04-9044-4056CF91A2ED"));
            extensionPath = extension.InstallPath;
            nserviceBusVersion = File.ReadAllText(Path.Combine(extension.InstallPath, "NServiceBusVersion.txt"));
            serviceControlEndpointPluginVersion = File.ReadAllText(Path.Combine(extension.InstallPath, "ServiceControlEndpointPluginVersion.txt"));
        }

        public static void SelectSolution()
        {
            currentApplication.ServiceProvider.TryGetService<ISolution>().Select();
        }

        public DebuggerEvents DebuggerEvents { get; set; }

        public int ServiceInsightProcessId { get; set; }

        public string GetTargetNsbVersion(IProductElement element)
        {
            // TODO retrieve from element
            return TargetNsbVersion;
        }

    }

    public enum TransportType
    {
        Msmq,
        RabbitMQ,
        SqlServer,
        AzureQueues,
        AzureServiceBus
    }
}

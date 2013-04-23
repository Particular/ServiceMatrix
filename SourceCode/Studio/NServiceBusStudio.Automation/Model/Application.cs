using AbstractEndpoint;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using NServiceBusStudio.Automation.CustomSolutionBuilder;
using NServiceBusStudio.Automation.Extensions;
using NServiceBusStudio.Automation.Infrastructure;
using NServiceBusStudio.Automation.Licensing;
using NuPattern.Runtime;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace NServiceBusStudio
{
    partial interface IApplication
    {
        string ContractsProjectName { get; }
        string InternalMessagesProjectName { get; }
        InfrastructureManager InfrastructureManager { get; }
        IEndpoints Endpoints { get; set; }
    }

    partial class Application : IRenameRefactoringNotSupported
    {        
        [Import]
        public IPatternManager PatternManager { get; set; }

        [Import]
        private ISolution Solution { get; set; }

        [Import]
        public CustomSolutionBuilder CustomSolutionBuilder { get; set; }

        [Import]
        public StatisticsManager StatisticsManager { get; set; }

        public IEndpoints Endpoints { get; set; }

        partial void Initialize()
        {
            this.InfrastructureManager = new InfrastructureManager(this, this.ServiceProvider, this.PatternManager);

            if (currentApplication == null)
            {
                currentApplication = this;
                // Force initialization of NserviceBusVersion from file
                this.InitializeExtensionDependentData();

                var events = this.ServiceProvider.TryGetService<ISolutionEvents>();
                events.SolutionOpened += new EventHandler<SolutionEventArgs>(events_SolutionOpened);

                this.CustomSolutionBuilder.Initialize(this.ServiceProvider);

                this.PatternManager.ElementDeleting += (s, e) =>
                {
                    // Delete all related artifacts links
                    var element = e.Value.As<IProductElement>();
                    element.RemoveArtifactLinks(this.UriService, this.Solution);

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
            this.NServiceBusVersion = nserviceBusVersion;
            this.ExtensionPath = extensionPath;

            SetPropagationHandlers();
            SetDomainSpecifiLogging();
            CheckLicense();
        }

        private void CheckLicense()
        {
            try
            {
                LicenseManager.PromptUserForLicense();
                this.EnableSolutionBuilder();
            }
            catch (Rhino.Licensing.LicenseExpiredException)
            {
                this.DisableSolutionBuilder();

                if (!this.AsProduct().IsSerializing) // is creating
                {
                    this.CustomSolutionBuilder.ShowNoSolutionState();
                    throw new Exception("Trial period for NServiceBus Studio has Expired. A new NServiceBus solution cannot be created.");
                }
            }
        }

        private void EnableSolutionBuilder()
        {
            this.IsValidLicensed = true;
            this.CustomSolutionBuilder.EnableSolutionBuilder();
        }

        private void DisableSolutionBuilder()
        {
            this.IsValidLicensed = false;
            this.CustomSolutionBuilder.DisableSolutionBuilder();
        }

        private void SetDomainSpecifiLogging()
        {
            this.PatternManager.ElementCreated += (s, e) => { if (!(e.Value is ICollection)) { tracer.TraceStatistics("{0} created with name: {1}", e.Value.DefinitionName, e.Value.InstanceName); } };
            this.PatternManager.ElementDeleted += (s, e) => { if (!(e.Value is ICollection)) { tracer.TraceInformation("{0} deleted with name: {1}", DateTime.Now.ToString(), e.Value.DefinitionName, e.Value.InstanceName); } };
        }

        private void SetPropagationHandlers()
        {
            this.OnInstantiatedEndpoint += (s, e) =>
            {
                var ep = s as IAbstractEndpoint;
                ep.ErrorQueue = this.ErrorQueue;
                ep.ForwardReceivedMessagesTo = this.ForwardReceivedMessagesTo;
            };

            this.ForwardReceivedMessagesToChanged += (s, e) =>
            {
                foreach (var ep in this.GetAbstractEndpoints())
                {
                    if (!ep.OverridenProperties.Contains("ForwardReceivedMessagesTo"))
                    {
                        ep.ForwardReceivedMessagesTo = this.ForwardReceivedMessagesTo;
                    }
                }
            };

            this.ErrorQueueChanged += (s, e) =>
            {
                foreach (var ep in this.GetAbstractEndpoints())
                {
                    if (!ep.OverridenProperties.Contains("ErrorQueue"))
                    {
                        ep.ErrorQueue = this.ErrorQueue;
                    }
                }
            };

            this.TransportChanged += (s, e) =>
            {
                if (this.Transport == TransportType.Msmq.ToString())
                {
                    this.TransportConnectionString = @"";
                }
                else if (this.Transport == TransportType.ActiveMQ.ToString())
                {
                    this.TransportConnectionString = @"ServerUrl=activemq:tcp://mybroker";
                }
                else if (this.Transport == TransportType.RabbitMQ.ToString())
                {
                    this.TransportConnectionString = @"host=localhost";
                }
                else if (this.Transport == TransportType.SqlServer.ToString())
                {
                    this.TransportConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;Integrated Security=True";
                }
            };
        }

        void events_SolutionOpened(object sender, SolutionEventArgs e)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                new Action(AddNugetFiles), null);
        }

        static private void AddNugetFiles()
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                                         new Action(delegate { }));

                if (currentApplication.Design.ContractsProject == null)
                {
                    currentApplication.Design.CreateContractsProject(string.Format("{0}.Contract", currentApplication.InstanceName));
                    currentApplication.Design.CreateInternalMessagesProject(string.Format("{0}.InternalMessages", currentApplication.InstanceName));
                }
                var solution = currentApplication.ServiceProvider.TryGetService<ISolution>();

                if (!solution.Find(".nuget").Any())
                {
                    var folder = solution.CreateSolutionFolder(".nuget");
                    folder.Add(Path.Combine(currentApplication.ExtensionPath, @".nuget\NuGet.exe"), @".nuget\NuGet.exe");
                    folder.Add(Path.Combine(currentApplication.ExtensionPath, @".nuget\NuGet.targets"), @".nuget\NuGet.targets");
                }
                solution.Select();
            }
            catch { }
        }

        public InfrastructureManager InfrastructureManager { get; private set; }

        public string ContractsProjectName
        {
            get
            {
                return this.Design.ContractsProject.InstanceName;
            }
        }

        public string InternalMessagesProjectName
        {
            get
            {
                return this.Design.InternalMessagesProject.InstanceName;
            }
        }

        static Application currentApplication = null;
        static string extensionPath;
        static string nserviceBusVersion;

        [Import(typeof(Microsoft.VisualStudio.Shell.SVsServiceProvider))]
        public IServiceProvider VsServiceProvider { get; set; }

        public void InitializeExtensionDependentData()
        {
            //IVsExtensionManager extensionManager = (IVsExtensionManager) this.VsServiceProvider.TryGetService<SVsExtensionManager>();
            //var extension = extensionManager.GetInstalledExtension("a5e9f15b-ad7f-4201-851e-186dd8db3bc9");
            var resolver = this.ServiceProvider.TryGetService<IFxrUriReferenceService>();
            var extension = resolver.ResolveUri<Microsoft.VisualStudio.ExtensionManager.IInstalledExtension>(new Uri(@"vsix://a5e9f15b-ad7f-4201-851e-186dd8db3bc9"));
            extensionPath = extension.InstallPath;
            nserviceBusVersion = File.ReadAllText(Path.Combine(extension.InstallPath, "NServiceBusVersion.txt"));
        }

        public static void SelectSolution()
        {
            currentApplication.ServiceProvider.TryGetService<ISolution>().Select();
        }

        
    }

    public enum TransportType
    {
        Msmq,
        ActiveMQ,
        RabbitMQ,
        SqlServer
    }
}

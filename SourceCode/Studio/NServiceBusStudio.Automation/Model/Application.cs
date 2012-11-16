using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using NServiceBusStudio.Automation.Commands;
using Microsoft.VisualStudio.Patterning.Runtime;
using NServiceBusStudio.Automation.Extensions;
using NServiceBusStudio.Automation.Infrastructure;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using System.IO;
using Microsoft.VisualStudio.ExtensionManager;
using AbstractEndpoint;
using NServiceBusStudio.Automation.CustomSolutionBuilder;
using System.ComponentModel.Composition;
namespace NServiceBusStudio
{
    partial interface IApplication
    {
        string ContractsProjectName { get; }
        string InternalMessagesProjectName { get; }
        InfrastructureManager InfrastructureManager { get; }
        IEndpoints Endpoints { get; set; }
    }

    partial class Application
    {
        [Import]
        public IPatternManager PatternManager { get; set; }

        [Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        private ISolution Solution { get; set; }

        [Import]
        public CustomSolutionBuilder CustomSolutionBuilder { get; set; }

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
                        var endpoint = element.Product.As<IAbstractEndpoint>();
                        componentLink.ComponentReference.Value.RemoveLinks(endpoint);
                    }
                };
            }

            // Set/Get static values
            currentApplication = this;
            this.NServiceBusVersion = nserviceBusVersion;
            this.ExtensionPath = extensionPath;

            SetPropagationHandlers();
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
        }

        static void events_SolutionOpened(object sender, SolutionEventArgs e)
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
            var extension = resolver.ResolveUri<IInstalledExtension>(new Uri(@"vsix://a5e9f15b-ad7f-4201-851e-186dd8db3bc9"));
            extensionPath = extension.InstallPath;
            nserviceBusVersion = File.ReadAllText(Path.Combine(extension.InstallPath, "NServiceBusVersion.txt"));
        }

        public static void SelectSolution()
        {
            currentApplication.ServiceProvider.TryGetService<ISolution>().Select();
        }
    }
}

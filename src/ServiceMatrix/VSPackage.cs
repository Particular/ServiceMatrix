namespace NServiceBusStudio
{
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using NServiceBusStudio.Automation.CustomSolutionBuilder;
    using NServiceBusStudio.Automation.CustomSolutionBuilder.Views;
    using NServiceBusStudio.Automation.Infrastructure;
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using NuPattern;
    using NuPattern.Diagnostics;
    using NuPattern.VisualStudio;
    using NuPattern.Runtime.Diagnostics;
    using ServiceMatrix.Diagramming.Views;
    using ServiceMatrix.Diagramming;
    using DslShell = Microsoft.VisualStudio.Modeling.Shell;
    using NServiceBusStudio.Automation.Exceptions;
    using System.IO;
    using System.Reflection;
    using ExceptionReporting;

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    [Guid(GuidList.guidNServiceBusStudioPkgString)]
    [ProvideToolWindow(typeof(NServiceBusDetailsToolWindow), Window = ToolWindowGuids.TaskList, Style = VsDockStyle.Tabbed, Transient = true)]
    [ProvideToolWindow(typeof(ServiceMatrixDiagramToolWindow), Window = ToolWindowGuids.DocOutline, Style = VsDockStyle.MDI, Transient = true)]
    [ProvideOptionPage(typeof(GeneralOptionsPage), "ServiceMatrix", "General", 0, 0, true)]
    [ProvideService(typeof(IDetailsWindowsManager), ServiceName = "IDetailsWindowManager")]
    [ProvideService(typeof(IDiagramsWindowsManager), ServiceName = "IDiagramsWindowsManager")]
    [ProvideService(typeof(NServiceBusDetailsToolWindow), ServiceName = "NServiceBusDetailsToolWindow")]
    [ProvideService(typeof(ServiceMatrixDiagramToolWindow), ServiceName = "NServiceBusDiagramsToolWindow")]
    [DslShell.ProvideBindingPathAttribute]
    public sealed class VSPackage : Package, IDetailsWindowsManager, IDiagramsWindowsManager
    {
        [Import]
        public ITraceOutputWindowManager TraceOutputWindowManager { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            AddServices();
            EnsureCreateTraceOutput();
            TrackUnhandledExceptions();
        }

        private void TrackUnhandledExceptions()
        {
            var reporter = new ExceptionReporter();
            reporter.Config.AppName = "ServiceMatrix";
            reporter.Config.AppVersion = "2.0.2";
            reporter.Config.CompanyName = "Particular Software";
            reporter.Config.ContactEmail = "contact@particular.net";
            reporter.Config.EmailReportAddress = "support@particular.net";
            reporter.Config.WebUrl = "http://particular.net";
            reporter.Config.TitleText = "ServiceMatrix - Unexpected error";
            reporter.Config.ShowFullDetail = false;
            reporter.Config.ShowLessMoreDetailButton = true;
            reporter.Config.ContactMessageTop = "[ServiceMatrix] Exception Report";

            var type = typeof(TraceSourceExtensions);
            var field = type.GetField("ShowExceptionAction", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, new Action<string, Exception>((s, e) =>
            {
                var customMessage = "";

                if (File.Exists(StatisticsManager.LoggingFile))
                {
                    using (var fileStream = new FileStream(StatisticsManager.LoggingFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var textReader = new StreamReader(fileStream))
                    {
                        customMessage = textReader.ReadToEnd();
                    }
                }

                if (!(e is ElementAlreadyExistsException))
                {
                    if (string.IsNullOrEmpty(customMessage))
                    {
                        reporter.Show(e);
                    }
                    else
                    {
                        reporter.Show(e.Message + Environment.NewLine + Environment.NewLine +
                                      "Additional Log:" + Environment.NewLine + customMessage, e);
                    }
                }
            }));
        }

        private void EnsureCreateTraceOutput()
        {
            // Creating Trace Output Window
            var componentModel = this.GetService<SComponentModel, IComponentModel>();
            componentModel.DefaultCompositionService.SatisfyImportsOnce(this);

            Trace.AutoFlush = true;

            var traceManager = Tracer.Manager as TracerManager;
            if (traceManager != null)
            {
                traceManager.SetTracingLevel(StatisticsManager.StatisticsListenerNamespace, SourceLevels.All);
            }

            TraceOutputWindowManager.CreateTracePane(new Guid("8678B5A5-9811-4D3E-921D-789E82C690D6"), "ServiceMatrix Logging", new[] { StatisticsManager.StatisticsListenerNamespace });
        }

        private void AddServices()
        {
            var serviceContainer = (IServiceContainer)this;
            serviceContainer.AddService(typeof(IDetailsWindowsManager), this, true);
            serviceContainer.AddService(typeof(IDiagramsWindowsManager), this, true);
        }

        void IDetailsWindowsManager.Show()
        {
            var window = EnsureCreateToolWindow<NServiceBusDetailsToolWindow>();
            if (window != null)
            {
                var frame = (IVsWindowFrame)window.Frame;
                frame.Show();
            }
        }

        void IDetailsWindowsManager.Enable()
        {
            EnableDisableDetailsPanel(true);
        }

        void IDetailsWindowsManager.Disable()
        {
            EnableDisableDetailsPanel(false);
        }

        void IDiagramsWindowsManager.Show()
        {
            var window = EnsureCreateToolWindow<ServiceMatrixDiagramToolWindow>();
            if (window != null)
            {
                var frame = (IVsWindowFrame)window.Frame;
                frame.Show();
            }
        }

        private void EnableDisableDetailsPanel(bool enable)
        {
            var window = EnsureCreateToolWindow<NServiceBusDetailsToolWindow>();
            if (window != null)
            {
                var content = (DetailsPanel)window.Content;
                content.IsEnabled = enable;
            }
        }

        T EnsureCreateToolWindow<T>() where T : class
        {
            var window = FindToolWindow(typeof(T), 0, false);
            if (window == null)
            {
                window = CreateToolWindow(typeof(T), 0).As<ToolWindowPane>();
                var serviceContainer = (IServiceContainer)this;
                serviceContainer.AddService(typeof(T), window.As<T>(), true);
            }

            return window as T;
        }
    }
}

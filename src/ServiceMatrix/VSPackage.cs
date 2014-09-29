using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using ExceptionReporting;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NServiceBusStudio.Automation.CustomSolutionBuilder;
using NServiceBusStudio.Automation.CustomSolutionBuilder.Views;
using NServiceBusStudio.Automation.Exceptions;
using NServiceBusStudio.Automation.Infrastructure;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Runtime;
using NuPattern.Runtime.Diagnostics;
using NuPattern.VisualStudio;
using ServiceMatrix.Diagramming;
using ServiceMatrix.Diagramming.Views;
using DslShell = Microsoft.VisualStudio.Modeling.Shell;

namespace NServiceBusStudio
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    [Guid(GuidList.guidNServiceBusStudioPkgString)]
    [ProvideToolWindow(typeof(NServiceBusDetailsToolWindow), Window = ToolWindowGuids.TaskList, Style = VsDockStyle.Tabbed, Transient = true)]
    [ProvideOptionPage(typeof(GeneralOptionsPage), "ServiceMatrix", "General", 0, 0, true)]
    [ProvideService(typeof(IDetailsWindowsManager), ServiceName = "IDetailsWindowManager")]
    [ProvideService(typeof(IDiagramsWindowsManager), ServiceName = "IDiagramsWindowsManager")]
    [ProvideService(typeof(NServiceBusDetailsToolWindow), ServiceName = "NServiceBusDetailsToolWindow")]
    [Description("ServiceMatrix")]
    [DslShell.ProvideBindingPathAttribute]
    [ProvideEditorFactory(typeof(ServiceMatrixDiagramEditorFactory), 500)]
    public sealed partial class VSPackage : Package, IDetailsWindowsManager, IDiagramsWindowsManager
    {
        [Import]
        public ITraceOutputWindowManager TraceOutputWindowManager { private get; set; }

        [Import]
        public IPatternManager PatternManager { private get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            AddServices();
            EnsureCreateTraceOutput();
            //TrackUnhandledExceptions();
            AdviseSolutionEvents();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnadviseSolutionEvents();
            }
        }

        private void TrackUnhandledExceptions()
        {
            var reporter = new ExceptionReporter();
            reporter.Config.AppName = ToolkitConstants.ToolkitName;
            reporter.Config.AppVersion = ToolkitConstants.Version;
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
            if (field != null)
            {
                field.SetValue(null, new Action<string, Exception>((s, e) =>
                {
#if DEBUG
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                        return;
                    }
#endif

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

            RegisterEditorFactory(new ServiceMatrixDiagramEditorFactory());
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
            ShowDiagramEditor(true, true);
        }

        const string canvasCaption = "ServiceMatrix - NServiceBus Canvas";

        void ShowDiagramEditor(bool create, bool forceActive)
        {
            if (PatternManager == null || !PatternManager.IsOpen)
            {
                return;
            }

            var editor = ServiceMatrix.Diagramming.Views.GuidList.ServiceMatrixDiagramEditorFactoryGuid;
            IVsUIHierarchy hierarchy;
            uint itemId;
            IVsWindowFrame frame;

            // check whether the slnbldr file is already open with the canvas editor
            Guid currentEditor;
            if (!VsShellUtilities.IsDocumentOpen(
                    this,
                    PatternManager.StoreFile,
                    VSConstants.LOGVIEWID_Primary,
                    out hierarchy,
                    out itemId,
                    out frame)
                || frame.GetGuidProperty((int)__VSFPROPID.VSFPROPID_guidEditorType, out currentEditor) != VSConstants.S_OK
                || currentEditor != editor)
            {
                if (!create)
                {
                    return;
                }

                // open the slnbldr file with the canvas editor
                VsShellUtilities.OpenDocumentWithSpecificEditor(
                    this,
                    PatternManager.StoreFile,
                    editor,
                    VSConstants.LOGVIEWID_Primary,
                    out hierarchy,
                    out itemId,
                    out frame);

                // override the default RDT settings so the editor does not participate of save/saveas and doesn't show
                // in the MRU list in the file menu
                OverrideDocumentFlags(frame);
            }

            ErrorHandler.ThrowOnFailure(frame.SetProperty((int)__VSFPROPID5.VSFPROPID_OverrideCaption, canvasCaption));
            if (forceActive)
            {
                ErrorHandler.ThrowOnFailure(frame.Show());
            }
        }

        private void OverrideDocumentFlags(IVsWindowFrame frame)
        {
            var table = (IVsRunningDocumentTable)GetService(typeof(SVsRunningDocumentTable));
            object cookie;
            ErrorHandler.ThrowOnFailure(frame.GetProperty((int)__VSFPROPID.VSFPROPID_DocCookie, out cookie));
            var pdwCookie = (uint)(int)cookie;
            if (pdwCookie != 0)
            {
                const uint lockType = (uint)(_VSRDTFLAGS.RDT_CantSave | _VSRDTFLAGS.RDT_DontAddToMRU);
                ErrorHandler.ThrowOnFailure(table.ModifyDocumentFlags(pdwCookie, lockType, 1));
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

        partial void AdviseSolutionEvents();

        partial void UnadviseSolutionEvents();
    }
}
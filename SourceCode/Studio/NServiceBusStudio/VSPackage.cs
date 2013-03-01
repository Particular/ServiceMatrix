using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.ComponentModel.Composition;
using NServiceBusStudio.Automation;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using NuPattern.Runtime;
using Microsoft.VisualStudio.ComponentModelHost;
using NServiceBusStudio.Automation.CustomSolutionBuilder.Views;
using NServiceBusStudio.Automation.CustomSolutionBuilder;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using NuPattern.Runtime.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Settings;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using System.Diagnostics;
using System.IO;

namespace NServiceBusStudio
{
    [ProvideToolWindow(typeof(NServiceBusDetailsToolWindow), Transient = false, MultiInstances = false, Style = VsDockStyle.Tabbed, Window = EnvDTE.Constants.vsWindowKindOutput)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidNServiceBusStudioPkgString)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ProvideService(typeof(IDetailsWindowsManager), ServiceName = "IDetailsWindowManager")]
    [ProvideService(typeof(NServiceBusDetailsToolWindow), ServiceName = "NServiceBusDetailsToolWindow")]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    public sealed class VSPackage : NServiceBus.Modeling.EndpointDesign.EndpointDesignPackage, IDetailsWindowsManager
    {
        [Import]
        public IShellEvents ShellEvents { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            this.AddServices();
            this.EnsureCreateToolWindow<NServiceBusDetailsToolWindow>();
            this.EnsureCreateTraceOutput();
        }

        private void EnsureCreateTraceOutput()
        {
            // Creating Trace Output Window
            var componentModel = this.GetService<SComponentModel, IComponentModel>();
            componentModel.DefaultCompositionService.SatisfyImportsOnce(this);

            var traceOutput = new TraceOutputWindowManager(this, this.ShellEvents, new Guid("8678B5A5-9811-4D3E-921D-789E82C690D6"), "NServiceBus Studio Logging", "NServiceBusStudio");
            
            Trace.AutoFlush = true;
            
            // Configuring Tracing Write File for Solution is closed
            var events = this.TryGetService<ISolutionEvents>();
            events.SolutionOpened += (s, e) => Tracer.AddListener("NServiceBusStudio", new TextWriterTraceListener(Path.ChangeExtension(e.Solution.PhysicalPath, "logging"), "SolutionTextWriterTraceListener"));
            events.SolutionClosed += (s, e) => Tracer.RemoveListener("NServiceBusStudio", "SolutionTextWriterTraceListener");
        }

        private void AddServices()
        {
            var serviceContainer = (IServiceContainer)this;
            serviceContainer.AddService(typeof(IDetailsWindowsManager), this, true);
        }

        void IDetailsWindowsManager.Show()
        {
            var window = this.FindToolWindow(typeof(NServiceBusDetailsToolWindow), 0, false);
            if (window != null)
            {
                var frame = (IVsWindowFrame)window.Frame;
                frame.Show();
            }
        }

        void EnsureCreateToolWindow<T>() 
            where T: class
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    var window = this.FindToolWindow(typeof(T), 0, false);
                    if (window == null)
                    {
                        try
                        {
                            window = this.CreateToolWindow(typeof(T), 0).As<ToolWindowPane>();

                        }
                        catch (Exception ex)
                        {
                            var s = ex.Message;
                            throw;
                        }
                    }
                    var serviceContainer = (IServiceContainer)this;
                    serviceContainer.AddService(typeof(T), window.As<T>(), true); 
                    var frame = (IVsWindowFrame)window.Frame;
                    //frame.Show();
                }));
        }
    }
}

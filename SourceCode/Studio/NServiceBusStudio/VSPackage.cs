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
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.ComponentModelHost;
using NServiceBusStudio.Automation.CustomSolutionBuilder.Views;
using NServiceBusStudio.Automation.CustomSolutionBuilder;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using Microsoft.VisualStudio.Patterning.Runtime.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Settings;
using System.ComponentModel.Design;

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
        protected override void Initialize()
        {
            base.Initialize();
            this.AddServices();
            this.EnsureCreateToolWindow<NServiceBusDetailsToolWindow>();
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

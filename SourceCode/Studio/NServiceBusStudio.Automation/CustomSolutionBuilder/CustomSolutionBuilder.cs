using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Patterning.Runtime.Shell;
using System.Windows.Controls;
using NServiceBusStudio.Automation.CustomSolutionBuilder.Views;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel.Composition;
using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;
using System.Threading;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder
{
    [Export]
    public class CustomSolutionBuilder
    {
        [Import]
        public ISolutionEvents SolutionEvents { get; set; }

        public static bool HasBeenAlreadyInitialized = false;

        public void Initialize(IServiceProvider serviceProvider)
        {
            if (HasBeenAlreadyInitialized)
            {
                if (this.ToolBarExtension != null)
                {
                    this.ToolBarExtension.CheckForEnablingSolution();
                }
                return;
            }
            HasBeenAlreadyInitialized = true;

            this.WireSolutionEvents();

            this.DetailsWindowManager = serviceProvider.TryGetService<IDetailsWindowsManager>();
            if (this.DetailsWindowManager != null)
            {
                this.DetailsWindowManager.Show();
            }
            SolutionBuilderToolWindow tw = null;

            var ptw = serviceProvider.GetService(typeof(IPackageToolWindow)) as IPackageToolWindow;

            tw = ptw.ShowWindow<SolutionBuilderToolWindow>();

            if (tw != null)
            {
                var content = tw.Content as UserControl;

                var contentGrid = content.Content as Grid;

                ScrollViewer scrollviewer = null;

                foreach (var theitem in contentGrid.Children)
                {
                    if (theitem is ScrollViewer)
                    {
                        scrollviewer = (theitem as ScrollViewer);
                    }
                }


                foreach (var item in contentGrid.Children)
                {
                    if (item is StackPanel)
                    {
                        foreach (var subitem in (item as StackPanel).Children)
                        {
                            if (subitem is StackPanel)
                            {
                                var toolbarSP = subitem as StackPanel;

                                this.ToolBarExtension = new ToolbarExtension
                                {
                                    ContentScrollViewer = scrollviewer,
                                    ServiceProvider = serviceProvider
                                }; 

                                toolbarSP.Children.Add(this.ToolBarExtension);
                            }
                        }

                    }
                }

                this.SolutionBuilderViewModel = scrollviewer.DataContext as Microsoft.VisualStudio.Patterning.Runtime.UI.SolutionBuilderViewModel;
            }
        }

        private Microsoft.VisualStudio.Patterning.Runtime.UI.SolutionBuilderViewModel SolutionBuilderViewModel;
        private ToolbarExtension ToolBarExtension;


        private void WireSolutionEvents()
        {
            this.SolutionEvents.SolutionClosed += (s, e) => 
            {
                if (this.ToolBarExtension != null)
                {
                    this.ToolBarExtension.ShowNoSolutionState();
                    //NServiceBusStudio.Automation.Tasks.NServiceBusTaskProgressToolWindow.ShowNoSolutionState();
                }
            };
            this.SolutionEvents.SolutionOpened += (s, e) => 
            {
                if (this.ToolBarExtension != null)
                {
                    var first = this.PatternManager.Store.Products.FirstOrDefault();
                    if (first != null && first.As<IApplication>() != null)
                    {
                        this.ToolBarExtension.CheckForEnablingSolution();
                    }
                }
            };

            Action refreshViews = () =>
            {
                if (LogicalViewModel.NServiceBusViewModel != null)
                {
                    LogicalViewModel.NServiceBusViewModel.RefreshViews();
                }
            };


            this.PatternManager.ElementInstantiated += (s, e) => refreshViews();
            this.PatternManager.ElementDeleted += (s, e) => refreshViews();

            this.PatternManager.ElementCreated += (s, e) => Application.ResetIsDirtyFlag();
            this.PatternManager.ElementDeleted += (s, e) => Application.ResetIsDirtyFlag();
            this.PatternManager.ElementInstantiated += (s, e) => Application.ResetIsDirtyFlag();
            this.PatternManager.PropertyChanged += (s, e) => Application.ResetIsDirtyFlag();
        }

        [Import]
        public IPatternManager PatternManager { get; set; }

        public IDetailsWindowsManager DetailsWindowManager { get; set; }


        #region Pattern Management Methods

        /// <summary>
        /// This function gets the MenuOptionViewModel instances from SolutionBuilder
        /// that can create new Endpoints
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IInstalledToolkitInfo> GetEndpointExtensionsInfo()
        {
            var r = this.SolutionBuilderViewModel.Nodes.First().Context.PatternManager.GetCandidateExtensionPoints("a5e9f15b-ad7f-4201-851e-186dd8db3bc9.Application.Design.Endpoints.Host");

            return r;
        }

        public void WaitForComponentsCreated(Action OnceCreatedAction, IService service, params string [] componentNames)
        {
            var dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            new Thread(() =>
                {
                    while (componentNames.Any(componentName => !service.Components.Component.Any(c => c.InstanceName == componentName)))
                    {
                        Thread.Sleep(1000);
                    }
                    dispatcher.Invoke(OnceCreatedAction);
                }).Start();
        }

        public void WaitForEventCreated(Action OnceCreatedAction, IService service, string eventName)
        {
            var dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            new Thread(() =>
            {
                while (!service.Contract.Events.Event.Any(e => e.InstanceName == eventName))
                {
                    Thread.Sleep(1000);
                }
                dispatcher.Invoke(OnceCreatedAction);
            }).Start();
        }

        #endregion
    }
}

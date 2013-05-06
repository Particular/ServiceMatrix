using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;
using NServiceBusStudio.Automation.CustomSolutionBuilder.Views;
using NuPattern;
using NuPattern.Runtime;
using NuPattern.Runtime.Shell.ToolWindows;
using NuPattern.Runtime.UI.ViewModels;
using NuPattern.VisualStudio.Solution;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows.Controls;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder
{
    [Export]
    public class CustomSolutionBuilder
    {
        [Import]
        public ISolutionEvents SolutionEvents { get; set; }
        
        public static bool HasBeenAlreadyInitialized = false;

        private ISolutionBuilderViewModel SolutionBuilderViewModel;
        private ToolbarExtension ToolBarExtension;
        private ScrollViewer Scrollviewer;

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

            tw = ptw.ShowWindow<SolutionBuilderToolWindow>(true);

            if (tw != null)
            {
                var content = tw.Content as UserControl;

                var contentGrid = content.Content as Grid;

                foreach (var theitem in contentGrid.Children)
                {
                    if (theitem is ScrollViewer)
                    {
                        Scrollviewer = (theitem as ScrollViewer);
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
                                    ContentScrollViewer = Scrollviewer,
                                    ServiceProvider = serviceProvider
                                }; 

                                toolbarSP.Children.Add(this.ToolBarExtension);
                            }
                        }

                    }
                }

                this.SolutionBuilderViewModel = Scrollviewer.DataContext as ISolutionBuilderViewModel;
            }
        }

        public void EnableSolutionBuilder()
        {
            this.Scrollviewer.IsEnabled = true;
            this.DetailsWindowManager.Enable();
            this.ToolBarExtension.Enable();
        }

        public void DisableSolutionBuilder()
        {
            this.Scrollviewer.IsEnabled = false;
            this.DetailsWindowManager.Disable();
            this.ToolBarExtension.Disable();
        }

        public void ShowNoSolutionState()
        {
            if (this.ToolBarExtension != null)
            {
                this.ToolBarExtension.ShowNoSolutionState();
                //NServiceBusStudio.Automation.Tasks.NServiceBusTaskProgressToolWindow.ShowNoSolutionState();
            }
        }

        private void WireSolutionEvents()
        {
            this.SolutionEvents.SolutionClosed += (s, e) => 
            {
                this.ShowNoSolutionState();
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

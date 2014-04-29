namespace NServiceBusStudio.Automation.CustomSolutionBuilder
{
    using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;
    using NServiceBusStudio.Automation.CustomSolutionBuilder.Views;
    using NuPattern;
    using NuPattern.Runtime;
    using NuPattern.VisualStudio.Solution;
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading;
    using System.Windows.Controls;
    using System.Windows.Threading;

    [Export]
    public class CustomSolutionBuilder
    {
        [Import]
        public ISolutionEvents SolutionEvents { get; set; }

        [Import]
        public IPatternWindows PatternWindows { get; set; }
        
        public static bool HasBeenAlreadyInitialized = false;

        private ToolbarExtension ToolBarExtension;
        private ScrollViewer Scrollviewer;

        public void Initialize(IServiceProvider serviceProvider)
        {
            if (HasBeenAlreadyInitialized)
            {
                if (ToolBarExtension != null)
                {
                    ToolBarExtension.CheckForEnablingSolution();
                }
                return;
            }
            HasBeenAlreadyInitialized = true;

            WireSolutionEvents();

            DetailsWindowManager = serviceProvider.TryGetService<IDetailsWindowsManager>();
            if (DetailsWindowManager != null)
            {
                DetailsWindowManager.Show();
            }
            
            PatternWindows.GetSolutionBuilderViewModel(serviceProvider);
            var toolWindow = PatternWindows.ShowSolutionBuilder(serviceProvider);

            if (toolWindow != null)
            {
                var content = toolWindow.Content as UserControl;

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

                                ToolBarExtension = new ToolbarExtension
                                {
                                    ContentScrollViewer = Scrollviewer,
                                    ServiceProvider = serviceProvider
                                }; 

                                toolbarSP.Children.Add(ToolBarExtension);
                            }
                        }

                    }
                }
            }
        }

        public void EnableSolutionBuilder()
        {
            Scrollviewer.IsEnabled = true;
            DetailsWindowManager.Enable();
            ToolBarExtension.Enable();
        }

        public void DisableSolutionBuilder()
        {
            Scrollviewer.IsEnabled = false;
            DetailsWindowManager.Disable();
            ToolBarExtension.Disable();
        }

        public void ShowNoSolutionState()
        {
            if (ToolBarExtension != null)
            {
                ToolBarExtension.ShowNoSolutionState();
                //NServiceBusStudio.Automation.Tasks.NServiceBusTaskProgressToolWindow.ShowNoSolutionState();
            }
        }

        private void WireSolutionEvents()
        {
            SolutionEvents.SolutionClosed += (s, e) => 
            {
                ShowNoSolutionState();
            };
            SolutionEvents.SolutionOpened += (s, e) => 
            {
                if (ToolBarExtension != null)
                {
                    var first = PatternManager.Store.Products.FirstOrDefault();
                    if (first != null && first.As<IApplication>() != null)
                    {
                        ToolBarExtension.CheckForEnablingSolution();
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


            PatternManager.ElementInstantiated += (s, e) => refreshViews();
            PatternManager.ElementDeleted += (s, e) => refreshViews();

            PatternManager.ElementCreated += (s, e) => Application.ResetIsDirtyFlag();
            PatternManager.ElementDeleted += (s, e) => Application.ResetIsDirtyFlag();
            PatternManager.ElementInstantiated += (s, e) => Application.ResetIsDirtyFlag();
            PatternManager.PropertyChanged += (s, e) => Application.ResetIsDirtyFlag();
        }

        [Import]
        public IPatternManager PatternManager { get; set; }

        public IDetailsWindowsManager DetailsWindowManager { get; set; }


        #region Pattern Management Methods

        public void WaitForComponentsCreated(Action OnceCreatedAction, IService service, params string [] componentNames)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            new Thread(() =>
                {
                    while (componentNames.Any(componentName => service.Components.Component.All(c => c.InstanceName != componentName)))
                    {
                        Thread.Sleep(1000);
                    }
                    dispatcher.Invoke(OnceCreatedAction);
                }).Start();
        }

        public void WaitForEventCreated(Action OnceCreatedAction, IService service, string eventName)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            new Thread(() =>
            {
                while (service.Contract.Events.Event.All(e => e.InstanceName != eventName))
                {
                    Thread.Sleep(1000);
                }
                dispatcher.Invoke(OnceCreatedAction);
            }).Start();
        }

        #endregion
    }
}

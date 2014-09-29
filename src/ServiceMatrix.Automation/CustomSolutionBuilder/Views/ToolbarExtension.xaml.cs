using System;
using System.Diagnostics;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using NServiceBusStudio.Automation.Commands;
using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;
using NServiceBusStudio.Automation.Extensions;
using NServiceBusStudio.Automation.Licensing;
using NuPattern;
using NuPattern.Runtime.UI.ViewModels;
using Process = System.Diagnostics.Process;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Views
{
    /// <summary>
    /// Interaction logic for ToolbarExtension.xaml
    /// </summary>
    partial class ToolbarExtension
    {
        public ToolbarExtension()
        {
            InitializeComponent();
        }

        public ScrollViewer ContentScrollViewer { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        private object DefaultSolutionBuilderView;
        private LogicalView NServiceBusView;
        private bool IsEnabledNServiceBusView;

        public void Enable()
        {
            EnableDisableNServiceBusView(true);
        }

        public void Disable()
        {
            EnableDisableNServiceBusView(false);
        }

        private void EnableDisableNServiceBusView(bool enable)
        {
            if (NServiceBusView != null)
            {
                NServiceBusView.IsEnabled = enable;
            }
            IsEnabledNServiceBusView = enable;
        }

        private void Canvas_Click(object sender, RoutedEventArgs e)
        {
            new ShowNewDiagramCommand { ServiceProvider = ServiceProvider }.Execute();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DefaultSolutionBuilderView == null)
            {
                DefaultSolutionBuilderView = ContentScrollViewer.Content;
            }

            if (NServiceBusView == null)
            {
                var SBdataContext = DataContext;
                LogicalViewModel.NServiceBusViewModel = new LogicalViewModel(SBdataContext as ISolutionBuilderViewModel);

                NServiceBusView = new LogicalView(LogicalViewModel.NServiceBusViewModel)
                    {
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                NServiceBusView.InitializeViewSelector();

                EnableDisableNServiceBusView(IsEnabledNServiceBusView);

                NServiceBusView.SelectedItemChanged += (s, f) =>
                {
                    if (s != null)
                    {
                        var selected = (s as LogicalViewModel.LogicalViewModelNode);
                        if (selected != null)
                        {
                            var fe = selected.InnerViewModel;
                            var window = (NServiceBusDetailsToolWindow)ServiceProvider.GetService(typeof(NServiceBusDetailsToolWindow));
                            if (window != null)
                            {
                                Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        ((DetailsPanel)window.Content).SetView(ServiceProvider, fe, SBdataContext);
                                        ((IVsWindowFrame)window.Frame).Show();
                                    }));
                            }
                        }
                    }
                };
            }
            else
            {
                NServiceBusView.DataContext = LogicalViewModel.NServiceBusViewModel;
            }

            ShowNServiceBusStudioView();
        }

        private void ShowNServiceBusStudioView(bool show = true)
        {
            var grid = (Grid)ContentScrollViewer.Parent;
            var window = (NServiceBusDetailsToolWindow)ServiceProvider.GetService(typeof(NServiceBusDetailsToolWindow));

            if (show)
            {
                ContentScrollViewer.Visibility = Visibility.Collapsed;
                if (!grid.Children.Contains(NServiceBusView))
                {
                    grid.Children.Add(NServiceBusView);
                    NServiceBusView.SetValue(Grid.RowProperty, 1);
                    if (window != null &&
                        (DataContext as ISolutionBuilderViewModel) != null &&
                        LogicalViewModel.NServiceBusViewModel.CurrentNode != null)
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            ((DetailsPanel)window.Content).SetView(ServiceProvider,
                            LogicalViewModel.NServiceBusViewModel.CurrentNode.InnerViewModel, DataContext);
                            ((IVsWindowFrame)window.Frame).Show();
                        }));
                    }
                }
            }
            else
            {
                if (grid.Children.Contains(NServiceBusView))
                {
                    grid.Children.Remove(NServiceBusView);
                }
                ContentScrollViewer.Visibility = Visibility.Visible;

                // Clear Details View
                if (window != null)
                {
                    ((DetailsPanel)window.Content).CleanAll();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (DefaultSolutionBuilderView == null)
            {
                DefaultSolutionBuilderView = ContentScrollViewer.Content;
            }
            else
            {
                ShowNServiceBusStudioView(false);
            }
        }

        internal void ShowNoSolutionState()
        {
            ShowNServiceBusStudioView(false);
            NServiceBusViewButton.IsEnabled = false;
            CanvasViewButton.IsEnabled = false;
            var window = (NServiceBusDetailsToolWindow)ServiceProvider.GetService(typeof(NServiceBusDetailsToolWindow));
            if (window != null)
            {
                ((DetailsPanel)window.Content).CleanAll();
            }
        }

        internal void CheckForEnablingSolution()
        {
            NServiceBusViewButton.IsEnabled = true;
            CanvasViewButton.IsEnabled = true;

            // Refresh Solution Builder View Model
            var SBdataContext = DataContext;
            LogicalViewModel.NServiceBusViewModel = new LogicalViewModel(SBdataContext as ISolutionBuilderViewModel);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var url = String.Format(@"http://particular.net/support?caller=serviceMatrix&smversion={0}&vsversion={1}",
                                    HttpUtility.UrlEncode(StatisticsInformation.GetOperatingSystemVersion()),
                                    HttpUtility.UrlEncode(StatisticsInformation.GetVisualStudioVersion(ServiceProvider.GetService<DTE>())));
            //var vsWebBroserService = this.ServiceProvider.GetService<SVsWebBrowsingService, IVsWebBrowsingService>();
            //var frame = default(IVsWindowFrame);
            //vsWebBroserService.Navigate(url, 1, out frame);

            Process.Start(new ProcessStartInfo(url));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            LicenseManager.Instance.PromptUserForLicenseIfTrialHasExpired();
            //LicenseManager.PromptUserForLicense(true);
        }
    }
}

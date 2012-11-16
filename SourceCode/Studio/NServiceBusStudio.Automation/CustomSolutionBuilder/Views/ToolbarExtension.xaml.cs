using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.Patterning.Runtime.UI;
using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Views
{
    /// <summary>
    /// Interaction logic for ToolbarExtension.xaml
    /// </summary>
    public partial class ToolbarExtension : UserControl
    {
        private ScrollViewer scrollviewer;

        public ToolbarExtension()
        {
            InitializeComponent();
        }

        public ScrollViewer ContentScrollViewer { get; set; }

        private object DefaultSolutionBuilderView = null;
        private LogicalView NServiceBusView = null;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.DefaultSolutionBuilderView == null)
            {
                this.DefaultSolutionBuilderView = this.ContentScrollViewer.Content;
            }

            if (NServiceBusView == null)
            {
                var SBdataContext = this.DataContext;
                LogicalViewModel.NServiceBusViewModel = new LogicalViewModel(SBdataContext as SolutionBuilderViewModel);

                NServiceBusView = new LogicalView(LogicalViewModel.NServiceBusViewModel)
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Stretch
                    };

                NServiceBusView.InitializeViewSelector();

                NServiceBusView.SelectedItemChanged += (s, f) =>
                {
                    if (s != null)
                    {
                        var selected = (s as LogicalViewModel.LogicalViewModelNode);
                        if (selected != null)
                        {
                            var fe = selected.InnerViewModel;
                            var window = (NServiceBusDetailsToolWindow)this.ServiceProvider.GetService(typeof(NServiceBusDetailsToolWindow));
                            if (window != null)
                            {
                                Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        (window.Content as DetailsPanel).SetView(this.ServiceProvider, fe, SBdataContext);
                                        ((Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame)window.Frame).Show();
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

            this.ShowNServiceBusStudioView(true);
        }

        private void ShowNServiceBusStudioView(bool show = true)
        {
            var grid = this.ContentScrollViewer.Parent as Grid;
            var window = (NServiceBusDetailsToolWindow)this.ServiceProvider.GetService(typeof(NServiceBusDetailsToolWindow));

            if (show)
            {
                this.ContentScrollViewer.Visibility = System.Windows.Visibility.Collapsed;
                if (!grid.Children.Contains(this.NServiceBusView))
                {
                    grid.Children.Add(this.NServiceBusView);
                    this.NServiceBusView.SetValue(Grid.RowProperty, 1);
                    if (window != null &&
                        (this.DataContext as SolutionBuilderViewModel) != null &&
                        NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels.LogicalViewModel.NServiceBusViewModel.CurrentNode != null)
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            (window.Content as DetailsPanel).SetView(this.ServiceProvider,
                            NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels.LogicalViewModel.NServiceBusViewModel.CurrentNode.InnerViewModel, this.DataContext);
                            ((Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame)window.Frame).Show();
                        }));
                    }
                }
            }
            else
            {
                if (grid.Children.Contains(this.NServiceBusView))
                {
                    grid.Children.Remove(this.NServiceBusView);
                }
                this.ContentScrollViewer.Visibility = System.Windows.Visibility.Visible;

                // Clear Details View
                if (window != null)
                {
                    (window.Content as DetailsPanel).CleanAll();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.DefaultSolutionBuilderView == null)
            {
                this.DefaultSolutionBuilderView = this.ContentScrollViewer.Content;
            }
            else
            {
                this.ShowNServiceBusStudioView(false);
            }
        }

        public IServiceProvider ServiceProvider { get; set; }

        internal void ShowNoSolutionState()
        {
            this.ShowNServiceBusStudioView(false);
            this.NServiceBusViewButton.IsEnabled = false;
            var window = (NServiceBusDetailsToolWindow)this.ServiceProvider.GetService(typeof(NServiceBusDetailsToolWindow));
            if (window != null)
            {
                (window.Content as DetailsPanel).CleanAll();
            }
        }

        internal void CheckForEnablingSolution()
        {
            this.NServiceBusViewButton.IsEnabled = true;
            
            // Refresh Solution Builder View Model
            var SBdataContext = this.DataContext;
            LogicalViewModel.NServiceBusViewModel = new LogicalViewModel(SBdataContext as SolutionBuilderViewModel);
        }
    }
}

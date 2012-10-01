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
using AbstractEndpoint;
using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.Shell;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Views
{
    /// <summary>
    /// Interaction logic for DetailsPanel.xaml
    /// </summary>
    public partial class DetailsPanel : UserControl
    {
        public DetailsPanel()
        {
            InitializeComponent();
            this.ViewModel = new DetailsPanelViewModel
            {
                 CleanDetails = CleanDetails,
                 SetPanel = SetPanel
            };
        }

        public DetailsPanelViewModel ViewModel { get; set; }

        public void SetView(IServiceProvider serviceProvider, ProductElementViewModel selectedElementViewModel, object logicalViewDataContext)
        {
            this.Caption = "NServiceBus Studio Details - " + selectedElementViewModel.Model.InstanceName;
            if (this.CaptionHasChanged != null)
            {
                this.CaptionHasChanged(this, EventArgs.Empty);
            }

            var model = selectedElementViewModel.Model;

            switch (model.DefinitionName)
            {
                case "Application":
                    this.ViewModel.BuildDetailsForApplication(model.As<IApplication>(), logicalViewDataContext as SolutionBuilderViewModel);
                    break;
                case "AbstractEndpoint":
                    this.ViewModel.BuildDetailsForEndpoint(model.As<IToolkitInterface>() as IAbstractEndpoint, logicalViewDataContext as SolutionBuilderViewModel);
                    break;
                case "Component":
                    this.ViewModel.BuildDetailsForComponent(model.As<IComponent>(), logicalViewDataContext as SolutionBuilderViewModel);
                    break;
                case "Command":
                    this.ViewModel.BuildDetailsForCommand(model.As<ICommand>(), logicalViewDataContext as SolutionBuilderViewModel);
                    break;
                case "Event":
                    this.ViewModel.BuildDetailsForEvent(model.As<IEvent>(), logicalViewDataContext as SolutionBuilderViewModel);
                    break;
                case "UseCase":
                    this.ViewModel.BuildDetailsForUseCase(model.As<IUseCase>(), logicalViewDataContext as SolutionBuilderViewModel);
                    break;
                case "Library":
                case "ServiceLibrary":
                    this.ViewModel.BuildDetailsForLibrary(model, logicalViewDataContext as SolutionBuilderViewModel);
                    break;
                default:
                    this.CleanDetails();
                    break;
            }
        }

        public void CleanDetails()
        {
            this.SetPanel(0, null);
            this.SetPanel(1, null);
            this.SetPanel(2, null);
            this.SetPanel(3, null);
            this.SetPanel(4, null);
        }

        public void SetPanel(int pos, FrameworkElement content)
        {
            Grid innerPanel = null;
            Border border = null;
            GridSplitter splitter = null;

            switch (pos)
            {
                case 0:
                    innerPanel = this.Panel1;
                    border = this.Border1;
                    break;
                case 1:
                    innerPanel = this.Panel2;
                    border = this.Border2;
                    splitter = Splitter1;
                    break;
                case 2:
                    innerPanel = this.Panel3;
                    border = this.Border3;
                    splitter = Splitter2;
                    break;
                case 3:
                    innerPanel = this.Panel4;
                    border = this.Border4;
                    splitter = Splitter3;
                    break;
                case 4:
                    innerPanel = this.Panel5;
                    border = this.Border5;
                    splitter = Splitter4;
                    break;
            }
            innerPanel.Children.Clear();
            if (content != null)
            {
                this.PanelsGrid.ColumnDefinitions[pos].Width = new GridLength(1, GridUnitType.Star);
                innerPanel.Children.Add(content);
                border.Visibility = System.Windows.Visibility.Visible;
                if (splitter != null)
                {
                    splitter.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else
            {
                this.PanelsGrid.ColumnDefinitions[pos].Width = new GridLength(0);
                border.Visibility = System.Windows.Visibility.Collapsed;
                if (splitter != null)
                {
                    splitter.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public string Caption { get; set; }
        public event EventHandler CaptionHasChanged;

        public void CleanAll()
        {
            this.Caption = "NServiceBus Studio Details";
            this.CleanDetails();
            if (this.CaptionHasChanged != null)
            {
                this.CaptionHasChanged(this, EventArgs.Empty);
            }
        }
    }
}

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using AbstractEndpoint;
    using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;
    using NuPattern.Runtime.ToolkitInterface;
    using NuPattern.Runtime.UI.ViewModels;

    /// <summary>
    /// Interaction logic for DetailsPanel.xaml
    /// </summary>
    public partial class DetailsPanel
    {
        public DetailsPanel()
        {
            InitializeComponent();
            ViewModel = new DetailsPanelViewModel
            {
                 CleanDetails = CleanDetails,
                 SetPanel = SetPanel
            };
        }

        public DetailsPanelViewModel ViewModel { get; set; }

        public void SetView(IServiceProvider serviceProvider, IProductElementViewModel selectedElementViewModel, object logicalViewDataContext)
        {
            Caption = "ServiceMatrix Details - " + selectedElementViewModel.Data.InstanceName;
            if (CaptionHasChanged != null)
            {
                CaptionHasChanged(this, EventArgs.Empty);
            }

            var model = selectedElementViewModel.Data;
            var definitionName = model.DefinitionName;
            switch (definitionName)
            {
                case "Application":
                    ViewModel.BuildDetailsForApplication(model.As<IApplication>(), logicalViewDataContext as ISolutionBuilderViewModel);
                    break;
                case "NServiceBusHost":
                case "NServiceBusMVC":
                case "NServiceBusWeb":
                    ViewModel.BuildDetailsForEndpoint(model.As<IToolkitInterface>() as IAbstractEndpoint, logicalViewDataContext as ISolutionBuilderViewModel);
                    break;
                case "Component":
                    ViewModel.BuildDetailsForComponent(model.As<IComponent>(), logicalViewDataContext as ISolutionBuilderViewModel);
                    break;
                case "Command":
                    ViewModel.BuildDetailsForCommand(model.As<ICommand>(), logicalViewDataContext as ISolutionBuilderViewModel);
                    break;
                case "Event":
                    ViewModel.BuildDetailsForEvent(model.As<IEvent>(), logicalViewDataContext as ISolutionBuilderViewModel);
                    break;
                case "UseCase":
                    ViewModel.BuildDetailsForUseCase(model.As<IUseCase>(), logicalViewDataContext as ISolutionBuilderViewModel);
                    break;
                case "Library":
                case "ServiceLibrary":
                    ViewModel.BuildDetailsForLibrary(model, logicalViewDataContext as ISolutionBuilderViewModel);
                    break;
                default:
                    CleanDetails();
                    break;
            }
        }

        public void CleanDetails()
        {
            SetPanel(0, null);
            SetPanel(1, null);
            SetPanel(2, null);
            SetPanel(3, null);
            SetPanel(4, null);
        }

        public void SetPanel(int pos, FrameworkElement content)
        {
            Grid innerPanel = null;
            Border border = null;
            GridSplitter splitter = null;

            switch (pos)
            {
                case 0:
                    innerPanel = Panel1;
                    border = Border1;
                    break;
                case 1:
                    innerPanel = Panel2;
                    border = Border2;
                    splitter = Splitter1;
                    break;
                case 2:
                    innerPanel = Panel3;
                    border = Border3;
                    splitter = Splitter2;
                    break;
                case 3:
                    innerPanel = Panel4;
                    border = Border4;
                    splitter = Splitter3;
                    break;
                case 4:
                    innerPanel = Panel5;
                    border = Border5;
                    splitter = Splitter4;
                    break;
            }
            innerPanel.Children.Clear();
            if (content != null)
            {
                PanelsGrid.ColumnDefinitions[pos].Width = new GridLength(1, GridUnitType.Star);
                innerPanel.Children.Add(content);
                border.Visibility = Visibility.Visible;
                if (splitter != null)
                {
                    splitter.Visibility = Visibility.Visible;
                }
            }
            else
            {
                PanelsGrid.ColumnDefinitions[pos].Width = new GridLength(0);
                border.Visibility = Visibility.Collapsed;
                if (splitter != null)
                {
                    splitter.Visibility = Visibility.Collapsed;
                }
            }
        }

        public string Caption { get; set; }
        public event EventHandler CaptionHasChanged;

        public void CleanAll()
        {
            Caption = "ServiceMatrix Details";
            CleanDetails();
            if (CaptionHasChanged != null)
            {
                CaptionHasChanged(this, EventArgs.Empty);
            }
        }
    }
}

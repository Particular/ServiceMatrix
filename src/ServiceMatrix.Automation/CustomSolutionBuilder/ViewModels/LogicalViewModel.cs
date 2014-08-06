using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using NuPattern.Runtime.UI;
using System.ComponentModel;
using NuPattern.Runtime;
using System.Collections.Specialized;
using System.Windows.Threading;
using NuPattern.Presentation;
using NuPattern.Runtime.UI.ViewModels;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels
{
    public class LogicalViewModel : INotifyPropertyChanged
    {
        // Fields
        private LogicalViewModelNode currentNode;

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Methods
        public LogicalViewModel(ISolutionBuilderViewModel sourceViewModel)
        {
            this.WireSolutionBuilderViewModel(sourceViewModel, false);

            //This is the Master View:
            this.IsMasterView = true;
            // 1. The selected node should be trackable
            this.TrackSelectedNode = true;
            // 2. We need to create the views list
            this.BuildViewsList();
        }

        public LogicalViewModel(ISolutionBuilderViewModel sourceViewModel, InnerPanelViewModel innerView)
        {
            this.Title = innerView.Title;
            this.WireSolutionBuilderViewModel(sourceViewModel, true);
            this.LogicalViewNodes = new ObservableCollection<LogicalViewModelNode>();
            IProductElementViewModel root = null;
            IEnumerable<string> menuFilters = null;
            InnerPanelItem rootItem = null;
            ObservableCollection<IProductElementViewModel> children = new ObservableCollection<IProductElementViewModel>();
            foreach (InnerPanelItem item in innerView.Items)
            {
                if (item is InnerPanelTitle)
                {
                    if (root != null)
                    {
                        var rootNode = new LogicalViewModelNode(this, root, children) { CustomIconPath = rootItem.IconPath };
                        if (menuFilters != null)
                        {
                            rootNode.FilterMenuItems(menuFilters.ToArray());
                        }
                        this.LogicalViewNodes.Add(rootNode);
                    }
                    rootItem = item;
                    root = this.GetProductNode(item);
                    menuFilters = (item as InnerPanelTitle).MenuFilters;
                    children = new ObservableCollection<IProductElementViewModel>();
                }
                else
                {
                    children.Add(this.GetProductNode(item));
                }
            }
            if (root != null)
            {
                var rootNode = new LogicalViewModelNode(this, root, children) { CustomIconPath = rootItem.IconPath };
                if (menuFilters != null)
                {
                    rootNode.FilterMenuItems(menuFilters.ToArray());
                }
                this.LogicalViewNodes.Add(rootNode);
            }
        }

        private void ActivateNode()
        {
            if (this.currentNode == null)
            {
                // Search for current node
                var found = this.FindLogicalNode(this.LogicalViewNodes, n => n.InnerViewModel.IsSelected);
                if (found != null)
                {
                    this.CurrentNode = found;
                }
                else
                {
                    return;
                }
            }

            this.SourceViewModel.TopLevelNodes[0].Context.PatternManager.ActivateElement(this.currentNode.InnerViewModel.Data);
            var innerModelToSelect = this.currentNode.InnerViewModel;

            if (LogicalViewModel.NServiceBusViewModel != this && this.currentNode.InnerViewModel.Data != null)
            {

                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                    {
                        string definitionName = this.currentNode.InnerViewModel.Data.DefinitionName.ToString();
                        switch (definitionName)
                        {
                            case "NServiceBusHost":
                            case "NServiceBusMVC":
                                NServiceBusViewModel.GenerateEndpointsView();
                                break;
                            case "Component":
                                NServiceBusViewModel.GenerateComponentsView();
                                break;
                            case "UseCase":
                                NServiceBusViewModel.GenerateUseCasesView();
                                break;
                            case "Command":
                            case "Event":
                                NServiceBusViewModel.GenerateMessagesView();
                                break;
                        }

                        var nodeToSelect =
                            LogicalViewModel.NServiceBusViewModel.FindLogicalNode(
                                LogicalViewModel.NServiceBusViewModel.LogicalViewNodes,
                                n => n.InnerViewModel == innerModelToSelect);
                        if (nodeToSelect != null)
                        {
                            nodeToSelect.IsSelected = true;
                            NServiceBusViewModel.CollapseAllButSelected();
                        }

                        NServiceBusViewModel.RaiseOnPropertyChanged("Title");
                    }));
                this.CurrentNode.InnerViewModel.IsSelected = false;
            }
        }

        private void RaiseOnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private LogicalViewModelNode FindLogicalNode(ObservableCollection<LogicalViewModelNode> observableCollection,
            Func<LogicalViewModelNode, bool> Condition)
        {
            foreach (LogicalViewModelNode node in observableCollection)
            {
                if (Condition(node))
                {
                    return node;
                }
                LogicalViewModelNode node2 = this.FindLogicalNode(node.LogicalViewNodes, Condition);
                if (node2 != null)
                {
                    return node2;
                }
            }
            return null;
        }

        private void GenerateCommands()
        {
            this.ActivateCommand = new RelayCommand(new Action(this.ActivateNode), () => true);
            this.LeftCommand = new RelayCommand(new Action(() => this.RollView(true)), () => true);
            this.RightCommand = new RelayCommand(new Action(() => this.RollView(false)), () => true);
        }

        private void RollView(bool left)
        {
            if (this == LogicalViewModel.NServiceBusViewModel)
            {
                var index = this.ViewsList.IndexOf(this.SelectedView);

                index += left ? -1 : +1;

                if (index < 0) { index = this.ViewsList.Count - 1; }
                if (index >= this.ViewsList.Count) { index = 0; }

                this.SelectedView = this.ViewsList[index];

                if (this.FocusOnViewRequested != null)
                {
                    this.FocusOnViewRequested(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler FocusOnViewRequested;

        internal void GenerateUseCasesView(bool setTitle = true)
        {
            if (setTitle)
            {
                this.SelectView("Use Cases");
            }
            ObservableCollection<LogicalViewModelNode> observables = new ObservableCollection<LogicalViewModelNode>();
            LogicalViewModelNode usecasesItem = new LogicalViewModelNode(this, this.SourceViewModel.TopLevelNodes.First().ChildNodes.Named("Use Cases"),
                this.SourceViewModel.TopLevelNodes.First().ChildNodes.Named("Use Cases").ChildNodes);
            observables.Add(new LogicalViewModelNode(this, this.SourceViewModel.TopLevelNodes.First<IProductElementViewModel>(), null));
            observables[0].LogicalViewNodes.Add(usecasesItem);

            // Add UseCases Node with just the Create New Use Case option
            observables[0].LogicalViewNodes[0].FilterMenuItems("Add");

            // Add Use Case -> Edit and Delete options
            foreach (var endpoint in usecasesItem.LogicalViewNodes)
            {
                endpoint.FilterMenuItems("Add Started By Endpoint", "Delete");
            }

            this.LogicalViewNodes = observables;
        }

        internal void GenerateEndpointsView(bool setTitle = true)
        {
            if (setTitle)
            {
                this.SelectView("Endpoints");
            }
            ObservableCollection<LogicalViewModelNode> observables = new ObservableCollection<LogicalViewModelNode>();
            LogicalViewModelNode endpointsItem = new LogicalViewModelNode(this, this.SourceViewModel.TopLevelNodes.First().ChildNodes.Named("Endpoints"),
                this.SourceViewModel.TopLevelNodes.First().ChildNodes.Named("Endpoints").ChildNodes);
            observables.Add(new LogicalViewModelNode(this, this.SourceViewModel.TopLevelNodes.First<IProductElementViewModel>(), null));
            observables[0].LogicalViewNodes.Add(endpointsItem);

            // Add Endpoints Node with just the Add->Nodes option
            observables[0].LogicalViewNodes[0].FilterMenuItems("Show Diagram", "Add");

            // Add Endpoint -> Add -> Component menu item
            foreach (var endpoint in endpointsItem.LogicalViewNodes)
            {
                endpoint.FilterMenuItems("Show Diagram", "Customize Authentication", "Add to Use Case", "Start Use Case");
                endpoint.MenuOptions.Add(endpoint.InnerViewModel.ChildNodes.Named("Components").MenuOptions.First(o => o.Caption == "Deploy Component..."));
            }

            this.LogicalViewNodes = observables;
        }

        public List<NServiceBusView> ViewsList { get; set; }

        public class NServiceBusView
        {
            public string Text { get; set; }
            public Action Action { get; set; }

            public override string ToString()
            {
                return this.Text;
            }
        }

        private NServiceBusView selectedView;
        public NServiceBusView SelectedView
        {
            get { return selectedView; }
            set
            {
                this.selectedView = value;
                this.selectedView.Action();
                this.RaiseOnPropertyChanged("SelectedView");
            }
        }

        public bool IsMasterView { get; private set; }

        private void SelectView(string title)
        {
            this.SelectedView = this.ViewsList.First(v => v.Text.Contains(title));
        }

        public void BuildViewsList()
        {
            this.ViewsList = new List<NServiceBusView> {
                new NServiceBusView { Text = "Endpoints View", Action = () => this.GenerateEndpointsView(false)},
                //new NServiceBusView { Text = "Use Cases View", Action = () => this.GenerateUseCasesView(false) },
                new NServiceBusView { Text = "Components View", Action = () => this.GenerateComponentsView(false) },
                new NServiceBusView { Text = "Messages View", Action = () => this.GenerateMessagesView(false) },
            };

            this.RaiseOnPropertyChanged("ViewsList");
            this.SelectView("Endpoints");
        }

        internal void GenerateComponentsView(bool setTitle = true)
        {
            if (setTitle)
            {
                this.SelectView("Components");
            }
            ObservableCollection<LogicalViewModelNode> observables = new ObservableCollection<LogicalViewModelNode>();

            var services = this.SourceViewModel.TopLevelNodes.First().ChildNodes.First(n => n.Data.DefinitionName == "Services");
            
            // Add Services Node with just the Add->Service option
            var servicesNode = new LogicalViewModelNode(this, services, services.ChildNodes);
            servicesNode.FilterMenuItems("Add");
            observables.Add(servicesNode);

            foreach (var service in observables[0].LogicalViewNodes)
            {
                // Add menu option "Add -> Component" from first child
                //service.MenuOptions = new ObservableCollection<IMenuOptionViewModel>();
                service.FilterMenuItems("Delete");
                service.MenuOptions.Add(new MenuOptionViewModel("Add",
                    new List<MenuOptionViewModel>{ 
                        service.InnerViewModel.ChildNodes.Named("Components").MenuOptions.First(o => o.Caption == "Add").MenuOptions.First() as MenuOptionViewModel
                    }));
                foreach (var component in service.InnerViewModel.ChildNodes.Named("Components").ChildNodes)
                {
                    service.LogicalViewNodes.Add(new LogicalViewModelNode(this, component, null));
                }
            }

            this.LogicalViewNodes = observables;
        }

        internal void GenerateMessagesView(bool setTitle = true)
        {
            if (setTitle)
            {
                this.SelectView("Messages");
            }
            ObservableCollection<LogicalViewModelNode> observables = new ObservableCollection<LogicalViewModelNode>();

            var services = this.SourceViewModel.TopLevelNodes.First().ChildNodes.First(n => n.Data.DefinitionName == "Services");

            // Add Services Node with just the Add->Service option
            var servicesNode = new LogicalViewModelNode(this, services, services.ChildNodes);
            servicesNode.FilterMenuItems("Add");

            observables.Add(servicesNode);

            foreach (var service in observables[0].LogicalViewNodes)
            {
                // Adding menu options for "Add->Commands" and "Add->Events"
                //service.MenuOptions = new ObservableCollection<IMenuOptionViewModel>();
                service.FilterMenuItems("Delete");

                service.MenuOptions.Add(new MenuOptionViewModel("Add",
                    new List<MenuOptionViewModel>{ 
                        service.InnerViewModel.ChildNodes.Named("Contract").ChildNodes.Named("Commands").MenuOptions.First(o => o.Caption == "Add").MenuOptions.First() as MenuOptionViewModel,
                        service.InnerViewModel.ChildNodes.Named("Contract").ChildNodes.Named("Events").MenuOptions.First(o => o.Caption == "Add").MenuOptions.First() as MenuOptionViewModel
                    }));

                foreach (var component in service.InnerViewModel.ChildNodes
                    .First(n => n.Data.DefinitionName == "Contract").ChildNodes
                    .First(n => n.Data.DefinitionName == "Commands").ChildNodes
                    .Union(service.InnerViewModel.ChildNodes
                    .First(n => n.Data.DefinitionName == "Contract").ChildNodes
                    .First(n => n.Data.DefinitionName == "Events").ChildNodes))
                {
                    service.LogicalViewNodes.Add(new LogicalViewModelNode(this, component, null));
                }
            }

            this.LogicalViewNodes = observables;
        }

        private void CollapseAllButSelected()
        {
            this.FindLogicalNode(this.LogicalViewNodes, n =>
                {
                    if (this.FindLogicalNode(n.LogicalViewNodes, m => m.InnerViewModel.IsSelected) == null)
                    {
                        n.Collapse();
                    }
                    return false;
                });
        }

        private IProductElementViewModel GetProductNode(InnerPanelItem item)
        {
            IProductElement target = item.Product;
            string label = item.Text;
            IProductElementViewModel model = this.SearchInNodes(this.SourceViewModel.TopLevelNodes, target);
            if (model == null)
            {
                var element = this.SourceViewModel.TopLevelNodes[0].Data.As<IApplication>().Design.DummyCollection.As<IAbstractElement>();
                var ctx = this.SourceViewModel.TopLevelNodes[0].Context;
                LabelElementViewModel model2 = new LabelElementViewModel(element, ctx)
                {
                    Label = label
                };
                model = model2;
            }
            else if (item is InnerPanelTitle && (item as InnerPanelTitle).ForceText)
            {
                var ctx = this.SourceViewModel.TopLevelNodes[0].Context;
                if (model.Data is IAbstractElement)
                {
                    LabelElementViewModel model2 = new LabelElementViewModel(model.Data.As<IAbstractElement>(), ctx)
                    {
                        Label = item.Text
                    };
                    model = model2;
                }
                //else
                //{
                //    LabelProductElementViewModel model2 = new LabelProductElementViewModel(model.Data.As<IProduct>(), ctx)
                //    {
                //        Label = item.Text
                //    };
                //    model = model2;
                //}
            }
            return model;
        }

        private void OnCurrentNodeChanged()
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("CurrentNode"));
            }
        }

        private IProductElementViewModel SearchInNodes(ObservableCollection<IProductElementViewModel> observableCollection, IProductElement target)
        {
            foreach (IProductElementViewModel model in observableCollection)
            {
                if (model.Data == target)
                {
                    return model;
                }
                IProductElementViewModel model2 = this.SearchInNodes(model.ChildNodes, target);
                if (model2 != null)
                {
                    return model2;
                }
            }
            return null;
        }

        private void WireSolutionBuilderViewModel(ISolutionBuilderViewModel sourceViewModel, bool wireCurrentNode)
        {
            this.SourceViewModel = sourceViewModel;
            this.GenerateCommands();
            if (wireCurrentNode)
            {
                this.SourceViewModel.CurrentNodeChanged += (s, e) =>
                {
                    if (this.LogicalViewNodes != null)
                    {
                        this.CurrentNode = this.FindLogicalNode(this.LogicalViewNodes, n => n.InnerViewModel == this.SourceViewModel.CurrentElement);
                        this.OnCurrentNodeChanged();
                    }
                };
            }
        }

        public void RefreshViews()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        var currentInnerNode = this.CurrentNode.InnerViewModel;
                        this.selectedView.Action();
                        var nodeToSelect = this.FindLogicalNode(this.LogicalViewNodes, n => n.InnerViewModel == currentInnerNode);
                        if (nodeToSelect != null)
                        {
                            nodeToSelect.IsSelected = true;
                        }
                    }
                    catch { }
                }));
        }

        // Properties
        public System.Windows.Input.ICommand ActivateCommand { get; private set; }
        public System.Windows.Input.ICommand LeftCommand { get; private set; }
        public System.Windows.Input.ICommand RightCommand { get; private set; }

        public LogicalViewModelNode CurrentNode
        {
            get
            {
                return this.currentNode;
            }
            private set
            {
                if (this.currentNode != value)
                {
                    if (((this.currentNode != null) && (this.currentNode.InnerViewModel != null)) && this.currentNode.InnerViewModel.IsEditing)
                    {
                        //this.currentNode.InnerViewModel.EndEdit();
                    }
                    this.currentNode = value;
                    this.OnCurrentNodeChanged();
                }
            }
        }

        public bool TrackSelectedNode { get; set; }

        private ObservableCollection<LogicalViewModelNode> logicalViewNodes;
        public ObservableCollection<LogicalViewModelNode> LogicalViewNodes
        {
            get { return logicalViewNodes; }
            private set
            {
                if (value != logicalViewNodes)
                {
                    this.logicalViewNodes = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("LogicalViewNodes"));
                    }
                }
            }
        }


        public static LogicalViewModel NServiceBusViewModel { get; set; }

        public ISolutionBuilderViewModel SourceViewModel { get; set; }

        private string title;

        public string Title
        {
            get { return this.title; }
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.RaiseOnPropertyChanged("Title");
                }
            }
        }

        // Nested Types
        public class LogicalViewModelNode : INotifyPropertyChanged
        {
            public string CustomIconPath { get; set; }
            //string _customIconPath;
            //public string CustomIconPath
            //{
            //    get
            //    {
            //        if (this._customIconPath == null)
            //        {
            //            return this.InnerViewModel.IconPath;
            //        }
            //        else
            //            return this._customIconPath;
            //    }
            //    set
            //    {
            //        _customIconPath = value;
            //    }
            //}
            public string IconPath
            {
                get
                {
                    if (this.CustomIconPath == null)
                    {
                        return this.InnerViewModel.IconPath;
                    }
                    else
                        return this.CustomIconPath;
                }
            }

            private ObservableCollection<IMenuOptionViewModel> overridableMenuOptions;

            public ObservableCollection<IMenuOptionViewModel> MenuOptions
            {
                get
                {
                    return overridableMenuOptions ?? this.InnerViewModel.MenuOptions;
                }
                set
                {
                    this.overridableMenuOptions = value;
                    this.RaisePropertyChanged("MenuOptions");
                }
            }

            // Methods
            public LogicalViewModelNode(
                LogicalViewModel viewModel,
                IProductElementViewModel root,
                ObservableCollection<IProductElementViewModel> children
                )
            {
                this.IsExpanded = true;
                this.ViewModel = viewModel;
                WireInnerViewModel(root);

                if (children != null)
                {
                    Func<IProductElementViewModel, LogicalViewModelNode> LogicalViewCreator = n =>
                        {
                            return new LogicalViewModelNode(viewModel, n, null);
                        };
                    this.LogicalViewNodes = new ObservableCollection<LogicalViewModelNode>(children.Select(n => LogicalViewCreator(n)));
                }
                else
                {
                    this.LogicalViewNodes = new ObservableCollection<LogicalViewModel.LogicalViewModelNode>();
                }

                if (!viewModel.IsMasterView)
                {
                    this.RemoveDeleteMenuItem();   
                }
            }

            private void WireInnerViewModel(IProductElementViewModel root)
            {
                this.InnerViewModel = root;
                this.InnerViewModel.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == "IsSelected")
                        {
                            // this.IsSelected = this.InnerViewModel.IsSelected;
                            if (!this.IsNServiceBusView)
                            {
                                this.RaisePropertyChanged("IsSelected");
                            }
                        }
                    };
            }

            public void FilterMenuItems(params string[] captions)
            {
                var sourceMenuOptions = this.InnerViewModel.MenuOptions;
                if (this.overridableMenuOptions != null)
                {
                    sourceMenuOptions = this.MenuOptions;
                }
                this.MenuOptions = new ObservableCollection<IMenuOptionViewModel>();
                foreach (var menuOption in sourceMenuOptions.Where(o => captions.Contains(o.Caption)))
                {
                    this.MenuOptions.Add(menuOption);
                }
            }

            internal void RemoveDeleteMenuItem()
            {
                this.MenuOptions = new ObservableCollection<IMenuOptionViewModel>(this.MenuOptions.Where(o => o.Caption != "Delete"));
            }

            // Properties
            public IProductElementViewModel InnerViewModel
            {
                get;
                private set;
            }

            public ObservableCollection<LogicalViewModel.LogicalViewModelNode> LogicalViewNodes { get; set; }

            public bool IsExpanded { get; set; }

            public void Collapse()
            {
                this.IsExpanded = false;
                this.RaisePropertyChanged("IsExpanded");
            }

            public void RaisePropertyChanged(string name)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private bool isSelected;

            private bool IsNServiceBusView
            {
                get
                {
                    return LogicalViewModel.NServiceBusViewModel == this.ViewModel;
                }
            }

            public bool IsSelected
            {
                get
                {
                    return this.IsNServiceBusView ? this.isSelected : this.InnerViewModel.IsSelected;
                }
                set
                {
                    if (this.IsNServiceBusView)
                    {
                        if (this.isSelected != value)
                        {
                            this.InnerViewModel.IsSelected = value;
                            this.isSelected = value;
                            this.RaisePropertyChanged("IsSelected");
                        }
                        if (this.isSelected)
                        {
                            this.ViewModel.CurrentNode = this;
                        }
                    }
                    else
                    {
                        this.InnerViewModel.IsSelected = value;
                    }
                }
            }

            public LogicalViewModel ViewModel { get; private set; }
        }

    }

    public static class ProductElementViewModelExtensions
    {
        public static IProductElementViewModel Named(this ObservableCollection<IProductElementViewModel> items, string name)
        {
            return items.First(n => n.Data.InstanceName == name);
        }
    }
}

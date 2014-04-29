using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NuPattern.Runtime;
using System.Windows.Threading;
using NuPattern.Presentation;
using NuPattern.Runtime.UI.ViewModels;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels
{
    using System.Windows.Input;

    public class LogicalViewModel : INotifyPropertyChanged
    {
        // Fields
        private LogicalViewModelNode currentNode;

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Methods
        public LogicalViewModel(ISolutionBuilderViewModel sourceViewModel)
        {
            WireSolutionBuilderViewModel(sourceViewModel, false);

            //This is the Master View:
            IsMasterView = true;
            // 1. The selected node should be trackable
            TrackSelectedNode = true;
            // 2. We need to create the views list
            BuildViewsList();
        }

        public LogicalViewModel(ISolutionBuilderViewModel sourceViewModel, InnerPanelViewModel innerView)
        {
            Title = innerView.Title;
            WireSolutionBuilderViewModel(sourceViewModel, true);
            LogicalViewNodes = new ObservableCollection<LogicalViewModelNode>();
            IProductElementViewModel root = null;
            IEnumerable<string> menuFilters = null;
            InnerPanelItem rootItem = null;
            var children = new ObservableCollection<IProductElementViewModel>();
            foreach (var item in innerView.Items)
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
                        LogicalViewNodes.Add(rootNode);
                    }
                    rootItem = item;
                    root = GetProductNode(item);
                    menuFilters = (item as InnerPanelTitle).MenuFilters;
                    children = new ObservableCollection<IProductElementViewModel>();
                }
                else
                {
                    children.Add(GetProductNode(item));
                }
            }
            if (root != null)
            {
                var rootNode = new LogicalViewModelNode(this, root, children) { CustomIconPath = rootItem.IconPath };
                if (menuFilters != null)
                {
                    rootNode.FilterMenuItems(menuFilters.ToArray());
                }
                LogicalViewNodes.Add(rootNode);
            }
        }

        private void ActivateNode()
        {
            if (currentNode == null)
            {
                // Search for current node
                var found = FindLogicalNode(LogicalViewNodes, n => n.InnerViewModel.IsSelected);
                if (found != null)
                {
                    CurrentNode = found;
                }
                else
                {
                    return;
                }
            }

            SourceViewModel.TopLevelNodes[0].Context.PatternManager.ActivateElement(currentNode.InnerViewModel.Data);
            var innerModelToSelect = currentNode.InnerViewModel;

            if (NServiceBusViewModel != this && currentNode.InnerViewModel.Data != null)
            {

                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                    {
                        var definitionName = currentNode.InnerViewModel.Data.DefinitionName;
                        switch (definitionName)
                        {
                            case "NServiceBusHost":
                            case "NServiceBusMVC":
                            case "NServiceBusWeb":
                                NServiceBusViewModel.GenerateEndpointsView();
                                break;
                            case "Component":
                                NServiceBusViewModel.GenerateComponentsView();
                                break;
                            case "UseCase":
                                NServiceBusViewModel.GenerateUseCasesView();
                                break;
                            case "Library":
                            case "ServiceLibrary":
                                NServiceBusViewModel.GenerateLibrariesView();
                                break;
                            case "Command":
                            case "Event":
                                NServiceBusViewModel.GenerateMessagesView();
                                break;
                        }

                        var nodeToSelect =
                            NServiceBusViewModel.FindLogicalNode(
                                NServiceBusViewModel.LogicalViewNodes,
                                n => n.InnerViewModel == innerModelToSelect);
                        if (nodeToSelect != null)
                        {
                            nodeToSelect.IsSelected = true;
                            NServiceBusViewModel.CollapseAllButSelected();
                        }

                        NServiceBusViewModel.RaiseOnPropertyChanged("Title");
                    }));
                CurrentNode.InnerViewModel.IsSelected = false;
            }
        }

        private void RaiseOnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private LogicalViewModelNode FindLogicalNode(ObservableCollection<LogicalViewModelNode> observableCollection,
            Func<LogicalViewModelNode, bool> Condition)
        {
            foreach (var node in observableCollection)
            {
                if (Condition(node))
                {
                    return node;
                }
                var node2 = FindLogicalNode(node.LogicalViewNodes, Condition);
                if (node2 != null)
                {
                    return node2;
                }
            }
            return null;
        }

        private void GenerateCommands()
        {
            ActivateCommand = new RelayCommand(new Action(ActivateNode), () => true);
            LeftCommand = new RelayCommand(new Action(() => RollView(true)), () => true);
            RightCommand = new RelayCommand(new Action(() => RollView(false)), () => true);
        }

        private void RollView(bool left)
        {
            if (this == NServiceBusViewModel)
            {
                var index = ViewsList.IndexOf(SelectedView);

                index += left ? -1 : +1;

                if (index < 0) { index = ViewsList.Count - 1; }
                if (index >= ViewsList.Count) { index = 0; }

                SelectedView = ViewsList[index];

                if (FocusOnViewRequested != null)
                {
                    FocusOnViewRequested(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler FocusOnViewRequested;

        internal void GenerateUseCasesView(bool setTitle = true)
        {
            if (setTitle)
            {
                SelectView("Use Cases");
            }
            var observables = new ObservableCollection<LogicalViewModelNode>();
            var usecasesItem = new LogicalViewModelNode(this, SourceViewModel.TopLevelNodes.First().ChildNodes.Named("Use Cases"),
                SourceViewModel.TopLevelNodes.First().ChildNodes.Named("Use Cases").ChildNodes);
            observables.Add(new LogicalViewModelNode(this, SourceViewModel.TopLevelNodes.First<IProductElementViewModel>(), null));
            observables[0].LogicalViewNodes.Add(usecasesItem);

            // Add UseCases Node with just the Create New Use Case option
            observables[0].LogicalViewNodes[0].FilterMenuItems("Add");

            // Add Use Case -> Edit and Delete options
            foreach (var endpoint in usecasesItem.LogicalViewNodes)
            {
                endpoint.FilterMenuItems("Add Started By Endpoint", "Delete");
            }

            LogicalViewNodes = observables;
        }

        internal void GenerateEndpointsView(bool setTitle = true)
        {
            if (setTitle)
            {
                SelectView("Endpoints");
            }
            var observables = new ObservableCollection<LogicalViewModelNode>();
            var endpointsItem = new LogicalViewModelNode(this, SourceViewModel.TopLevelNodes.First().ChildNodes.Named("Endpoints"),
                SourceViewModel.TopLevelNodes.First().ChildNodes.Named("Endpoints").ChildNodes);
            observables.Add(new LogicalViewModelNode(this, SourceViewModel.TopLevelNodes.First<IProductElementViewModel>(), null));
            observables[0].LogicalViewNodes.Add(endpointsItem);

            // Add Endpoints Node with just the Add->Nodes option
            observables[0].LogicalViewNodes[0].FilterMenuItems("Show Diagram", "Add");

            // Add Endpoint -> Add -> Component menu item
            foreach (var endpoint in endpointsItem.LogicalViewNodes)
            {
                endpoint.FilterMenuItems("Show Diagram", "Customize Authentication", "Add to Use Case", "Start Use Case");
                endpoint.MenuOptions.Add(endpoint.InnerViewModel.ChildNodes.Named("Components").MenuOptions.First(o => o.Caption == "Deploy Component..."));
            }

            LogicalViewNodes = observables;
        }

        public List<NServiceBusView> ViewsList { get; set; }

        public class NServiceBusView
        {
            public string Text { get; set; }
            public Action Action { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private NServiceBusView selectedView;
        public NServiceBusView SelectedView
        {
            get { return selectedView; }
            set
            {
                selectedView = value;
                selectedView.Action();
                RaiseOnPropertyChanged("SelectedView");
            }
        }

        public bool IsMasterView { get; private set; }

        private void SelectView(string title)
        {
            SelectedView = ViewsList.First(v => v.Text.Contains(title));
        }

        public void BuildViewsList()
        {
            ViewsList = new List<NServiceBusView> {
                new NServiceBusView { Text = "Endpoints View", Action = () => GenerateEndpointsView(false)},
                //new NServiceBusView { Text = "Use Cases View", Action = () => this.GenerateUseCasesView(false) },
                new NServiceBusView { Text = "Components View", Action = () => GenerateComponentsView(false) },
                new NServiceBusView { Text = "Messages View", Action = () => GenerateMessagesView(false) },
                new NServiceBusView { Text = "Libraries View", Action = () => GenerateLibrariesView(false) },
            };

            RaiseOnPropertyChanged("ViewsList");
            SelectView("Endpoints");
        }

        private void GenerateLibrariesView(bool setTitle = true)
        {
            if (setTitle)
            {
                SelectView("Libraries");
            }
            var observables = new ObservableCollection<LogicalViewModelNode>();

            var libraries = SourceViewModel.TopLevelNodes.First().ChildNodes.First(n => n.Data.DefinitionName == "Libraries");

            // Add Libraries Node with just the Add->Library option
            var librariesNode = new LogicalViewModelNode(this, libraries, libraries.ChildNodes);
            librariesNode.FilterMenuItems("Add");
            observables.Add(librariesNode);

            var services = SourceViewModel.TopLevelNodes.First().ChildNodes.First(n => n.Data.DefinitionName == "Services");

            // Add Services Node with just the Add->Service option
            var servicesNode = new LogicalViewModelNode(this, services, services.ChildNodes);
            servicesNode.FilterMenuItems("Add");
            observables.Add(servicesNode);

            foreach (var service in servicesNode.LogicalViewNodes)
            {
                // Adding menu options for "Add->Commands" and "Add->Events"
                //service.MenuOptions = new ObservableCollection<IMenuOptionViewModel>();
                service.FilterMenuItems("Delete");
                service.MenuOptions.Add(service.InnerViewModel.ChildNodes.Named("Libraries").MenuOptions.First(o => o.Caption == "Add"));

                foreach (var library in service.InnerViewModel.ChildNodes
                    .First(n => n.Data.DefinitionName == "ServiceLibraries").ChildNodes)
                {
                    service.LogicalViewNodes.Add(new LogicalViewModelNode(this, library, null));
                }
            }

            LogicalViewNodes = observables;
        }

        internal void GenerateComponentsView(bool setTitle = true)
        {
            if (setTitle)
            {
                SelectView("Components");
            }
            var observables = new ObservableCollection<LogicalViewModelNode>();

            var services = SourceViewModel.TopLevelNodes.First().ChildNodes.First(n => n.Data.DefinitionName == "Services");
            
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

            LogicalViewNodes = observables;
        }

        internal void GenerateMessagesView(bool setTitle = true)
        {
            if (setTitle)
            {
                SelectView("Messages");
            }
            var observables = new ObservableCollection<LogicalViewModelNode>();

            var services = SourceViewModel.TopLevelNodes.First().ChildNodes.First(n => n.Data.DefinitionName == "Services");

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

            LogicalViewNodes = observables;
        }

        private void CollapseAllButSelected()
        {
            FindLogicalNode(LogicalViewNodes, n =>
                {
                    if (FindLogicalNode(n.LogicalViewNodes, m => m.InnerViewModel.IsSelected) == null)
                    {
                        n.Collapse();
                    }
                    return false;
                });
        }

        private IProductElementViewModel GetProductNode(InnerPanelItem item)
        {
            var target = item.Product;
            var label = item.Text;
            var model = SearchInNodes(SourceViewModel.TopLevelNodes, target);
            if (model == null)
            {
                var element = SourceViewModel.TopLevelNodes[0].Data.As<IApplication>().Design.DummyCollection.As<IAbstractElement>();
                var ctx = SourceViewModel.TopLevelNodes[0].Context;
                var model2 = new LabelElementViewModel(element, ctx)
                {
                    Label = label
                };
                model = model2;
            }
            else if (item is InnerPanelTitle && (item as InnerPanelTitle).ForceText)
            {
                var ctx = SourceViewModel.TopLevelNodes[0].Context;
                if (model.Data is IAbstractElement)
                {
                    var model2 = new LabelElementViewModel(model.Data.As<IAbstractElement>(), ctx)
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentNode"));
            }
        }

        private IProductElementViewModel SearchInNodes(ObservableCollection<IProductElementViewModel> observableCollection, IProductElement target)
        {
            foreach (var model in observableCollection)
            {
                if (model.Data == target)
                {
                    return model;
                }
                var model2 = SearchInNodes(model.ChildNodes, target);
                if (model2 != null)
                {
                    return model2;
                }
            }
            return null;
        }

        private void WireSolutionBuilderViewModel(ISolutionBuilderViewModel sourceViewModel, bool wireCurrentNode)
        {
            SourceViewModel = sourceViewModel;
            GenerateCommands();
            if (wireCurrentNode)
            {
                SourceViewModel.CurrentNodeChanged += (s, e) =>
                {
                    if (LogicalViewNodes != null)
                    {
                        CurrentNode = FindLogicalNode(LogicalViewNodes, n => n.InnerViewModel == SourceViewModel.CurrentElement);
                        OnCurrentNodeChanged();
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
                        var currentInnerNode = CurrentNode.InnerViewModel;
                        selectedView.Action();
                        var nodeToSelect = FindLogicalNode(LogicalViewNodes, n => n.InnerViewModel == currentInnerNode);
                        if (nodeToSelect != null)
                        {
                            nodeToSelect.IsSelected = true;
                        }
                    }
                    catch { }
                }));
        }

        // Properties
        public ICommand ActivateCommand { get; private set; }
        public ICommand LeftCommand { get; private set; }
        public ICommand RightCommand { get; private set; }

        public LogicalViewModelNode CurrentNode
        {
            get
            {
                return currentNode;
            }
            private set
            {
                if (currentNode != value)
                {
                    if (((currentNode != null) && (currentNode.InnerViewModel != null)) && currentNode.InnerViewModel.IsEditing)
                    {
                        //this.currentNode.InnerViewModel.EndEdit();
                    }
                    currentNode = value;
                    OnCurrentNodeChanged();
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
                    logicalViewNodes = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LogicalViewNodes"));
                    }
                }
            }
        }


        public static LogicalViewModel NServiceBusViewModel { get; set; }

        public ISolutionBuilderViewModel SourceViewModel { get; set; }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaiseOnPropertyChanged("Title");
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
                    if (CustomIconPath == null)
                    {
                        return InnerViewModel.IconPath;
                    }
                    else
                        return CustomIconPath;
                }
            }

            private ObservableCollection<IMenuOptionViewModel> overridableMenuOptions;

            public ObservableCollection<IMenuOptionViewModel> MenuOptions
            {
                get
                {
                    return overridableMenuOptions ?? InnerViewModel.MenuOptions;
                }
                set
                {
                    overridableMenuOptions = value;
                    RaisePropertyChanged("MenuOptions");
                }
            }

            // Methods
            public LogicalViewModelNode(
                LogicalViewModel viewModel,
                IProductElementViewModel root,
                ObservableCollection<IProductElementViewModel> children
                )
            {
                IsExpanded = true;
                ViewModel = viewModel;
                WireInnerViewModel(root);

                if (children != null)
                {
                    Func<IProductElementViewModel, LogicalViewModelNode> LogicalViewCreator = n =>
                        {
                            return new LogicalViewModelNode(viewModel, n, null);
                        };
                    LogicalViewNodes = new ObservableCollection<LogicalViewModelNode>(children.Select(n => LogicalViewCreator(n)));
                }
                else
                {
                    LogicalViewNodes = new ObservableCollection<LogicalViewModelNode>();
                }

                if (!viewModel.IsMasterView)
                {
                    RemoveDeleteMenuItem();   
                }
            }

            private void WireInnerViewModel(IProductElementViewModel root)
            {
                InnerViewModel = root;
                InnerViewModel.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == "IsSelected")
                        {
                            // this.IsSelected = this.InnerViewModel.IsSelected;
                            if (!IsNServiceBusView)
                            {
                                RaisePropertyChanged("IsSelected");
                            }
                        }
                    };
            }

            public void FilterMenuItems(params string[] captions)
            {
                var sourceMenuOptions = InnerViewModel.MenuOptions;
                if (overridableMenuOptions != null)
                {
                    sourceMenuOptions = MenuOptions;
                }
                MenuOptions = new ObservableCollection<IMenuOptionViewModel>();
                foreach (var menuOption in sourceMenuOptions.Where(o => captions.Contains(o.Caption)))
                {
                    MenuOptions.Add(menuOption);
                }
            }

            internal void RemoveDeleteMenuItem()
            {
                MenuOptions = new ObservableCollection<IMenuOptionViewModel>(MenuOptions.Where(o => o.Caption != "Delete"));
            }

            // Properties
            public IProductElementViewModel InnerViewModel
            {
                get;
                private set;
            }

            public ObservableCollection<LogicalViewModelNode> LogicalViewNodes { get; set; }

            public bool IsExpanded { get; set; }

            public void Collapse()
            {
                IsExpanded = false;
                RaisePropertyChanged("IsExpanded");
            }

            public void RaisePropertyChanged(string name)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private bool isSelected;

            private bool IsNServiceBusView
            {
                get
                {
                    return NServiceBusViewModel == ViewModel;
                }
            }

            public bool IsSelected
            {
                get
                {
                    return IsNServiceBusView ? isSelected : InnerViewModel.IsSelected;
                }
                set
                {
                    if (IsNServiceBusView)
                    {
                        if (isSelected != value)
                        {
                            InnerViewModel.IsSelected = value;
                            isSelected = value;
                            RaisePropertyChanged("IsSelected");
                        }
                        if (isSelected)
                        {
                            ViewModel.CurrentNode = this;
                        }
                    }
                    else
                    {
                        InnerViewModel.IsSelected = value;
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

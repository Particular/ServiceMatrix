using AbstractEndpoint;
using Microsoft.VisualStudio.Shell;
using Mindscape.WpfDiagramming;
using NServiceBusStudio.Automation.Diagrams.ViewModels.Shapes;
using NuPattern.Runtime;
using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NuPattern;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels
{
    [Export]
    public class NServiceBusDiagramAdapter
    {
        public ISolutionBuilderViewModel SolutionBuilderViewModel { get; set; }

        public NServiceBusDiagramViewModel ViewModel { get; set; }

        public IPatternWindows PatternWindows { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        [ImportingConstructor]
        public NServiceBusDiagramAdapter([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider, 
                                         [Import] IPatternWindows patternWindows)
        {
            this.ViewModel = new NServiceBusDiagramViewModel();
            this.PatternWindows = patternWindows;
            this.ServiceProvider = serviceProvider;

            this.SolutionBuilderViewModel = this.PatternWindows.GetSolutionBuilderViewModel(serviceProvider);

            GenerateCurrentDiagram(this.SolutionBuilderViewModel.TopLevelNodes);
            HandleChanges(this.SolutionBuilderViewModel.TopLevelNodes);
        }

        private void GenerateCurrentDiagram(ObservableCollection<IProductElementViewModel> observableCollection)
        {
            var allNodes = observableCollection.Traverse(x => x.ChildNodes);

            // Add Endpoints
            AddElementOf(allNodes, new[] { "NServiceBusHost", "NServiceBusMVC", "NServiceBusWeb" });

            // Add Services
            AddElementOf(allNodes, new[] { "Service" });

            // Add Component
            AddElementOf(allNodes, new[] { "Component" });

            // Add Command
            AddElementOf(allNodes, new[] { "Command" });

            // Add Event
            AddElementOf(allNodes, new[] { "Event" });

            // Add ComponentLink
            AddElementOf(allNodes, new[] { "ComponentLink" });

            // Add Send/Receive Commands/Events
            AddElementOf(allNodes, new[] { "CommandLink", "EventLink", "ProcessedCommandLink", "SubscribedEventLink" });
        }

        private void HandleChanges(ObservableCollection<IProductElementViewModel> observableCollection)
        {
            observableCollection.CollectionChanged += TopLevelNodes_CollectionChanged;

            foreach (var item in observableCollection)
            {
                HandleChanges(item.ChildNodes);
            }
        }

        private void TopLevelNodes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IProductElementViewModel newElement in e.NewItems)
                    {
                        AddElement(newElement);
                        HandleChanges(new ObservableCollection<IProductElementViewModel>(new [] { newElement }));
                    }
                    break;
            }
        }

        private void AddElementOf(IEnumerable<IProductElementViewModel> viewModels, IEnumerable<string> types)
        {
            viewModels.Where(x => types.Contains(x.Data.Info.Name))
                      .ToList()
                      .ForEach(x => AddElement(x));
        }

        private void AddElement(IProductElementViewModel newElement)
        {
            var elementType = newElement.Data.Info.Name;

            switch (elementType)
            {
                case "NServiceBusHost":
                case "NServiceBusMVC":
                case "NServiceBusWeb":
                    this.ViewModel.GetOrCreateEndpointNode(newElement);
                    break;
                case "Service":
                    this.ViewModel.GetOrCreateServiceNode(Guid.Empty, newElement);
                    break;
                case "Component":
                    this.ViewModel.GetOrCreateComponentNode(newElement);
                    break;
                case "Command":
                    this.ViewModel.GetOrCreateCommandNode(newElement);
                    break;
                case "Event":
                    this.ViewModel.GetOrCreateEventNode(newElement);
                    break;
                case "ComponentLink":
                    var componentLink = newElement.Data.As<IAbstractComponentLink>();
                    CreateComponentLink(componentLink);
                    break;

                // Connections
                case "CommandLink": // Component -> Command
                    var commandLink = newElement.Data.As<ICommandLink>();
                    CreateCommandLink(commandLink);
                    break;
                case "EventLink": // Component -> Event
                    var eventLink = newElement.Data.As<IEventLink>();
                    CreateEventLink(eventLink);
                    break;
                case "ProcessedCommandLink": // Command -> Component
                    var processedCommandLink = newElement.Data.As<IProcessedCommandLink>();
                    CreateProcessedCommandLink(processedCommandLink);
                    break;
                case "SubscribedEventLink": // Event -> Component
                    var subscribedEventLink = newElement.Data.As<ISubscribedEventLink>();
                    CreateSubscribedEventLink(subscribedEventLink);
                    break;
                
            }

        }



        

        private void CreateComponentLink(IAbstractComponentLink componentLink)
        {
            var component = componentLink.ComponentReference.Value;
            var service = component.Parent.Parent;
            var serviceViewModel = FindViewModel(service.AsElement().Id);
            var componentViewModel = FindViewModel(component.AsElement().Id);

            this.ViewModel.GetOrCreateComponentLink(componentLink.ParentEndpointComponents.ParentEndpoint.As<NuPattern.Runtime.IProductElement>().Id,
                                                    serviceViewModel,
                                                    componentViewModel);

            component.Publishes.CommandLinks.ForEach(x => CreateCommandLink(x));
            component.Publishes.EventLinks.ForEach(x => CreateEventLink(x));
            component.Subscribes.ProcessedCommandLinks.ForEach(x => CreateProcessedCommandLink(x));
            component.Subscribes.SubscribedEventLinks.ForEach(x => CreateSubscribedEventLink(x));
        }


        private void CreateCommandLink(ICommandLink commandLink)
        {
            var commandNode = this.ViewModel.GetOrCreateCommandNode(this.FindViewModel(commandLink.CommandReference.Value.AsElement().Id));

            foreach (var component in this.ViewModel.GetAllComponentsNode(commandLink.Parent.Parent.AsElement().Id))
            {
                this.ViewModel.GetOrCreateCommandConnection(component, commandNode);
            }
        }

        private void CreateEventLink(IEventLink eventLink)
        {
            var eventNode = this.ViewModel.GetOrCreateEventNode(this.FindViewModel(eventLink.EventReference.Value.AsElement().Id));

            foreach (var component in this.ViewModel.GetAllComponentsNode(eventLink.Parent.Parent.AsElement().Id))
            {
                this.ViewModel.GetOrCreateEventConnection(component, eventNode);
            }
        }

        private void CreateProcessedCommandLink(IProcessedCommandLink processedCommandLink)
        {
            var commandNode = this.ViewModel.GetOrCreateCommandNode(this.FindViewModel(processedCommandLink.CommandReference.Value.AsElement().Id));

            foreach (var component in this.ViewModel.GetAllComponentsNode(processedCommandLink.Parent.Parent.AsElement().Id))
            {
                this.ViewModel.GetOrCreateCommandConnection(commandNode, component);
            }
        }


        private void CreateSubscribedEventLink(ISubscribedEventLink subscribedEventLink)
        {
            var eventNode = this.ViewModel.GetOrCreateEventNode(this.FindViewModel(subscribedEventLink.EventReference.Value.AsElement().Id));

            foreach (var component in this.ViewModel.GetAllComponentsNode(subscribedEventLink.Parent.Parent.AsElement().Id))
            {
                this.ViewModel.GetOrCreateEventConnection(eventNode, component);
            }
        }


        private IProductElementViewModel FindViewModel(Guid elementId)
        {
            var allNodes = this.SolutionBuilderViewModel.TopLevelNodes.Traverse(x => x.ChildNodes);
            return allNodes.FirstOrDefault(x => x.Data.Id == elementId);
        }

    }
}

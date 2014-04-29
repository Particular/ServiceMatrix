using AbstractEndpoint;
using Microsoft.VisualStudio.Shell;
using NServiceBusStudio;
using NuPattern;
using NuPattern.Runtime;
using NuPattern.Runtime.UI.ViewModels;
using NuPattern.VisualStudio.Solution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;

namespace ServiceMatrix.Diagramming.ViewModels
{
    using System.IO;

    [Export]
    public class ServiceMatrixDiagramAdapter
    {
        public ISolution Solution { get; set; }

        public IPatternWindows PatternWindows { get; set; }

        public ISolutionBuilderViewModel SolutionBuilderViewModel { get; set; }

        public ServiceMatrixDiagramMindscapeViewModel ViewModel { get; set; }

        public Action CloseWindow { get; set; }

        [ImportingConstructor]
        public ServiceMatrixDiagramAdapter([Import] ISolution solution,
                                           [Import] IPatternWindows patternWindows,
                                           [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            ViewModel = new ServiceMatrixDiagramMindscapeViewModel(patternWindows, serviceProvider);
            Solution = solution;
            PatternWindows = patternWindows;
            
            StartListening(serviceProvider);
        }

        private void StartListening(IServiceProvider serviceProvider)
        {
            var events = serviceProvider.TryGetService<ISolutionEvents>();

            events.SolutionOpened += (s, e) =>
            {
                WireSolution(serviceProvider);
            };

            events.SolutionClosed += (s, e) =>
            {
                ViewModel.CleanAll();
                UnhandleChanges(SolutionBuilderViewModel.TopLevelNodes);
                SolutionBuilderViewModel = null;
                CloseWindow();
            };

            if (Solution.IsOpen)
            {
                WireSolution(serviceProvider);
            }
        }

        public void WireSolution(IServiceProvider serviceProvider)
        {
            ViewModel.LayoutAlgorithm.LoadShapePositions(Path.GetDirectoryName(Solution.PhysicalPath));
            SolutionBuilderViewModel = PatternWindows.GetSolutionBuilderViewModel(serviceProvider);

            GenerateCurrentDiagram(SolutionBuilderViewModel.TopLevelNodes);
            HandleChanges(SolutionBuilderViewModel.TopLevelNodes);
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

            // Add Message
            AddElementOf(allNodes, new[] { "Message" });

            // Add ComponentLink
            AddElementOf(allNodes, new[] { "ComponentLink" });

            // Add Send/Receive Commands/Events
            AddElementOf(allNodes, new[] { "CommandLink", "EventLink", "ProcessedCommandLink", "SubscribedEventLink" });

            // Add Send/Receive ProcessedCommandLinkReply
            AddElementOf(allNodes, new[] { "ProcessedCommandLinkReply" });

            // Add Send/Receive HandleMessageLink
            AddElementOf(allNodes, new[] { "HandleMessageLink" });
            
        }

        private void HandleChanges(ObservableCollection<IProductElementViewModel> observableCollection)
        {
            observableCollection.CollectionChanged += ProductElementViewModel_CollectionChanged;

            foreach (var item in observableCollection)
            {
                HandleChanges(item.ChildNodes);
            }
        }

        private void UnhandleChanges(ObservableCollection<IProductElementViewModel> observableCollection)
        {
            observableCollection.CollectionChanged -= ProductElementViewModel_CollectionChanged;

            foreach (var item in observableCollection)
            {
                HandleChanges(item.ChildNodes);
            }
        }

        private void ProductElementViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
                case NotifyCollectionChangedAction.Remove:
                    foreach (IProductElementViewModel removedElement in e.OldItems)
                    {
                        RemoveElement(removedElement);
                        UnhandleChanges(new ObservableCollection<IProductElementViewModel>(new[] { removedElement }));
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
                case "Application":
                    newElement.ChildNodes.Traverse(x => x.ChildNodes).Where(x => x.Data.DefinitionName != "Service").ForEach (x => AddElement (x));
                    break;

                case "NServiceBusHost":
                case "NServiceBusMVC":
                case "NServiceBusWeb":
                    ViewModel.GetOrCreateEndpointNode(newElement);
                    break;
                case "Service":
                    ViewModel.GetOrCreateServiceNode(Guid.Empty, newElement);
                    break;
                case "Component":
                    ViewModel.GetOrCreateComponentNode(newElement);
                    break;
                case "Command":
                    ViewModel.GetOrCreateCommandNode(newElement);
                    break;
                case "Event":
                    ViewModel.GetOrCreateEventNode(newElement);
                    break;
                case "Message":
                    ViewModel.GetOrCreateMessageNode(newElement);
                    break;
                case "ComponentLink":
                    CreateComponentLink(newElement);
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

                case "ProcessedCommandLinkReply": // Component -> Mesage
                    var processedCommandLinkReply = newElement.Data.As<IProcessedCommandLinkReply>();
                    CreateProcessedCommandLinkReply(processedCommandLinkReply);
                    break;

                case "HandledMessageLink": // Message -> Component
                    var handledMessageLink = newElement.Data.As<IHandledMessageLink>();
                    CreateHandledMessageLink(handledMessageLink);
                    break;
            }
        }

        private void RemoveElement(IProductElementViewModel removedElement)
        {
            switch (removedElement.Data.Info.Name)
            {
                case "NServiceBusHost":
                case "NServiceBusMVC":
                case "NServiceBusWeb":
                    foreach (var cl in removedElement.ChildNodes.First().ChildNodes)
                    {
                        RemoveComponentLink(cl);
                    }
                    break;
                case "ComponentLink":
                    RemoveComponentLink(removedElement);
                    break;
                
            }

            ViewModel.DeleteNodesById(removedElement.Data.Id);
        }

        private void CreateComponentLink(IProductElementViewModel viewModel)
        {
            var componentLink = viewModel.Data.As<IAbstractComponentLink>();
            var component = componentLink.ComponentReference.Value;
            var service = component.Parent.Parent;
            var serviceViewModel = FindViewModel(service.AsElement().Id);
            var componentViewModel = FindViewModel(component.AsElement().Id);

            ViewModel.GetOrCreateComponentLink(componentLink.ParentEndpointComponents.ParentEndpoint.As<IProductElement>().Id,
                                                    serviceViewModel,
                                                    componentViewModel,
                                                    viewModel);

            CreateComponentLinks(component);
        }

        private void CreateComponentLinks(IComponent component)
        {
            component.Publishes.CommandLinks.ForEach(x => CreateCommandLink(x));
            component.Publishes.EventLinks.ForEach(x => CreateEventLink(x));
            component.Subscribes.ProcessedCommandLinks.ForEach(x => { if (x.ProcessedCommandLinkReply != null) CreateProcessedCommandLinkReply(x.ProcessedCommandLinkReply); });
            component.Subscribes.ProcessedCommandLinks.ForEach(x => CreateProcessedCommandLink(x));
            component.Subscribes.SubscribedEventLinks.ForEach(x => CreateSubscribedEventLink(x));
            component.Subscribes.HandledMessageLinks.ForEach(x => CreateHandledMessageLink(x));
        }


        private void CreateCommandLink(ICommandLink commandLink)
        {
            var commandNode = ViewModel.GetOrCreateCommandNode(FindViewModel(commandLink.CommandReference.Value.AsElement().Id));

            foreach (var component in ViewModel.GetAllComponentsNode(commandLink.Parent.Parent.AsElement().Id))
            {
                ViewModel.GetOrCreateCommandConnection(component, commandNode);
            }
        }

        private void CreateEventLink(IEventLink eventLink)
        {
            var eventNode = ViewModel.GetOrCreateEventNode(FindViewModel(eventLink.EventReference.Value.AsElement().Id));

            foreach (var component in ViewModel.GetAllComponentsNode(eventLink.Parent.Parent.AsElement().Id))
            {
                ViewModel.GetOrCreateEventConnection(component, eventNode);
            }
        }

        private void CreateProcessedCommandLink(IProcessedCommandLink processedCommandLink)
        {
            var commandNode = ViewModel.GetOrCreateCommandNode(FindViewModel(processedCommandLink.CommandReference.Value.AsElement().Id));

            foreach (var component in ViewModel.GetAllComponentsNode(processedCommandLink.Parent.Parent.AsElement().Id))
            {
                ViewModel.GetOrCreateCommandConnection(commandNode, component);
            }
        }


        private void CreateSubscribedEventLink(ISubscribedEventLink subscribedEventLink)
        {
            var eventNode = ViewModel.GetOrCreateEventNode(FindViewModel(subscribedEventLink.EventReference.Value.AsElement().Id));

            foreach (var component in ViewModel.GetAllComponentsNode(subscribedEventLink.Parent.Parent.AsElement().Id))
            {
                ViewModel.GetOrCreateEventConnection(eventNode, component);
            }
        }

        private void CreateProcessedCommandLinkReply(IProcessedCommandLinkReply processedCommandLinkReply)
        {
            var messageNode = ViewModel.GetOrCreateMessageNode(FindViewModel(processedCommandLinkReply.MessageReference.Value.AsElement().Id));
            
            foreach (var component in ViewModel.GetAllComponentsNode(processedCommandLinkReply.Parent.Parent.Parent.AsElement().Id))
            {
                ViewModel.GetOrCreateMessageConnection(component, messageNode);
            }
        }

        private void CreateHandledMessageLink(IHandledMessageLink handledMessageLink)
        {
            var messageNode = ViewModel.GetOrCreateMessageNode(FindViewModel(handledMessageLink.MessageReference.Value.AsElement().Id));
            
            foreach (var component in ViewModel.GetAllComponentsNode(handledMessageLink.Parent.Parent.AsElement().Id))
            {
                ViewModel.GetOrCreateMessageConnection(messageNode, component);
            }
        }

        private void RemoveComponentLink(IProductElementViewModel removedElement)
        {
            var componentViewModel = ViewModel.DeleteComponentLinkNode(removedElement);
            if (componentViewModel != null && componentViewModel.Data.Product != null)
            {
                ViewModel.GetOrCreateComponentNode(componentViewModel);

                var component = componentViewModel.Data.As<IComponent>();
                CreateComponentLinks(component);
            }
        }


        private IProductElementViewModel FindViewModel(Guid elementId)
        {
            var allNodes = SolutionBuilderViewModel.TopLevelNodes.Traverse(x => x.ChildNodes);
            return allNodes.FirstOrDefault(x => x.Data.Id == elementId);
        }

        // CUSTOM OPERATIONS ON DIAGRAM

        public void AddEndpoint(string endpointName, string hostType)
        {
            var app = SolutionBuilderViewModel.TopLevelNodes.First().Data.As<IApplication>();

            switch (hostType)
            {
                case "NServiceBusHost":
                    app.Design.Endpoints.CreateNServiceBusHost(endpointName);
                    break;
                case "NServiceBusMVC":
                    app.Design.Endpoints.CreateNServiceBusMVC(endpointName);
                    break;
                case "NServiceBusWeb":
                    app.Design.Endpoints.CreateNServiceBusWeb(endpointName);
                    break;
                default:
                    break;
            }
        }

        public void AddService(string serviceName)
        {
            var app = SolutionBuilderViewModel.TopLevelNodes.First().Data.As<IApplication>();

            app.Design.Services.CreateService(serviceName);
        }

    }
}

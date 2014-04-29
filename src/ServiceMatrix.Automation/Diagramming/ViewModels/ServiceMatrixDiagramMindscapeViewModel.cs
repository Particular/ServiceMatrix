namespace ServiceMatrix.Diagramming.ViewModels
{
    using Mindscape.WpfDiagramming;
    using ServiceMatrix.Diagramming.ViewModels.BaseViewModels;
    using ServiceMatrix.Diagramming.ViewModels.Shapes;
    using NuPattern.Runtime.UI.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ServiceMatrix.Diagramming.ViewModels.Connections;
    using NServiceBusStudio;
    using NuPattern.Runtime;
    using System.Windows.Controls;

    public class ServiceMatrixDiagramMindscapeViewModel: Diagram
    {
        public ServiceMatrixDiagramLayoutAlgorithm LayoutAlgorithm { get; set; }

        public ServiceMatrixDiagramMindscapeViewModel(IPatternWindows patternWindows, IServiceProvider serviceProvider)
        {
            DefaultConnectionBuilder = new ServiceMatrixConnectionBuilder();
            LayoutAlgorithm = new ServiceMatrixDiagramLayoutAlgorithm(this);
            NodeRemover = new ServiceMatrixDiagramRemover();

            PatternWindows = patternWindows;
            ServiceProvider = serviceProvider;
        }

        public EndpointNode GetOrCreateEndpointNode(IProductElementViewModel viewModel)
        {
            var endpoint = FindNode<EndpointNode>(viewModel.Data.Id);

            if (endpoint == null)
            {
                endpoint = new EndpointNode(viewModel);
                LayoutAlgorithm.SetElementPosition(endpoint);

                AddNode(endpoint);
            }

            return endpoint;
        }

        

        private EmptyEndpointNode GetOrCreateEmptyEndpointNode(IProductElementViewModel endpointsViewModel)
        {
            var emptyEndpoint = FindNode<EmptyEndpointNode>(EmptyEndpointNode.NodeId);

            if (emptyEndpoint == null)
            {
                emptyEndpoint = new EmptyEndpointNode(endpointsViewModel.MenuOptions.FirstOrDefault( x=> x.Caption == "Deploy Unhosted Components..."));
                LayoutAlgorithm.SetElementPosition(emptyEndpoint);

                AddNode(emptyEndpoint);
            }

            return emptyEndpoint;
        }

        public ServiceNode GetOrCreateServiceNode(Guid endpointId, IProductElementViewModel viewModel)
        {
            var service = FindNode<ServiceNode>(x => x.Id == viewModel.Data.Id &&
                                                            x.ParentNode.Id == endpointId);

            if (service == null)
            {
                var endpoint = FindNode<EndpointNode>(endpointId);

                if (endpoint == null &&
                    endpointId == EmptyEndpointNode.NodeId)
                {
                    var endpointsViewModel = viewModel.ParentNode.ParentNode.ChildNodes.First(x => x.Data.InstanceName == "Endpoints");
                    endpoint = GetOrCreateEmptyEndpointNode(endpointsViewModel);
                }

                service = new ServiceNode(viewModel, endpoint);
                AddNode(service);
            }

            return service;
        }


        public CommandNode GetOrCreateCommandNode(IProductElementViewModel viewModel)
        {
            var command = FindNode<CommandNode>(viewModel.Data.Id);

            if (command == null)
            {
                command = new CommandNode(viewModel);
                LayoutAlgorithm.SetElementPosition(command);

                AddNode(command);
            }

            return command;
        }

        public EventNode GetOrCreateEventNode(IProductElementViewModel viewModel)
        {
            var @event = FindNode<EventNode>(viewModel.Data.Id);

            if (@event == null)
            {
                @event = new EventNode(viewModel);
                LayoutAlgorithm.SetElementPosition(@event);

                AddNode(@event);
            }

            return @event;
        }

        public MessageNode GetOrCreateMessageNode(IProductElementViewModel viewModel)
        {
            var message = FindNode<MessageNode>(viewModel.Data.Id);

            if (message == null)
            {
                message = new MessageNode(viewModel);
                LayoutAlgorithm.SetElementPosition(message);

                AddNode(message);
            }

            return message;
        }

        public ComponentNode GetOrCreateComponentNode(IProductElementViewModel viewModel)
        {
            var component = FindNode<ComponentNode>(viewModel.Data.Id);

            if (component == null)
            {
                var service = GetOrCreateServiceNode(EmptyEndpointNode.NodeId, viewModel.ParentNode.ParentNode);

                component = new ComponentNode(viewModel, service, null);
                AddNode(component);
            }

            return component;
        }


        public ComponentNode GetOrCreateComponentLink(Guid endpointId, IProductElementViewModel serviceViewModel, IProductElementViewModel componentViewModel, IProductElementViewModel viewModel)
        {
            // Find undeployed component
            var undeployedComponentNode = FindComponent(EmptyEndpointNode.NodeId,
                                                        serviceViewModel.Data.Id, 
                                                        componentViewModel.Data.Id);
            
            if (undeployedComponentNode != null)
            {
                DeleteNode(undeployedComponentNode);
            }

            // Create New Component Link
            var componentNode = FindComponent(endpointId,
                                              serviceViewModel.Data.Id,
                                              componentViewModel.Data.Id);

            if (componentNode == null)
            {
                var serviceNode = GetOrCreateServiceNode(endpointId,
                                                         serviceViewModel);

                componentNode = new ComponentNode(componentViewModel,
                                                  serviceNode,
                                                  viewModel);
                AddNode(componentNode);
            }

            return componentNode;
        }

        public IProductElementViewModel DeleteComponentLinkNode(IProductElementViewModel removedElement)
        {
            var component = Nodes.FirstOrDefault(x => x is ComponentNode && ((ComponentNode)x).ComponentLinkViewModel == removedElement) as ComponentNode;

            if (component != null)
            {
                DeleteNode(component);

                // Remove service if it's empty
                var serviceNode = component.ParentNode;
                if (serviceNode != null &&
                    !serviceNode.ChildNodes.Any())
                {
                    DeleteNode(serviceNode);
                }

                return component.InnerViewModel;
            }

            return null;
        }

        // ================ CONNECTIONS ==========================

        public CommandConnection GetOrCreateCommandConnection(GroupableNode sourceNode, GroupableNode targetNode)
        {
            var commandConnection = FindConnection(sourceNode, targetNode) as CommandConnection;

            if (commandConnection == null)
            {
                commandConnection = new CommandConnection(sourceNode, targetNode);
                Connections.Add(commandConnection);
            }

            return commandConnection;
        }

        public EventConnection GetOrCreateEventConnection(GroupableNode sourceNode, GroupableNode targetNode)
        {
            var eventConnection = FindConnection(sourceNode, targetNode) as EventConnection;

            if (eventConnection == null)
            {
                eventConnection = new EventConnection(sourceNode, targetNode);
                Connections.Add(eventConnection);
            }

            return eventConnection;
        }

        public MessageConnection GetOrCreateMessageConnection(GroupableNode sourceNode, GroupableNode targetNode)
        {
            var messageConnection = FindConnection(sourceNode, targetNode) as MessageConnection;

            if (messageConnection == null)
            {
                messageConnection = new MessageConnection(sourceNode, targetNode);
                Connections.Add(messageConnection);
            }

            return messageConnection;
        }

        
        private DiagramConnection FindConnection(GroupableNode source, GroupableNode target)
        {
            return Connections.FirstOrDefault(x => source.ConnectionPoints.Any(y => y == x.FromConnectionPoint) &&
                                                        target.ConnectionPoints.Any(y => y == x.ToConnectionPoint));
        }


        // ================ HELPERS ==========================

        public IEnumerable<ComponentNode> GetAllComponentsNode(Guid componentId)
        {
            return Nodes.Where(x => x is ComponentNode && ((ComponentNode)x).Id == componentId).Cast<ComponentNode>().ToList();
        }

        private T FindNode<T>(Guid elementId) where T : GroupableNode
        {
            return Nodes.FirstOrDefault(x => x is T && ((T)x).Id == elementId) as T;
        }

        private T FindNode<T>(Func<T, bool> filter) where T : GroupableNode
        {
            return Nodes.FirstOrDefault(x => x is T && filter((T)x)) as T;
        }

        private ComponentNode FindComponent(Guid endpointId, Guid serviceId, Guid componentId)
        {
            return Nodes.FirstOrDefault(x => x is ComponentNode &&
                                                ((ComponentNode)x).Id == componentId &&
                                                ((ComponentNode)x).ParentNode.Id == serviceId &&
                                                ((ComponentNode)x).ParentNode.ParentNode.Id == endpointId) as ComponentNode;
        }

        public void DeleteNodesById(Guid id)
        {
            var nodes = Nodes.Where(x => x is GroupableNode && ((GroupableNode)x).Id == id).Cast<GroupableNode>().ToList();
            nodes.ForEach(x => DeleteNode(x));
        }

        public void DeleteNode(GroupableNode node)
        {
            // Remove Node from Parent
            if (node.ParentNode != null)
            {
                node.ParentNode.RemoveChild(node);
            }

            // Remove child nodes
            var groupNode = node as GroupNode;
            if (groupNode != null)
            {
                groupNode.ChildNodes.ToList().ForEach(x => DeleteNode(x));
            }

            // If It's a Component, Remove Empty Service
            if (node is ComponentNode &&
                node.ParentNode is ServiceNode &&
                node.ParentNode.ChildNodes.Count == 0)
            {
                DeleteNode(node.ParentNode);
            }

            // If It's a Service, Remove Empty Undeployed Endpoint
            if (node is ServiceNode &&
                node.ParentNode is EmptyEndpointNode &&
                node.ParentNode.ChildNodes.Count == 0)
            {
                DeleteNode(node.ParentNode);
            }

            // Remove Layout information
            LayoutAlgorithm.RemoveElementPosition(node);

            // Remove node
            Nodes.Remove(node);

            // Unhandle events
            node.ActivateElement -= Node_ActivateElement;
        }

        public void CleanAll()
        {
            Nodes.ToList().ForEach(x => Nodes.Remove(x));
            Connections.ToList().ForEach(x => Connections.Remove(x));
            LayoutAlgorithm.UnloadShapePositiions();
        }

        private void AddNode(GroupableNode node)
        {
            Nodes.Add(node);
            node.ActivateElement += Node_ActivateElement;
        }

        void Node_ActivateElement(object sender, EventArgs e)
        {
            var toolWindow = PatternWindows.ShowSolutionBuilder(ServiceProvider);

            var content = toolWindow.Content as UserControl;
            var contentGrid = content.Content as Grid;
            var scrollviewer = default(ScrollViewer);

            foreach (var theitem in contentGrid.Children)
            {
                if (theitem is ScrollViewer)
                {
                    scrollviewer = (theitem as ScrollViewer);
                }
            }

            if (scrollviewer != null)
            {
                scrollviewer.Focus();
            }
        }


        // ================ NODES HIGHLIGHTING ==========================

        public void HighlightNode(GroupableNode node)
        {
            if (node is ServiceNode)
            {
                var serviceRelatedNodes = GetServiceRelatedNodes(node);
                serviceRelatedNodes.ForEach(x => x.IsHighlighted = true);
            }
            else if (node is ComponentNode || node is MessageBaseNode)
            {
                var relatedNodes = GetComponentsMessagesRelatedNodes(node);
                relatedNodes.ForEach(x => x.IsHighlighted = true);
                HighlightConnection(relatedNodes);
            }

            node.IsHighlighted = true;
        }

        public void UnhighlightNode(GroupableNode node)
        {
            if (node is ServiceNode)
            {
                var serviceRelatedNodes = GetServiceRelatedNodes(node);
                serviceRelatedNodes.ForEach(x => x.IsHighlighted = false);
            }
            else if (node is ComponentNode || node is MessageBaseNode)
            {
                var relatedNodes = GetComponentsMessagesRelatedNodes(node);
                relatedNodes.ForEach(x => x.IsHighlighted = false);
                UnhighlightConnection(relatedNodes);
            }

            node.IsHighlighted = false;
        }

        private List<GroupableNode> GetComponentsMessagesRelatedNodes(GroupableNode node)
        {
            var relatedNodes = new List<GroupableNode>
            {
                node
            };

            var nodeConnections = Connections.Cast<BaseConnection>()
                                                  .Where(x => x.Source == node ||
                                                              x.Target == node)
                                                  .ToList();

            foreach (var nodeConnection in nodeConnections)
            {
                GroupableNode nodeElement;
                if (nodeConnection.Source == node)
                    nodeElement = Nodes.Cast<GroupableNode>().FirstOrDefault(x => x == nodeConnection.Target);
                else
                    nodeElement = Nodes.Cast<GroupableNode>().FirstOrDefault(x => x == nodeConnection.Source);

                relatedNodes.Add(nodeElement);
            }

            return relatedNodes;
        }

        private List<GroupableNode> GetServiceRelatedNodes(GroupableNode node)
        {
            var service = node.InnerViewModel.Data.As<IService>();

            var nodesId = new List<Guid>
            {
                service.AsElement().Id
            };
            nodesId.AddRange(service.Components.Component.Select(x => x.AsElement().Id)); // Related Components Id
            nodesId.AddRange(service.Contract.Commands.Command.Select(x => x.AsElement().Id)); // Related Commands Id
            nodesId.AddRange(service.Contract.Events.Event.Select(x => x.AsElement().Id)); // Related Events Id
            nodesId.AddRange(service.Contract.Messages.Message.Select(x => x.AsElement().Id)); // Related Messages Id

            var allNodes = Nodes.Cast<GroupableNode>()
                                     .Where(x => nodesId.Contains(x.Id))
                                     .ToList();
            return allNodes;
        }


        // ================ CONNECTIONS HIGHLIGHTING ==========================

        public void HighlightConnection(BaseConnection context)
        {
            context.IsHighlighted = true;
            SetConnectionsIsShadowed(context, true);
        }

        public void UnhighlightConnection(BaseConnection context)
        {
            context.IsHighlighted = false;
            SetConnectionsIsShadowed(context, false);
        }
        
        public void HighlightConnection(List<GroupableNode> context)
        {
            SetConnectionsIsHighlighted(context, true);
            SetConnectionsIsShadowed(context, true);
        }

        public void UnhighlightConnection(List<GroupableNode> context)
        {
            SetConnectionsIsHighlighted(context, false);
            SetConnectionsIsShadowed(context, false);
        }

        public void SetConnectionsIsShadowed(BaseConnection context, bool value)
        {
            var otherNodeConnections = Connections.Cast<BaseConnection>()
                                                       .Where(x => x != context)
                                                       .ToList();

            otherNodeConnections.ForEach(x => x.IsShadowed = value);
        }

        public void SetConnectionsIsShadowed(List<GroupableNode> context, bool value)
        {
            var otherNodeConnections = Connections.Cast<BaseConnection>()
                                                       .Where(x => !(context.Contains (x.Source) && context.Contains(x.Target)))
                                                       .ToList();

            otherNodeConnections.ForEach(x => x.IsShadowed = value);
        }

        private void SetConnectionsIsHighlighted(List<GroupableNode> context, bool value)
        {
            var otherNodeConnections = Connections.Cast<BaseConnection>()
                                                       .Where(x => context.Contains(x.Source) && context.Contains(x.Target))
                                                       .ToList();

            otherNodeConnections.ForEach(x => x.IsHighlighted = value);
        }

        public IPatternManager PatternManager { get; set; }

        public IPatternWindows PatternWindows { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

    }
}

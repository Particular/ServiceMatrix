using NuPattern;
using AbstractEndpoint;
using Mindscape.WpfDiagramming;
using ServiceMatrix.Diagramming.ViewModels.BaseViewModels;
using ServiceMatrix.Diagramming.ViewModels.Shapes;
using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ServiceMatrix.Diagramming.ViewModels.Connections;
using NServiceBusStudio;
using NuPattern.Runtime;
using System.Windows.Controls;

namespace ServiceMatrix.Diagramming.ViewModels
{
    public class ServiceMatrixDiagramMindscapeViewModel: Diagram
    {
        public ServiceMatrixDiagramLayoutAlgorithm LayoutAlgorithm { get; set; }

        public ServiceMatrixDiagramMindscapeViewModel(IPatternWindows patternWindows, IServiceProvider serviceProvider)
        {
            this.DefaultConnectionBuilder = new ServiceMatrixConnectionBuilder();
            this.LayoutAlgorithm = new ServiceMatrixDiagramLayoutAlgorithm(this);
            this.NodeRemover = new ServiceMatrixDiagramRemover();

            this.PatternWindows = patternWindows;
            this.ServiceProvider = serviceProvider;
        }

        public EndpointNode GetOrCreateEndpointNode(IProductElementViewModel viewModel)
        {
            var endpoint = this.FindNode<EndpointNode>(viewModel.Data.Id);

            if (endpoint == null)
            {
                endpoint = new EndpointNode(viewModel);
                this.LayoutAlgorithm.SetElementPosition(endpoint);

                AddNode(endpoint);
            }

            return endpoint;
        }

        

        private EmptyEndpointNode GetOrCreateEmptyEndpointNode(IProductElementViewModel endpointsViewModel)
        {
            var emptyEndpoint = this.FindNode<EmptyEndpointNode>(EmptyEndpointNode.NodeId);

            if (emptyEndpoint == null)
            {
                emptyEndpoint = new EmptyEndpointNode(endpointsViewModel.MenuOptions.FirstOrDefault( x=> x.Caption == "Deploy Unhosted Components..."));
                this.LayoutAlgorithm.SetElementPosition(emptyEndpoint);

                AddNode(emptyEndpoint);
            }

            return emptyEndpoint;
        }

        public ServiceNode GetOrCreateServiceNode(Guid endpointId, IProductElementViewModel viewModel)
        {
            var service = this.FindNode<ServiceNode>((x) => x.Id == viewModel.Data.Id &&
                                                            x.ParentNode.Id == endpointId);

            if (service == null)
            {
                var endpoint = this.FindNode<EndpointNode>(endpointId);

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
            var command = this.FindNode<CommandNode>(viewModel.Data.Id);

            if (command == null)
            {
                command = new CommandNode(viewModel);
                this.LayoutAlgorithm.SetElementPosition(command);

                AddNode(command);
            }

            return command;
        }

        public EventNode GetOrCreateEventNode(IProductElementViewModel viewModel)
        {
            var @event = this.FindNode<EventNode>(viewModel.Data.Id);

            if (@event == null)
            {
                @event = new EventNode(viewModel);
                this.LayoutAlgorithm.SetElementPosition(@event);

                AddNode(@event);
            }

            return @event;
        }

        public MessageNode GetOrCreateMessageNode(IProductElementViewModel viewModel)
        {
            var message = this.FindNode<MessageNode>(viewModel.Data.Id);

            if (message == null)
            {
                message = new MessageNode(viewModel);
                this.LayoutAlgorithm.SetElementPosition(message);

                AddNode(message);
            }

            return message;
        }

        public ComponentNode GetOrCreateComponentNode(IProductElementViewModel viewModel)
        {
            var component = this.FindNode<ComponentNode>(viewModel.Data.Id);

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
                this.DeleteNode(undeployedComponentNode);
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
            var component = this.Nodes.FirstOrDefault(x => x is ComponentNode && ((ComponentNode)x).ComponentLinkViewModel == removedElement) as ComponentNode;

            if (component != null)
            {
                this.DeleteNode(component);

                // Remove service if it's empty
                var serviceNode = component.ParentNode;
                if (serviceNode != null &&
                    !serviceNode.ChildNodes.Any())
                {
                    this.DeleteNode(serviceNode);
                }

                return component.InnerViewModel;
            }

            return null;
        }

        // ================ CONNECTIONS ==========================

        public CommandConnection GetOrCreateCommandConnection(GroupableNode sourceNode, GroupableNode targetNode)
        {
            var commandConnection = this.FindConnection(sourceNode, targetNode) as CommandConnection;

            if (commandConnection == null)
            {
                commandConnection = new CommandConnection(sourceNode, targetNode);
                this.Connections.Add(commandConnection);
            }

            return commandConnection;
        }

        public EventConnection GetOrCreateEventConnection(GroupableNode sourceNode, GroupableNode targetNode)
        {
            var eventConnection = this.FindConnection(sourceNode, targetNode) as EventConnection;

            if (eventConnection == null)
            {
                eventConnection = new EventConnection(sourceNode, targetNode);
                this.Connections.Add(eventConnection);
            }

            return eventConnection;
        }

        public MessageConnection GetOrCreateMessageConnection(GroupableNode sourceNode, GroupableNode targetNode)
        {
            var messageConnection = this.FindConnection(sourceNode, targetNode) as MessageConnection;

            if (messageConnection == null)
            {
                messageConnection = new MessageConnection(sourceNode, targetNode);
                this.Connections.Add(messageConnection);
            }

            return messageConnection;
        }

        
        private DiagramConnection FindConnection(GroupableNode source, GroupableNode target)
        {
            return this.Connections.FirstOrDefault(x => source.ConnectionPoints.Any(y => y == x.FromConnectionPoint) &&
                                                        target.ConnectionPoints.Any(y => y == x.ToConnectionPoint));
        }


        // ================ HELPERS ==========================

        public IEnumerable<ComponentNode> GetAllComponentsNode(Guid componentId)
        {
            return this.Nodes.Where(x => x is ComponentNode && ((ComponentNode)x).Id == componentId).Cast<ComponentNode>().ToList();
        }

        private T FindNode<T>(Guid elementId) where T : GroupableNode
        {
            return this.Nodes.FirstOrDefault(x => x is T && ((T)x).Id == elementId) as T;
        }

        private T FindNode<T>(Func<T, bool> filter) where T : GroupableNode
        {
            return this.Nodes.FirstOrDefault(x => x is T && filter((T)x)) as T;
        }

        private ComponentNode FindComponent(Guid endpointId, Guid serviceId, Guid componentId)
        {
            return this.Nodes.FirstOrDefault(x => x is ComponentNode &&
                                                ((ComponentNode)x).Id == componentId &&
                                                ((ComponentNode)x).ParentNode.Id == serviceId &&
                                                ((ComponentNode)x).ParentNode.ParentNode.Id == endpointId) as ComponentNode;
        }

        public void DeleteNodesById(Guid id)
        {
            var nodes = this.Nodes.Where(x => x is GroupableNode && ((GroupableNode)x).Id == id).Cast<GroupableNode>().ToList();
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
                this.DeleteNode(node.ParentNode);
            }

            // If It's a Service, Remove Empty Undeployed Endpoint
            if (node is ServiceNode &&
                node.ParentNode is EmptyEndpointNode &&
                node.ParentNode.ChildNodes.Count == 0)
            {
                this.DeleteNode(node.ParentNode);
            }

            // Remove Layout information
            this.LayoutAlgorithm.RemoveElementPosition(node);

            // Remove node
            this.Nodes.Remove(node);

            // Unhandle events
            node.ActivateElement -= Node_ActivateElement;

            node = null;
        }

        public void CleanAll()
        {
            this.Nodes.ToList().ForEach(x => this.Nodes.Remove(x));
            this.Connections.ToList().ForEach(x => this.Connections.Remove(x));
            this.LayoutAlgorithm.UnloadShapePositiions();
        }

        private void AddNode(GroupableNode node)
        {
            this.Nodes.Add(node);
            node.ActivateElement += Node_ActivateElement;
        }

        void Node_ActivateElement(object sender, EventArgs e)
        {
            var toolWindow = this.PatternWindows.ShowSolutionBuilder(this.ServiceProvider);

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
            var relatedNodes = new List<GroupableNode>();
            relatedNodes.Add(node);

            var nodeConnections = this.Connections.Cast<BaseConnection>()
                                                  .Where(x => x.Source == node ||
                                                              x.Target == node)
                                                  .ToList();

            foreach (var nodeConnection in nodeConnections)
            {
                var nodeElement = default(GroupableNode);
                if (nodeConnection.Source == node)
                    nodeElement = this.Nodes.Cast<GroupableNode>().FirstOrDefault(x => x == nodeConnection.Target);
                else
                    nodeElement = this.Nodes.Cast<GroupableNode>().FirstOrDefault(x => x == nodeConnection.Source);

                relatedNodes.Add(nodeElement);
            }

            return relatedNodes;
        }

        private List<GroupableNode> GetServiceRelatedNodes(GroupableNode node)
        {
            var service = node.InnerViewModel.Data.As<IService>();

            var nodesId = new List<Guid>();
            nodesId.Add(service.AsElement().Id); // Service Id
            nodesId.AddRange(service.Components.Component.Select(x => x.AsElement().Id)); // Related Components Id
            nodesId.AddRange(service.Contract.Commands.Command.Select(x => x.AsElement().Id)); // Related Commands Id
            nodesId.AddRange(service.Contract.Events.Event.Select(x => x.AsElement().Id)); // Related Events Id
            nodesId.AddRange(service.Contract.Messages.Message.Select(x => x.AsElement().Id)); // Related Messages Id

            var allNodes = this.Nodes.Cast<GroupableNode>()
                                     .Where(x => nodesId.Contains(x.Id))
                                     .ToList();
            return allNodes;
        }


        // ================ CONNECTIONS HIGHLIGHTING ==========================

        public void HighlightConnection(BaseConnection context)
        {
            context.IsHighlighted = true;
            this.SetConnectionsIsShadowed(context, true);
        }

        public void UnhighlightConnection(BaseConnection context)
        {
            context.IsHighlighted = false;
            this.SetConnectionsIsShadowed(context, false);
        }
        
        public void HighlightConnection(List<GroupableNode> context)
        {
            this.SetConnectionsIsHighlighted(context, true);
            this.SetConnectionsIsShadowed(context, true);
        }

        public void UnhighlightConnection(List<GroupableNode> context)
        {
            this.SetConnectionsIsHighlighted(context, false);
            this.SetConnectionsIsShadowed(context, false);
        }

        public void SetConnectionsIsShadowed(BaseConnection context, bool value)
        {
            var otherNodeConnections = this.Connections.Cast<BaseConnection>()
                                                       .Where(x => x != context)
                                                       .ToList();

            otherNodeConnections.ForEach(x => x.IsShadowed = value);
        }

        public void SetConnectionsIsShadowed(List<GroupableNode> context, bool value)
        {
            var otherNodeConnections = this.Connections.Cast<BaseConnection>()
                                                       .Where(x => !(context.Contains (x.Source) && context.Contains(x.Target)))
                                                       .ToList();

            otherNodeConnections.ForEach(x => x.IsShadowed = value);
        }

        private void SetConnectionsIsHighlighted(List<GroupableNode> context, bool value)
        {
            var otherNodeConnections = this.Connections.Cast<BaseConnection>()
                                                       .Where(x => context.Contains(x.Source) && context.Contains(x.Target))
                                                       .ToList();

            otherNodeConnections.ForEach(x => x.IsHighlighted = value);
        }

        public IPatternManager PatternManager { get; set; }

        public IPatternWindows PatternWindows { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

    }
}

using NuPattern;
using AbstractEndpoint;
using Mindscape.WpfDiagramming;
using NServiceBusStudio.Automation.Diagrams.ViewModels.BaseViewModels;
using NServiceBusStudio.Automation.Diagrams.ViewModels.Shapes;
using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NServiceBusStudio.Automation.Diagrams.ViewModels.Connections;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels
{
    public class NServiceBusDiagramViewModel: Diagram
    {
        public NServiceBusDiagramLayoutAlgorithm LayoutAlgorithm { get; set; }

        public NServiceBusDiagramViewModel()
        {
            this.DefaultConnectionBuilder = new NServiceBusConnectionBuilder();
            this.LayoutAlgorithm = new NServiceBusDiagramLayoutAlgorithm(this);
        }

        public EndpointNode GetOrCreateEndpointNode(IProductElementViewModel viewModel)
        {
            var endpoint = this.FindNode<EndpointNode>(viewModel.Data.Id);

            if (endpoint == null)
            {
                endpoint = new EndpointNode(viewModel);
                this.LayoutAlgorithm.SetElementPosition(endpoint);

                this.Nodes.Add(endpoint);
            }

            return endpoint;
        }

        private EmptyEndpointNode GetOrCreateEmptyEndpointNode()
        {
            var emptyEndpoint = this.FindNode<EmptyEndpointNode>(EmptyEndpointNode.NodeId);

            if (emptyEndpoint == null)
            {
                emptyEndpoint = new EmptyEndpointNode();
                this.LayoutAlgorithm.SetElementPosition(emptyEndpoint);

                this.Nodes.Add(emptyEndpoint);
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
                    endpoint = GetOrCreateEmptyEndpointNode();
                }

                service = new ServiceNode(viewModel, endpoint);
                this.Nodes.Add(service);
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

                this.Nodes.Add(command);
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

                this.Nodes.Add(@event);
            }

            return @event;
        }

        public ComponentNode GetOrCreateComponentNode(IProductElementViewModel viewModel)
        {
            var component = this.FindNode<ComponentNode>(viewModel.Data.Id);

            if (component == null)
            {
                var service = GetOrCreateServiceNode(EmptyEndpointNode.NodeId, viewModel.ParentNode.ParentNode);

                component = new ComponentNode(viewModel, service);
                this.Nodes.Add(component);
            }

            return component;
        }


        public ComponentNode GetOrCreateComponentLink(Guid endpointId, IProductElementViewModel serviceViewModel, IProductElementViewModel viewModel)
        {
            // Find undeployed component
            var undeployedComponentNode = FindComponent(EmptyEndpointNode.NodeId,
                                                        serviceViewModel.Data.Id, 
                                                        viewModel.Data.Id);
            this.Nodes.Remove(undeployedComponentNode);


            // Create New Component Link
            var componentNode = FindComponent(endpointId,
                                              serviceViewModel.Data.Id,
                                              viewModel.Data.Id);

            if (componentNode == null)
            {
                var serviceNode = GetOrCreateServiceNode(endpointId,
                                                         serviceViewModel);

                componentNode = new ComponentNode(viewModel,
                                                  serviceNode);
                this.Nodes.Add(componentNode);
            }

            return componentNode;
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
                groupNode.ChildNodes.ForEach(x => DeleteNode(x));
            }

            // Remove node
            this.Nodes.Remove(node);
        }

        public void CleanAll()
        {
            this.Nodes.ToList().ForEach(x => this.Nodes.Remove(x));
            this.Connections.ToList().ForEach(x => this.Connections.Remove(x));
        }
    }
}

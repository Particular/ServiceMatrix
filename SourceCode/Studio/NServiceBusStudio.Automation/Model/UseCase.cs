using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractEndpoint;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace NServiceBusStudio
{
    partial interface IUseCase
    {
        IEnumerable<IAbstractEndpoint> RelatedEndpoints { get; }
        IEnumerable<IComponent> RelatedComponents { get; }
        IEnumerable<ICommand> RelatedCommands { get; }
        IEnumerable<IEvent> RelatedEvents { get; }
        IEnumerable<IAbstractEndpoint> EndpointsStartingUseCases { get; }

        void AddRelatedElement(IProductElement element);
        void RemoveRelatedElement(IProductElement element);
        void AddEndpointStartingUseCase(IAbstractEndpoint endpoint);
    }

    partial class UseCase
    {
        partial void Initialize()
        {
            this.EnsureRelatedElementListsExist();
            this.PopulateRelatedElements();
        }

        private void PopulateRelatedElements()
        {
            foreach (var related in this.UseCaseLinks)
            {
                var linked = this.As<IProductElement>().Root.Traverse().FirstOrDefault(i => i.Id == related.LinkedElementId);
                if (linked != null)
                {
                    this.InternalAddRelatedElement(linked.As<IProductElement>(), related.StartsUseCase);
                }
            }
        }

        public void AddRelatedElement(IProductElement element)
        {
            this.EnsureRelatedElementListsExist();
            if (!this.UseCaseLinks.Any(l => l.LinkedElementId == element.Id))
            {
                this.InternalAddRelatedElement(element, false, true);
            }
        }

        public void AddEndpointStartingUseCase(IAbstractEndpoint endpoint)
        {
            var element = endpoint.As<IProductElement>();
            this.EnsureRelatedElementListsExist();
            if (!this.UseCaseLinks.Any(l => l.LinkedElementId == element.Id))
            {
                this.InternalAddRelatedElement(element, true, true);
            }
        }

        private void EnsureRelatedElementListsExist()
        {
            if (this.relatedEndpoints == null)
            {
                this.endpointsStartingUseCases = new List<IAbstractEndpoint>();
                this.relatedEndpoints = new List<IAbstractEndpoint>();
                this.relatedComponents = new List<IComponent>();
                this.relatedCommands = new List<ICommand>();
                this.relatedEvents = new List<IEvent>();
            }
        }

        public void InternalAddRelatedElement(IProductElement element, bool isStarting = false, bool createLink = false)
        {
            if (element != null)
            {
                var command = element.As<ICommand>();
                var @event = element.As<IEvent>();
                var endpoint = element.As<IAbstractEndpoint>();
                var component = element.As<IComponent>();

                IUseCaseLink link = null;

                if (createLink)
                {
                    link = this.CreateUseCaseLink(element.Id.ToString());
                    link.LinkedElementId = element.Id;
                    if (command != null)
                    {
                        link.ElementType = "Command";
                    }
                    else if (@event != null)
                    {
                        link.ElementType = "Event";
                    }
                    else if (endpoint != null)
                    {
                        link.ElementType = "Endpoint";
                    }
                    else if (component != null)
                    {
                        link.ElementType = "Component";
                    }

                    link.StartsUseCase = isStarting;
                }

                if (command != null && !this.relatedCommands.Contains(command))
                {
                    this.relatedCommands.Add(command);
                }
                else if (@event != null && !this.relatedEvents.Contains(@event))
                {
                    this.relatedEvents.Add(@event);
                }
                else if (endpoint != null)
                {
                    if (isStarting && !this.endpointsStartingUseCases.Contains(endpoint))
                    {
                        this.endpointsStartingUseCases.Add(endpoint);
                    }
                    else if (!this.relatedEndpoints.Contains(endpoint) && !this.endpointsStartingUseCases.Contains(endpoint))
                    {
                        this.relatedEndpoints.Add(endpoint);
                    }
                }
                else if (component != null && !this.relatedComponents.Contains(component))
                {
                    this.relatedComponents.Add(component);
                }
            }
        }

        public void RemoveRelatedElement(IProductElement element)
        {
            var command = element.As<ICommand>();
            var @event = element.As<IEvent>();
            var endpoint = element.As<IAbstractEndpoint>();
            var component = element.As<IComponent>();
            var link = this.UseCaseLinks.FirstOrDefault(l => l.LinkedElementId == element.Id);
            if (link != null)
            {
                link.Delete();
            }

            if (command != null && relatedCommands.Contains(command))
            {
                this.relatedCommands.Remove(command);
            }
            else if (@event != null && this.relatedEvents.Contains(@event))
            {
                this.relatedEvents.Remove(@event);
            }
            else if (endpoint != null && this.relatedEndpoints.Contains(endpoint))
            {
                this.relatedEndpoints.Remove(endpoint);
            }
            else if (component != null && this.relatedComponents.Contains(component))
            {
                this.relatedComponents.Remove(component);
            }
        }

        private List<IAbstractEndpoint> relatedEndpoints;
        public IEnumerable<IAbstractEndpoint> RelatedEndpoints
        {
            get 
            {
                return this.RelatedComponents.SelectMany(c => c.DeployedTo)
                    .Except(this.EndpointsStartingUseCases)
                    .Distinct();
            }
        }

        private List<IComponent> relatedComponents;
        public IEnumerable<IComponent> RelatedComponents
        {
            get
            {
                return this.relatedComponents;
            }
        }

        private List<ICommand> relatedCommands;
        public IEnumerable<ICommand> RelatedCommands
        {
            get
            {
                return this.relatedCommands;
            }
        }

        private List<IEvent> relatedEvents;
        public IEnumerable<IEvent> RelatedEvents
        {
            get
            {
                return this.relatedEvents;
            }
        }

        private List<IAbstractEndpoint> endpointsStartingUseCases;
        public IEnumerable<IAbstractEndpoint> EndpointsStartingUseCases
        {
            get { return this.endpointsStartingUseCases; }
        }


    }
}

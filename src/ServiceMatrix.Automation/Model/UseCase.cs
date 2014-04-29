namespace NServiceBusStudio
{
    using System.Collections.Generic;
    using System.Linq;
    using AbstractEndpoint;
    using NuPattern.Runtime;

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
            EnsureRelatedElementListsExist();
            PopulateRelatedElements();
        }

        private void PopulateRelatedElements()
        {
            foreach (var related in UseCaseLinks)
            {
                var linked = As<IProductElement>().Root.Traverse().FirstOrDefault(i => i.Id == related.LinkedElementId);
                if (linked != null)
                {
                    InternalAddRelatedElement(linked.As<IProductElement>(), related.StartsUseCase);
                }
            }
        }

        public void AddRelatedElement(IProductElement element)
        {
            EnsureRelatedElementListsExist();
            if (UseCaseLinks.All(l => l.LinkedElementId != element.Id))
            {
                InternalAddRelatedElement(element, false, true);
            }
        }

        public void AddEndpointStartingUseCase(IAbstractEndpoint endpoint)
        {
            var element = endpoint.As<IProductElement>();
            EnsureRelatedElementListsExist();
            if (UseCaseLinks.All(l => l.LinkedElementId != element.Id))
            {
                InternalAddRelatedElement(element, true, true);
            }
        }

        private void EnsureRelatedElementListsExist()
        {
            if (relatedEndpoints == null)
            {
                endpointsStartingUseCases = new List<IAbstractEndpoint>();
                relatedEndpoints = new List<IAbstractEndpoint>();
                relatedComponents = new List<IComponent>();
                relatedCommands = new List<ICommand>();
                relatedEvents = new List<IEvent>();
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

                if (createLink)
                {
                    var link = CreateUseCaseLink(element.Id.ToString());
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

                if (command != null && !relatedCommands.Contains(command))
                {
                    relatedCommands.Add(command);
                }
                else if (@event != null && !relatedEvents.Contains(@event))
                {
                    relatedEvents.Add(@event);
                }
                else if (endpoint != null)
                {
                    if (isStarting && !endpointsStartingUseCases.Contains(endpoint))
                    {
                        endpointsStartingUseCases.Add(endpoint);
                    }
                    else if (!relatedEndpoints.Contains(endpoint) && !endpointsStartingUseCases.Contains(endpoint))
                    {
                        relatedEndpoints.Add(endpoint);
                    }
                }
                else if (component != null && !relatedComponents.Contains(component))
                {
                    relatedComponents.Add(component);
                }
            }
        }

        public void RemoveRelatedElement(IProductElement element)
        {
            var command = element.As<ICommand>();
            var @event = element.As<IEvent>();
            var endpoint = element.As<IAbstractEndpoint>();
            var component = element.As<IComponent>();
            var link = UseCaseLinks.FirstOrDefault(l => l.LinkedElementId == element.Id);
            if (link != null)
            {
                link.Delete();
            }

            if (command != null && relatedCommands.Contains(command))
            {
                relatedCommands.Remove(command);
            }
            else if (@event != null && relatedEvents.Contains(@event))
            {
                relatedEvents.Remove(@event);
            }
            else if (endpoint != null && relatedEndpoints.Contains(endpoint))
            {
                relatedEndpoints.Remove(endpoint);
            }
            else if (component != null && relatedComponents.Contains(component))
            {
                relatedComponents.Remove(component);
            }
        }

        private List<IAbstractEndpoint> relatedEndpoints;
        public IEnumerable<IAbstractEndpoint> RelatedEndpoints
        {
            get 
            {
                return RelatedComponents.SelectMany(c => c.DeployedTo)
                    .Except(EndpointsStartingUseCases)
                    .Distinct();
            }
        }

        private List<IComponent> relatedComponents;
        public IEnumerable<IComponent> RelatedComponents
        {
            get
            {
                return relatedComponents;
            }
        }

        private List<ICommand> relatedCommands;
        public IEnumerable<ICommand> RelatedCommands
        {
            get
            {
                return relatedCommands;
            }
        }

        private List<IEvent> relatedEvents;
        public IEnumerable<IEvent> RelatedEvents
        {
            get
            {
                return relatedEvents;
            }
        }

        private List<IAbstractEndpoint> endpointsStartingUseCases;
        public IEnumerable<IAbstractEndpoint> EndpointsStartingUseCases
        {
            get { return endpointsStartingUseCases; }
        }


    }
}

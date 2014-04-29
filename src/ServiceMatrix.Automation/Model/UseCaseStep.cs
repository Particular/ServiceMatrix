namespace NServiceBusStudio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NuPattern.Runtime;
    using AbstractEndpoint;

    partial interface IUseCaseStep
    {
        UseCaseStepPattern Pattern { get; set; }

        IEnumerable<IAbstractEndpoint> RelatedEndpoints { get; }
        IEnumerable<IComponent> RelatedComponents { get; }
        //IEnumerable<ICommand> RelatedCommands { get; }
        //IEnumerable<IEvent> RelatedEvents { get; }
    }

    partial class UseCaseStep
    {
        private UseCaseStepPattern pattern;
        public UseCaseStepPattern Pattern
        {
            get { return pattern; }
            set
            {
                if (pattern != value)
                {
                    pattern = value;
                    PatternValue = pattern.ToString();
                }
            }
        }


        partial void Initialize()
        {
            PatternValueChanged += (s, e) =>
                {
                    Pattern = (UseCaseStepPattern) Enum.Parse(typeof(UseCaseStepPattern), PatternValue);
                };
            if (PatternValue != null && PatternValue.Length > 0)
            {
                pattern = (UseCaseStepPattern)Enum.Parse(typeof(UseCaseStepPattern), PatternValue);
            }
        }

        private List<IAbstractEndpoint> relatedEndpoints;
        public IEnumerable<IAbstractEndpoint> RelatedEndpoints
        {
            get
            {
                if (relatedEndpoints == null)
                {
                    relatedEndpoints = new List<IAbstractEndpoint>();

                    relatedEndpoints.AddRange(
                        TheApplication.Design.Endpoints.GetAll()
                        .Where(a => a.InstanceName == SourceEndpointName || a.InstanceName == TargetEndpointName));
                }
                return relatedEndpoints;
            }
        }

        public IApplication TheApplication { get { return As<IProductElement>().Root.As<IApplication>(); } }


        private List<IComponent> relatedComponents;
        public IEnumerable<IComponent> RelatedComponents
        {
            get
            {
                if (relatedComponents == null)
                {
                    relatedComponents = new List<IComponent>();
                    
                    //this.relatedComponents.AddRange(
                    //    this.TheApplication.Design.
                    //    );
                }
                return relatedComponents;

            }
        }

    }

    public enum UseCaseStepPattern
    {
        EndpointSendsCommandToEndpoint,
        EndpointDoesComponent,
        EndpointPublishesEvent,
        EndpointSubscribesToEvent,
        EndpointReceivesCommand,
    };
}

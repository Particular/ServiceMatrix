using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Patterning.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio
{
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
            get { return this.pattern; }
            set
            {
                if (this.pattern != value)
                {
                    this.pattern = value;
                    this.PatternValue = this.pattern.ToString();
                }
            }
        }


        partial void Initialize()
        {
            this.PatternValueChanged += (s, e) =>
                {
                    this.Pattern = (UseCaseStepPattern) Enum.Parse(typeof(UseCaseStepPattern), PatternValue);
                };
            if (this.PatternValue != null && this.PatternValue.Length > 0)
            {
                this.pattern = (UseCaseStepPattern)Enum.Parse(typeof(UseCaseStepPattern), PatternValue);
            }
        }

        private List<IAbstractEndpoint> relatedEndpoints;
        public IEnumerable<IAbstractEndpoint> RelatedEndpoints
        {
            get
            {
                if (this.relatedEndpoints == null)
                {
                    this.relatedEndpoints = new List<IAbstractEndpoint>();

                    this.relatedEndpoints.AddRange(
                        this.TheApplication.Design.Endpoints.GetAll()
                        .Where(a => a.InstanceName == this.SourceEndpointName || a.InstanceName == this.TargetEndpointName));
                }
                return this.relatedEndpoints;
            }
        }

        public IApplication TheApplication { get { return this.As<IProductElement>().Root.As<IApplication>(); } }


        private List<IComponent> relatedComponents;
        public IEnumerable<IComponent> RelatedComponents
        {
            get
            {
                if (this.relatedComponents == null)
                {
                    this.relatedComponents = new List<IComponent>();
                    
                    //this.relatedComponents.AddRange(
                    //    this.TheApplication.Design.
                    //    );
                }
                return this.relatedComponents;

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

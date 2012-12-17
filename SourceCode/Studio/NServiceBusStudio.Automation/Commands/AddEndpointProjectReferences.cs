using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel.Composition;
using NServiceBusStudio.Automation.Extensions;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.Patterning.Runtime;
using AbstractEndpoint;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Add Endpoint Project References")]
    [Description("Add references in the Endpoint Project to the required projects")]
    [CLSCompliant(false)]
    public class AddEndpointProjectReferences : FeatureCommand
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }

        public override void Execute()
        {
            var component = Model.Helpers.GetComponentFromLinkedElement(this.CurrentElement);
            var service = component.Parent.Parent;

            foreach (var endpoint in service.Parent.Parent.Endpoints.GetAll()
                .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component)))
            {
                if (component.Publishes.EventLinks.Any() || component.Subscribes.SubscribedEventLinks.Any())
                {
                    endpoint.Project.AddReference(service.Parent.Parent.ContractsProject.AsElement().GetProject());
                }
                if (component.Subscribes.ProcessedCommandLinks.Any() || component.Publishes.CommandLinks.Any())
                {
                    endpoint.Project.AddReference(service.Parent.Parent.InternalMessagesProject.AsElement().GetProject());
                }
            }
        }
    }
}

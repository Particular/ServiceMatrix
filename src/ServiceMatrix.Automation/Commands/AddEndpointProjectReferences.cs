
namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using NServiceBusStudio.Automation.Extensions;
    using System.ComponentModel.DataAnnotations;
    using NuPattern.Runtime;
    using NServiceBusStudio.Automation.Model;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Add Endpoint Project References")]
    [Description("Add references in the Endpoint Project to the required projects")]
    [CLSCompliant(false)]
    public class AddEndpointProjectReferences : Command
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
            var component = Helpers.GetComponentFromLinkedElement(CurrentElement);
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

using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using AbstractEndpoint;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using NServiceBusStudio.Automation.Extensions;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    [DisplayName("Add Endpoint Project References")]
    [Description("Add references in the Service Project to the required projects")]
    [CLSCompliant(false)]
    public class AddEndpointProjectReferences : FeatureCommand
    {
        [Required]
        [Import(AllowDefault = true)]
        IProductElement Endpoint { get; set; }

        [Import]
        public IServiceProvider ServiceProvider { get; set; }

        public override void Execute()
        {
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
            var endpoint = this.Endpoint.As<IToolkitElement>() as IAbstractEndpoint;

            foreach (var subscribedComponent in endpoint.EndpointComponents.AbstractComponentLinks)
            {
                endpoint.Project.AddReference(subscribedComponent.ComponentReference.Value.Project);
            }
        }
    }
}

namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using AbstractEndpoint;
    using NServiceBusStudio.Automation.Extensions;
    using NuPattern.Runtime;
    using Microsoft.VisualStudio.Shell;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Add Endpoint Project References")]
    [Description("Add references in the Service Project to the required projects")]
    [CLSCompliant(false)]
    public class AddEndpointProjectReferences : Command
    {
        [Required]
        [Import(AllowDefault = true)]
        IProductElement Endpoint { get; set; }

        [Import(typeof(SVsServiceProvider))]
        public IServiceProvider ServiceProvider { get; set; }

        public override void Execute()
        {
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
            var endpoint = Endpoint.As<IToolkitElement>() as IAbstractEndpoint;

            foreach (var subscribedComponent in endpoint.EndpointComponents.AbstractComponentLinks)
            {
                endpoint.Project.AddReference(subscribedComponent.ComponentReference.Value.Project);
            }
        }
    }
}

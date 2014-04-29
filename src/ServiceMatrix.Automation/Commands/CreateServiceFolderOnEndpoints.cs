namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using NuPattern.Runtime;
    using NServiceBusStudio.Automation.Model;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Create Service Folder On Endpoints")]
    [Description("Add folder for service components code on Endpoint Projects")]
    [CLSCompliant(false)]
    public class CreateServiceFolderOnEndpoints : Command
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

                if (endpoint.Project.Folders.All(f => f.Name != service.CodeIdentifier))
                {
                    try
                    {
                        endpoint.Project.CreateFolder(service.CodeIdentifier);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}

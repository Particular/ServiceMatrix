using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Composition;
using NServiceBusStudio.Automation.Extensions;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Create Service Folder On Endpoints")]
    [Description("Add folder for service components code on Endpoint Projects")]
    [CLSCompliant(false)]
    public class CreateServiceFolderOnEndpoints : NuPattern.Runtime.Command
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

                if (!endpoint.Project.Folders.Any(f => f.Name == service.CodeIdentifier))
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

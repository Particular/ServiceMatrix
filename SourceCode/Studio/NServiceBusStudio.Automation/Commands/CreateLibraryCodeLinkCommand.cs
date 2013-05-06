using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands
{
    public class CreateLibraryCodeLinkCommand : NuPattern.Runtime.Command
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }
        
        public override void Execute()
        {
            var lr = this.CurrentElement.As<ILibraryReference>();
            if (lr != null)
            {
                var component = lr.Parent.Parent;
                if (component.DeployedTo != null)
                {
                    component.EndpointDefined(null);
                }
            }
           
        }
    }
}

using NuPattern.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("SetUnfoldedCustomCode")]
    [Description("SetUnfoldedCustomCode")]
    [CLSCompliant(false)]
    public class SetUnfoldedCustomCode : NuPattern.Runtime.Command
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
            var app = this.CurrentElement.Root.As<IApplication>();
            var component = this.CurrentElement.As<IComponent>();

            var isDeployed = app.Design.Endpoints.GetAll()
                    .Any(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component));

            if (!isDeployed)
            {
                return;
            }
            
            component.UnfoldedCustomCode = true;
        }
    
    }
}

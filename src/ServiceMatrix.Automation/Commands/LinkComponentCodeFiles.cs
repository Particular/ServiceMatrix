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
    [DisplayName("Links Component Files when necessary")]
    [Description("Links Component Files when necessary")]
    [CLSCompliant(false)]
    public class LinkComponentCodeFiles : NuPattern.Runtime.Command
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
            var component = this.CurrentElement.As<IComponent>();
            var app = this.CurrentElement.Root.As<IApplication>();
            var endpoints = app.Design.Endpoints.GetAll().Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component));

            foreach (var e in endpoints)
            {
                component.AddLinks(e);
            }

            component.UnfoldedCustomCode = true;
        }
    
    }
}

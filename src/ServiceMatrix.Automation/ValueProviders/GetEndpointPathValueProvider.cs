using NuPattern.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.ValueProviders
{
    [CLSCompliant(false)]
    [DisplayName("GetEndpointPathValueProvider")]
    [Category("General")]
    [Description("GetEndpointPathValueProvider")]
    public class GetEndpointPathValueProvider : ValueProvider
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public bool AddInfrastructureFolder { get; set; }

        public override object Evaluate()
        {
            try
            {
                var app = this.CurrentElement.Root.As<IApplication>();
                var component = this.CurrentElement.As<IComponent>();

                var endpoint = app.Design.Endpoints.GetAll()
                    .FirstOrDefault(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component));

                if (endpoint != null)
                {
                    var path = String.Format ("\\{0}.{1}", app.CodeIdentifier, endpoint.InstanceName);
                    if (this.AddInfrastructureFolder)
                    {
                        path += "\\Infrastructure";                        
                    }

                    path += String.Format("\\{0}", component.Parent.Parent.CodeIdentifier);
                    return path;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}

namespace NServiceBusStudio.Automation.ValueProviders
{
    using NuPattern.Runtime;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

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
                var app = CurrentElement.Root.As<IApplication>();
                var component = CurrentElement.As<NServiceBusStudio.IComponent>();

                var endpoint = app.Design.Endpoints.GetAll()
                    .FirstOrDefault(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component));

                if (endpoint != null)
                {
                    var path = String.Format ("\\{0}.{1}", app.InstanceName, endpoint.InstanceName);
                    if (AddInfrastructureFolder)
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

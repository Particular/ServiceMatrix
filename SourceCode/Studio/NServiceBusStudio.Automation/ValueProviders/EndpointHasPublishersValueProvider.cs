using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio.Automation.ValueProviders
{
    [CLSCompliant(false)]
    [DisplayName("Endpoint Has Component that Publishes an Event")]
    [Category("General")]
    [Description("Endpoint Has Component that Publishes an Event.")]
    public class EndpointHasPublishersValueProvider : ValueProvider
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

        public override object Evaluate()
        {
            var endpoint = this.CurrentElement.As<IToolkitInterface>() as IAbstractEndpoint;
            return (object)(endpoint.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value.Publishes.EventLinks.Any()));
        }
    }
}

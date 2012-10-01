using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Patterning.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio.Automation.ValueProviders
{
    [CLSCompliant(false)]
    [DisplayName("Parent Component Publishes An Event")]
    [Category("General")]
    [Description("Returns true if the parent component publishes an event.")]
    public class ParentComponentPublishesAnEventValueProvider : ComponentFromLinkBasedValueProvider
    {
        public override object Evaluate()
        {
            return this.Component.Publishes.EventLinks.Any();
        }
    }
}

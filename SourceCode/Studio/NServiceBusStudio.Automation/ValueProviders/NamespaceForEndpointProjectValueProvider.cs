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
using System.Xml.Serialization;

namespace NServiceBusStudio.Automation.ValueProviders
{
    [CLSCompliant(false)]
    [DisplayName("Namespace For Endpoint Project")]
    [Category("General")]
    [Description("Returns the Namespace for Endpoint project + Service folder.")]
    public class NamespaceForEndpointProjectValueProvider : ComponentFromLinkBasedValueProvider
    {
        public override object Evaluate()
        {
            // this is a component fixed to the Code project now....
            return this.Component.Parent.Parent.Parent.Parent.Parent.CodeIdentifier + "."
                        + this.Component.Parent.Parent.CodeIdentifier;
        }

    }
}

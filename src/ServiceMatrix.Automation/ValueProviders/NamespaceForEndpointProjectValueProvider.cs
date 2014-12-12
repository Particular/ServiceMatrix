using System;
using System.ComponentModel;

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
            var root = CurrentElement.Root.As<IApplication>();

            // this is a component fixed to the Code project now....
            return root.InstanceName + "." + Component.Parent.Parent.CodeIdentifier;
        }
    }
}

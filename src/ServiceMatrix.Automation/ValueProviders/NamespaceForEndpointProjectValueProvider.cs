namespace NServiceBusStudio.Automation.ValueProviders
{
    using System;
    using System.ComponentModel;

    [CLSCompliant(false)]
    [DisplayName("Namespace For Endpoint Project")]
    [Category("General")]
    [Description("Returns the Namespace for Endpoint project + Service folder.")]
    public class NamespaceForEndpointProjectValueProvider : ComponentFromLinkBasedValueProvider
    {
        public override object Evaluate()
        {
            // this is a component fixed to the Code project now....
            return Component.Parent.Parent.Parent.Parent.Parent.CodeIdentifier + "."
                        + Component.Parent.Parent.CodeIdentifier;
        }

    }
}

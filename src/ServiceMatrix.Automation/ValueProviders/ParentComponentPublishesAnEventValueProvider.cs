namespace NServiceBusStudio.Automation.ValueProviders
{
    using System;
    using System.Linq;
    using System.ComponentModel;

    [CLSCompliant(false)]
    [DisplayName("Parent Component Publishes An Event")]
    [Category("General")]
    [Description("Returns true if the parent component publishes an event.")]
    public class ParentComponentPublishesAnEventValueProvider : ComponentFromLinkBasedValueProvider
    {
        public override object Evaluate()
        {
            return Component.Publishes.EventLinks.Any();
        }
    }
}

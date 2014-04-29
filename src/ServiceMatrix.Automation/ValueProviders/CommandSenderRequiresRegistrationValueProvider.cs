namespace NServiceBusStudio.Automation.ValueProviders
{
    using System;
    using System.Linq;
    using System.ComponentModel;

    [CLSCompliant(false)]
    [DisplayName("CommandSenderRequiresRegistrationValueProvider")]
    [Category("General")]
    [Description("CommandSenderRequiresRegistrationValueProvider")]
    public class CommandSenderRequiresRegistrationValueProvider : ComponentFromLinkBasedValueProvider
    {
        public override object Evaluate()
        {
            try
            {
                var endpoints = Service.Parent.Parent.Endpoints.GetAll()
                    .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == Component));

                return endpoints.Any(ep => ep.CommandSenderNeedsRegistration);
            }
            catch
            {
                return false;
            }
        }
    }
}

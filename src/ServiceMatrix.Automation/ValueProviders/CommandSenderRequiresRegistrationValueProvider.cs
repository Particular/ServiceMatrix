using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio.Automation.ValueProviders
{
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
                var endpoints = this.Service.Parent.Parent.Endpoints.GetAll()
                    .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this.Component));

                return endpoints.Any(ep => ep.CommandSenderNeedsRegistration);
            }
            catch
            {
                return false;
            }
        }
    }
}

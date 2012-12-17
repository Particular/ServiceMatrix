using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using NServiceBusStudio.Automation.Extensions;
using AbstractEndpoint;
using NServiceBusStudio;

namespace NServiceBusStudio
{
    partial interface INServiceBusHost : IAbstractEndpoint
    {

    }

    partial class NServiceBusHost
    {
        public IProject Project
        {
            get { return this.AsElement().GetProject(); }
        }

        public IAbstractEndpointComponents EndpointComponents
        {
            get { return (NServiceBusHostComponents)this.NServiceBusHostComponents; }
        }

        partial void Initialize()
        {
            AbstractEndpointExtensions.RaiseOnInitializing(this);
            this.ErrorQueueChanged += (s, e) =>
            {
                this.SetOverridenProperties("ErrorQueue", this.ErrorQueue != this.AsElement().Root.As<IApplication>().ErrorQueue);
            };
            this.ForwardReceivedMessagesToChanged += (s, e) =>
            {
                this.SetOverridenProperties("ForwardReceivedMessagesTo", this.ForwardReceivedMessagesTo != this.AsElement().Root.As<IApplication>().ForwardReceivedMessagesTo);
            };
        }

        private List<string> overridenProperties = new List<string>();

        public IEnumerable<string> OverridenProperties
        {
            get { return this.overridenProperties; }
        }

        private void SetOverridenProperties(string propertyName, bool doOverride)
        {
            if (!doOverride)
            {
                if (this.overridenProperties.Contains(propertyName))
                {
                    this.overridenProperties.Remove(propertyName);
                }
            }
            else
            {
                if (!this.overridenProperties.Contains(propertyName))
                {
                    this.overridenProperties.Add(propertyName);
                }
            }
        }

        public EndpointCustomizationFuncs Customization { get { return null; } }
    }
}

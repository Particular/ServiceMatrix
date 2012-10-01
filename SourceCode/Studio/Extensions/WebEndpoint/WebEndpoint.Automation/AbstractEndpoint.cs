using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using NServiceBusStudio.Automation.Extensions;
using AbstractEndpoint;
using System;
using NServiceBusStudio;
using System.Collections.Generic;

namespace WebEndpoint
{
    partial class AbstractEndpoint
    {
        public IProject Project 
        {
            get { return this.AsProduct().GetProject(); }
        }

        public IAbstractEndpointComponents EndpointComponents
        {
            get { return (EndpointComponents)this.DefaultView.EndpointComponents; }
        }

        partial void Initialize()
        {
            AbstractEndpointExtensions.RaiseOnInitializing(this);
            this.ErrorQueueChanged += (s, e) =>
            {
                this.SetOverridenProperties("ErrorQueue", this.ErrorQueue != this.AsProduct().Root.As<IApplication>().ErrorQueue);
            };
            this.ForwardReceivedMessagesToChanged += (s, e) =>
            {
                this.SetOverridenProperties("ForwardReceivedMessagesTo", this.ForwardReceivedMessagesTo != this.AsProduct().Root.As<IApplication>().ForwardReceivedMessagesTo);
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

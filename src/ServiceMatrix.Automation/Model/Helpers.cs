using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Model
{
    public static class Helpers
    {
        public static IComponent GetComponentFromLinkedElement(IProductElement currentElement)
        {
            IComponent component = null;

            if (currentElement.As<IComponent>() != null)
            {
                return currentElement.As<IComponent>();
            }

            var subscribedEventLink = currentElement.As<ISubscribedEventLink>();
            var processedCommandLink = currentElement.As<IProcessedCommandLink>();
            var eventLink = currentElement.As<IEventLink>();
            var commandLink = currentElement.As<ICommandLink>();

            if (subscribedEventLink != null)
            {
                component = subscribedEventLink.Parent.Parent;
            }
            else if (processedCommandLink != null)
            {
                component = processedCommandLink.Parent.Parent;
            }
            else if (eventLink != null)
            {
                component = eventLink.Parent.Parent;
            }
            else if (commandLink != null)
            {
                component = commandLink.Parent.Parent;
            }

            return component;
        }

        public static INServiceBusMVC GetMvcEndpointFromLinkedElement(IProductElement currentElement)
        {
            var currentComponent = currentElement.As<NServiceBusStudio.IComponent>();
            var app = currentElement.Root.As<IApplication>();

            foreach (var endpoint in app.Design.Endpoints.GetMvcEndpoints())
            {
                var componentLinks = endpoint.EndpointComponents.AbstractComponentLinks;
                if (componentLinks.Select(link => link.ComponentReference.Value).Any(component => component == currentComponent))
                {
                    return endpoint;
                }
            }

            return null;
        }

        
    }

}

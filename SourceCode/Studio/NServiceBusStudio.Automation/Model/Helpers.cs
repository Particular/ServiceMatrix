using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Patterning.Runtime;

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
    }
}

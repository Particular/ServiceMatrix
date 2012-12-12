using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractEndpoint;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace NServiceBusStudio.Automation.Extensions
{
    public static class GenerateComponentsHandlerOrder
    {
        public static string GetComponentsHandlerOrder(this IProductElement endpoint)
        {
            var sb = new StringBuilder();
            var app = endpoint.Root.As<NServiceBusStudio.IApplication>();
            var endpoints = app.Design.Endpoints.As<IAbstractElement>().Extensions;
            var sourceComponents = (endpoint.As<IToolkitInterface>() as IAbstractEndpoint).EndpointComponents.AbstractComponentLinks.OrderBy(o => o.Order);
            var components = sourceComponents.Select(ac => ac.ComponentReference.Value)
                                       .Where(c => c.Subscribes.ProcessedCommandLinks.Any() || c.Subscribes.SubscribedEventLinks.Any())
                                       .Select(cmp => HandlerTypeFromComponent(cmp));

            // Add authentication first if needed
            if (app.HasAuthentication && (endpoint.As<IToolkitInterface>() as IAbstractEndpoint).Project != null)
            {
                components = new string[] { string.Format("{0}.Infrastructure.Authentication", (endpoint.As<IToolkitInterface>() as IAbstractEndpoint).Project.Data.RootNamespace) }.Union(components);
            }

            ////
            if (components.Count() > 1)
            {
                sb.Append(@"
	    public void SpecifyOrder(Order order)
	    {
	        order.Specify(");

                int pos = 1;
                foreach (var c in components)
                {
                    if (pos == 1)
                    {
                        sb.AppendFormat("First<{0}>", c);
                    }
                    else if (pos == 2)
                    {
                        sb.AppendFormat(".Then<{0}>()", c);
                    }
                    else
                    {
                        sb.AppendFormat(".AndThen<{0}>()", c);
                    }
                    pos++;
                }
                sb.Append(@");
	    }");
            }

            ////

            return sb.ToString();
        }

        private static string HandlerTypeFromComponent(IComponent component)
        {
            return component.Namespace + "." + component.CodeIdentifier;
        }

    }
}

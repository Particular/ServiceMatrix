using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractEndpoint;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace NServiceBusStudio.Automation.Extensions
{
    public static class GenerateUnicastBusConfig
    {
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> source)
        {
            return source ?? new T[] { };
        }

        public static string GetMessageEndpointMappingsConfig(this IProductElement endpoint)
        {
            var sb = new StringBuilder();
            var app = endpoint.Root.As<NServiceBusStudio.IApplication>();
            var endpoints = app.Design.Endpoints.As<IAbstractElement>().Extensions;

            try
            {
                foreach (var component in (endpoint.As<IToolkitInterface>() as IAbstractEndpoint).EndpointComponents
                                           .AbstractComponentLinks.Select(ac => ac.ComponentReference.Value)
                                           .Where(c => c.Publishes.CommandLinks.Any() || c.Subscribes.SubscribedEventLinks.Any()))
                {
                    // Add mappings from the messages(command types) sent by this endpoint
                    // to the endpoints (root namespace for endpoint projects) processing them.
                    foreach (var command in component.Publishes.CommandLinks.Select(c => c.CommandReference.Value))
                    {
                        var componentProcessingCommand = app.Design.Services.Service
                                                            .SelectMany(s => s.Components.Component
                                                                                .Where(c => c.Subscribes.ProcessedCommandLinks
                                                                                            .Any(cl => cl.CommandReference
                                                                                                    .Value == command)))
                                                            .FirstOrDefault();
                        if (componentProcessingCommand != null)
                        {
                            foreach (var endpointHost in FindComponentHostEndpoints(endpoints, componentProcessingCommand))
                            {
                                if (endpointHost != null && endpoint.InstanceName != endpointHost.InstanceName)
                                {
                                    sb.AppendLine(String.Format("<add Messages=\"{0}\" Endpoint=\"{1}\" />",
                                        command.Parent.Namespace + "." + command.CodeIdentifier + ", " + app.InternalMessagesProjectName,
                                        (endpointHost != null) ? endpointHost.GetProject().Data.RootNamespace : ""));
                                }
                            }
                        }
                    }

                    // Add mappings from the messages(event types) subscribed by this endpoint
                    // to the endpoints (root namespace for endpoint projects) publishing them.
                    foreach (var eventt in component.Subscribes.SubscribedEventLinks.Select(c => c.EventReference.Value)
                        .Where(e => e != null))
                    {
                        var componentPublishingEvent = app.Design.Services.Service
                                                            .SelectMany(s => s.Components.Component
                                                                                .Where(c => c.Publishes.EventLinks
                                                                                            .Any(el => el.EventReference
                                                                                                    .Value == eventt)))
                                                            .FirstOrDefault();
                        if (componentPublishingEvent != null)
                        {
                            foreach (var endpointHost in FindComponentHostEndpoints(endpoints, componentPublishingEvent))
                            {
                                sb.AppendLine(String.Format("<add Messages=\"{0}\" Endpoint=\"{1}\" />",
                                    eventt.Parent.Namespace + "." + eventt.CodeIdentifier + ", " + app.ContractsProjectName,
                                    (endpointHost != null) ? endpointHost.GetProject().Data.RootNamespace : ""));
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return sb.ToString();
        }

        private static NServiceBusStudio.IComponent FindProcessorComponent(NServiceBusStudio.ICommand command)
        {
            var service = command.Parent.Parent.Parent;
            return service.Components.Component.FirstOrDefault(c => c.Subscribes.ProcessedCommandLinks.Any(i => i.CommandReference.Value == command));
        }

        private static IEnumerable<IProductElement> FindProcessorEndpoints(IEnumerable<IProductElement> endpoints, NServiceBusStudio.IEvent eventt)
        {
            var service = eventt.Parent.Parent.Parent;
            var components = service.Components.Component.Where(c => c.Subscribes.SubscribedEventLinks.Any(i => i.EventReference.Value == eventt));
            return endpoints.Where(p => (p.As<IToolkitInterface>() as IAbstractEndpoint)
                                        .EndpointComponents.AbstractComponentLinks
                                        .Any(l => l.ComponentReference != null && components.Contains(l.ComponentReference.Value)));
        }

        private static IEnumerable<IProductElement> FindComponentHostEndpoints(IEnumerable<IProductElement> endpoints, NServiceBusStudio.IComponent component)
        {
            return endpoints
                .Where(ep =>
                    {
                        var abstractEndpoint = ep.As<IToolkitInterface>() as IAbstractEndpoint;
                        return abstractEndpoint != null
                            && abstractEndpoint.EndpointComponents.AbstractComponentLinks
                                                                  .Any(cl => cl.ComponentReference.Value == component);
                    });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using Microsoft.VisualStudio.Modeling;
using System.IO;
using Microsoft.VisualStudio.Modeling.Diagrams;
using AbstractEndpoint;

namespace NServiceBus.Modeling.EndpointDesign
{
	internal partial class EndpointDesignDocData
    {
        [Import(AllowDefault = true)]
        public IPatternManager ProductManager { get; set; }

        protected override void Load(string fileName, bool isReload)
        {
            base.Load(fileName, isReload);

            var endpointModelDSL = this.RootElement as EndpointModel;
            var diagramDSL = PresentationViewsSubject.GetPresentation(this.RootElement).First() as EndpointDesignDiagram;

            var app = this.ProductManager.Products.First().As<NServiceBusStudio.IApplication>();
            
            if (app == null)
                throw new Exception("Cannot get access to the NuPattern Model");

            var endpointName = Path.GetFileNameWithoutExtension(fileName);
            var endpoints = app.Design.Endpoints.GetAll();

            if (endpointName != "Endpoints")
            {
                var endpoint = endpoints.FirstOrDefault(e => e.InstanceName == endpointName);

                if (endpoint == null) 
                {
                    throw new Exception("Endpoint not found");
                }

                endpoints = new [] { endpoint };
            }

            GenerateDiagram(endpointModelDSL, endpoints);
        }

        private void GenerateDiagram(EndpointModel endpointModelDSL, IEnumerable<IAbstractEndpoint> endpoints)
        {
            OrderShapeAddedToDiagramRule.SendReceiveEndpointCounter = 0;

            foreach (var endpoint in endpoints)
            {
                GenerateEndpointDiagram(endpointModelDSL, endpoint);
            }
        }

        private void GenerateEndpointDiagram(EndpointModel endpointModelDSL, IAbstractEndpoint endpoint)
        {
            var endpointModel = endpointModelDSL.CreateSendReceiveEndpoint((e) =>
            {
                e.Name = endpoint.InstanceName;
                e.Description = endpoint.As<IProductElement>().Notes;

            }) as SendReceiveEndpoint;

            foreach (var endpointComponent in endpoint.As<IProductElement>().GetChildren())
            {
                var components = endpointComponent.As<IToolkitInterface>() as IAbstractEndpointComponents;

                if (components != null)
                {
                    foreach (var componentLink in components.AbstractComponentLinks)
                    {
                        var component = componentLink.ComponentReference.Value;

                        foreach (var @event in component.Subscribes.SubscribedEventLinks.Select(e => e.EventReference.Value).Where(v => v != null))
                        {
                            var eventModel = AddOrGetEvent(endpointModelDSL, @event.InstanceName, @event.Notes);
                            endpointModel.ProcessEvents.Add(eventModel);
                        }

                        foreach (var command in component.Subscribes.ProcessedCommandLinks.Select(c => c.CommandReference.Value))
                        {
                            var commandModel = AddOrGetCommand(endpointModelDSL, command.InstanceName, command.Notes);
                            endpointModel.ProcessCommands.Add(commandModel);
                        }

                        foreach (var @event in component.Publishes.EventLinks.Select(e => e.EventReference.Value))
                        {
                            var eventModel = AddOrGetEvent(endpointModelDSL, @event.InstanceName, @event.Notes);
                            endpointModel.EmittedEvents.Add(eventModel);
                        }

                        foreach (var command in component.Publishes.CommandLinks.Select(c => c.CommandReference.Value))
                        {
                            var commandModel = AddOrGetCommand(endpointModelDSL, command.InstanceName, command.Notes);
                            endpointModel.EmittedCommands.Add(commandModel);
                        }
                    }
                }
            }
        }

        private static Command AddOrGetCommand(EndpointModel endpointModelDSL, string commandInstanceName, string commandDescription)
        {
            var command = endpointModelDSL.Commands.FirstOrDefault(x => x.Name == commandInstanceName);

            if (command == null)
            {
                command = endpointModelDSL.CreateCommand((c) =>
                {
                    c.Name = commandInstanceName;
                    c.Description = commandDescription;
                }) as Command;
            }

            return command;
        }

        private static Event AddOrGetEvent(EndpointModel endpointModelDSL, string eventInstanceName, string eventDescription)
        {
            var @event = endpointModelDSL.Events.FirstOrDefault(x => x.Name == eventInstanceName);

            if (@event == null)
            {
                @event = endpointModelDSL.CreateEvent((e) =>
                {
                    e.Name = eventInstanceName;
                    e.Description = eventDescription;
                }) as Event;
            }

            return @event;
        }

        protected override void Save(string fileName)
        {
            // base.Save(fileName);
        }
    }
}

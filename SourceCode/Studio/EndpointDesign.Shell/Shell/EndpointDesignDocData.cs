using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Patterning.Runtime;
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
                throw new Exception("Cannot get access to the VSPAT Model");

            var endpoint = app.Design.Endpoints.GetAll().FirstOrDefault (e => e.InstanceName == Path.GetFileNameWithoutExtension(fileName));

            if (endpoint == null)
                throw new Exception("Endpoint not found");

            var endpointModel = endpointModelDSL.CreateSendReceiveEndpoint((e) =>
            {
                e.Name = endpoint.InstanceName;
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
                            var eventModel = AddOrGetEvent(endpointModelDSL, @event.InstanceName);
                            endpointModel.ProcessEvents.Add(eventModel);
                        }

                        foreach (var command in component.Subscribes.ProcessedCommandLinks.Select(c => c.CommandReference.Value))
                        {
                            var commandModel = AddOrGetCommand(endpointModelDSL, command.InstanceName);
                            endpointModel.ProcessCommands.Add(commandModel);
                        }

                        foreach (var @event in component.Publishes.EventLinks.Select(e => e.EventReference.Value))
                        {
                            var eventModel = AddOrGetEvent(endpointModelDSL, @event.InstanceName);
                            endpointModel.EmittedEvents.Add(eventModel);
                        }

                        foreach (var command in component.Publishes.CommandLinks.Select(c => c.CommandReference.Value))
                        {
                            var commandModel = AddOrGetCommand(endpointModelDSL, command.InstanceName);
                            endpointModel.EmittedCommands.Add(commandModel);
                        }
	                }
                }
	        }
        }

        private static Command AddOrGetCommand(EndpointModel endpointModelDSL, string commandInstanceName)
        {
            var command = endpointModelDSL.Commands.FirstOrDefault(x => x.Name == commandInstanceName);

            if (command == null)
            {
                command = endpointModelDSL.CreateCommand((c) =>
                {
                    c.Name = commandInstanceName;
                }) as Command;
            }

            return command;
        }

        private static Event AddOrGetEvent(EndpointModel endpointModelDSL, string eventInstanceName)
        {
            var @event = endpointModelDSL.Events.FirstOrDefault(x => x.Name == eventInstanceName);

            if (@event == null)
            {
                @event = endpointModelDSL.CreateEvent((e) =>
                {
                    e.Name = eventInstanceName;
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

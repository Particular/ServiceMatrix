using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using AbstractEndpoint;
using NServiceBusStudio.Automation.Dialog;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern;
using NuPattern.Presentation;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands
{
    public class ShowEventComponentPicker : NuPattern.Runtime.Command
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Window Factory, used to create a Window Dialog.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        private IDialogWindowFactory WindowFactory
        {
            get;
            set;
        }

        public override void Execute()
        {
            var endpoint = this.CurrentElement.As<IAbstractEndpoint>();

            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var app = this.CurrentElement.Root.As<IApplication>();

            // Get available events
            var elements = new Dictionary<string, ICollection<string>>();
            foreach (var service in app.Design.Services.Service)
            {
                elements.Add(service.InstanceName, service.Contract.Events.Event.Select(x => x.InstanceName).ToList());
            }

            var viewModel =
                new ElementHierarchyPickerViewModel
                {
                    SlaveName = "Event Name:",
                    Title = "Publish Event",
                    Elements = elements,
                };

            var picker = this.WindowFactory.CreateDialog<ElementHierarchyPicker>(viewModel);
            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    var selectedService = viewModel.SelectedMasterItem;
                    var selectedEvent = viewModel.SelectedSlaveItem;

                    var service = app.Design.Services.Service.FirstOrDefault(x => x.InstanceName == selectedService);
                    if (service == null)
                    {
                        service = app.Design.Services.CreateService(selectedService);
                    }

                    var @event = service.Contract.Events.Event.FirstOrDefault(x => x.InstanceName == selectedEvent);
                    if (@event == null)
                    {
                        @event = service.Contract.Events.CreateEvent(selectedEvent);
                    }

                    var component = service.Components.Component.FirstOrDefault(x => x.Publishes.EventLinks.Any(y => y.EventReference.Value == @event));
                    if (component == null)
                    {
                        component = service.Components.CreateComponent(@event.InstanceName + "Sender", x => x.Publishes.CreateLink(@event));

                        var deployToEndpoint = default(EventHandler);

                        deployToEndpoint = new EventHandler((s, e) =>
                        {
                            var c = s as IComponent;
                            if (c.InstanceName == selectedEvent + "Sender")
                            {
                                c.DeployTo(endpoint);
                                app.OnInstantiatedComponent -= deployToEndpoint;
                            }
                        });

                        app.OnInstantiatedComponent += deployToEndpoint;
                    }
                    else
                    {
                        component.DeployTo(endpoint);
                    }
                }
            }
        }
    }
}

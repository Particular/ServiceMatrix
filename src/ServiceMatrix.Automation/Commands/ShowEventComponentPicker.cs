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
            private get;
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
            var endpoint = CurrentElement.As<IAbstractEndpoint>();

            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var app = CurrentElement.Root.As<IApplication>();

            // Get available events
            var elements = app.Design.Services.Service
                .Select(s => 
                    Tuple.Create(
                        s.InstanceName,
                        (ICollection<string>)s.Contract.Events.Event.Select(x => x.InstanceName).OrderBy(c => c).ToList()))
                .OrderBy(t => t.Item1).ToList();

            var viewModel =
                new ElementHierarchyPickerViewModel(elements)
                {
                    MasterName = "Service Name",
                    SlaveName = "Event Name",
                    Title = "Publish Event"
                };

            var picker = WindowFactory.CreateDialog<ElementHierarchyPicker>(viewModel);
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

                        deployToEndpoint = (s, e) =>
                        {
                            var c = s as IComponent;
                            if (c != null && c.InstanceName == selectedEvent + "Sender")
                            {
                                c.DeployTo(endpoint);
                                app.OnInstantiatedComponent -= deployToEndpoint;
                            }
                        };

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

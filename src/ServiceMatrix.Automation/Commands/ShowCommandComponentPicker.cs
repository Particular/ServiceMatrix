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
    public class ShowCommandComponentPicker : NuPattern.Runtime.Command
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

            // Get available commands
            var elements = new Dictionary<string, ICollection<string>>();
            foreach (var service in app.Design.Services.Service)
            {
                elements.Add(service.InstanceName, service.Contract.Commands.Command.Select(x => x.InstanceName).ToList());
            }

            var viewModel = 
                new ElementHierarchyPickerViewModel
                {
                    SlaveName = "Command Name:",
                    Title = "Send Command",
                    Elements = elements,
                };

            var picker = this.WindowFactory.CreateDialog<ElementHierarchyPicker>(viewModel);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    var selectedService = viewModel.SelectedMasterItem;
                    var selectedCommand = viewModel.SelectedSlaveItem;

                    var service = app.Design.Services.Service.FirstOrDefault(x => x.InstanceName == selectedService);
                    if (service == null)
                    {
                        service = app.Design.Services.CreateService(selectedService);
                    }

                    var command = service.Contract.Commands.Command.FirstOrDefault(x => x.InstanceName == selectedCommand);
                    if (command == null)
                    {
                        command = service.Contract.Commands.CreateCommand(selectedCommand);
                    }

                    var component = service.Components.Component.FirstOrDefault(x => x.Publishes.CommandLinks.Any(y => y.CommandReference.Value == command));
                    if (component == null)
                    {
                        var deployToEndpoint = default(EventHandler);

                        deployToEndpoint = new EventHandler((s, e) =>
                        {
                            var c = s as IComponent;
                            if (c.InstanceName == selectedCommand + "Sender")
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

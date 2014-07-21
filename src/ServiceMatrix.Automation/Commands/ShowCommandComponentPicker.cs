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
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var endpoint = CurrentElement.As<IAbstractEndpoint>();

            var app = CurrentElement.Root.As<IApplication>();

            // Get available commands
            var elements = app.Design.Services.Service
                .Select(s =>
                    Tuple.Create(
                        s.InstanceName,
                        (ICollection<string>)s.Contract.Commands.Command.Select(x => x.InstanceName).OrderBy(c => c).ToList()))
                .OrderBy(t => t.Item1).ToList();

            var viewModel = new ElementHierarchyPickerViewModel(elements)
            {
                MasterName = "Service Name",
                SlaveName = "Command Name",
                Title = "Send Command",
            };

            var picker = WindowFactory.CreateDialog<ElementHierarchyPicker>(viewModel);

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

                        deployToEndpoint =
                            (s, e) =>
                            {
                                var c = s as IComponent;
                                if (c != null && c.InstanceName == selectedCommand + "Sender")
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

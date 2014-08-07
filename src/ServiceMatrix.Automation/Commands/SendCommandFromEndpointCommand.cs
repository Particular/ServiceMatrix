using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using AbstractEndpoint.Automation.Dialog;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern;
using NuPattern.Presentation;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands
{
    public class SendCommandFromEndpointCommand : NuPattern.Runtime.Command
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { private get; set; }

        /// <summary>
        /// Gets or sets the Window Factory, used to create a Window Dialog.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IDialogWindowFactory WindowFactory { get; set; }

        public override void Execute()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var app = CurrentElement.Root.As<IApplication>();
            var command = CurrentElement.As<ICommand>();

            var viewModel =
                new SenderEndpointPickerViewModel(
                    app,
                    e => e.EndpointComponents.AbstractComponentLinks
                            .Any(c => c.ComponentReference.Value.Publishes.CommandLinks.Any(cl => cl.CommandReference.Value == command)))
                {
                    Title = "Send from...",
                };


            var picker = WindowFactory.CreateDialog<SenderEndpointPicker>(viewModel);
            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    var service = command.GetParentService();

                    foreach (var endpoint in viewModel.SelectedEndpoints)
                    {
                        var closureEndpoint = endpoint;

                        // create and deploy new publisher command
                        var publisherComponent = service.Components.CreateComponent(command.InstanceName + "Sender", x => x.Publishes.CreateLink(command));
                        var deployToEndpoint = default(EventHandler);
                        deployToEndpoint =
                            (s, e) =>
                            {
                                var c = s as IComponent;
                                if (c != null && c == publisherComponent)
                                {
                                    c.DeployTo(closureEndpoint);
                                    app.OnInstantiatedComponent -= deployToEndpoint;
                                }
                            };
                        app.OnInstantiatedComponent += deployToEndpoint;
                    }
                }
            }
        }
    }
}
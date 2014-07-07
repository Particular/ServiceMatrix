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
        IDialogWindowFactory WindowFactory { get; set; }

        public override void Execute()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var app = CurrentElement.Root.As<IApplication>();

            var viewModel =
                new EndpointPickerViewModel(app.Design.Endpoints.GetAll().Select(ep => ep.InstanceName).ToList())
                {
                    Title = "Send from...",
                    AllowEndpointCreation = false
                };


            var picker = WindowFactory.CreateDialog<EndpointPicker>(viewModel);
            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    var command = CurrentElement.As<ICommand>();
                    var service = command.GetParentService();

                    var endpoint = app.Design.Endpoints.GetAll().FirstOrDefault(e => e.InstanceName == viewModel.SelectedItems.First());

                    // create and deploy new publisher command
                    var publisherComponent = service.Components.CreateComponent(command.InstanceName + "Sender", x => x.Publishes.CreateLink(command));
                    var deployToEndpoint = default(EventHandler);
                    deployToEndpoint =
                        (s, e) =>
                        {
                            var c = s as IComponent;
                            if (c != null && c == publisherComponent)
                            {
                                c.DeployTo(endpoint);
                                app.OnInstantiatedComponent -= deployToEndpoint;
                            }
                        };
                    app.OnInstantiatedComponent += deployToEndpoint;
                }
            }
        }
    }
}
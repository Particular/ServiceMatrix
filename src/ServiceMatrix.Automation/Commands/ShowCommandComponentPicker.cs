using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
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

            var viewModel = new ServiceAndCommandPickerViewModel(app, endpoint);

            var picker = WindowFactory.CreateDialog<ServiceAndCommandPicker>(viewModel);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    var selectedService = viewModel.SelectedService;
                    var selectedCommand = viewModel.SelectedCommand;

                    var service =
                        app.Design.Services.Service.FirstOrDefault(x => x.InstanceName == selectedService)
                        ?? app.Design.Services.CreateService(selectedService);

                    var newCommand = false;
                    var command = service.Contract.Commands.Command.FirstOrDefault(x => x.InstanceName == selectedCommand);
                    if (command == null)
                    {
                        newCommand = true;
                        command = service.Contract.Commands.CreateCommand(selectedCommand);
                    }

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

                    if (newCommand)
                    {
                        if (viewModel.SelectedHandlerComponent == null)
                        {
                            service.Components.CreateComponent(command.InstanceName + "Handler", x => x.Subscribes.CreateLink(command));
                        }
                        else
                        {
                            var handlerComponent = viewModel.SelectedHandlerComponent;
                            handlerComponent.Subscribes.CreateLink(command);

                            if (handlerComponent.ProcessesMultipleMessages)
                            {
                                var sagaRecommendationMessage =
                                    handlerComponent.IsSaga
                                        ? String.Format("Would you like to update your existing saga?")
                                        : String.Format("Convert ‘{0}’ to saga to correlate between commands & events?", handlerComponent.CodeIdentifier);
                                var result = MessageBox.Show(sagaRecommendationMessage, "ServiceMatrix - Saga recommendation", MessageBoxButton.YesNo);
                                if (result == MessageBoxResult.Yes)
                                {
                                    new ShowComponentSagaStarterPicker
                                    {
                                        WindowFactory = WindowFactory,
                                        CurrentElement = handlerComponent
                                    }.Execute();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using System.Windows.Input;
using NServiceBusStudio.Automation.Dialog;
using AbstractEndpoint.Automation.Dialog;
using NServiceBusStudio.Automation.CustomSolutionBuilder;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Show the UseCase Command Picker dialog")]
    [Category("General")]
    [Description("Show the UseCase Command Picker dialog.")]
    [CLSCompliant(false)]
    public class ShowUseCaseCommandPickerCommand : NuPattern.Runtime.Command
    {
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

        [Import(AllowDefault = true)]
        public CustomSolutionBuilder.CustomSolutionBuilder CustomSolutionBuilderInstance { get; set; }

        public override void Execute()
        {
            using (new MouseCursor(Cursors.Arrow))
            {
                var picker = WindowFactory.CreateDialog<UseCaseComponentPicker>();
                var usecase = this.CurrentElement.As<IUseCase>();
                var dialog = picker as UseCaseComponentPicker;
                dialog.Title = "Add Command to Use Case";
                dialog.SetComponentLabel("Command Name:");
                var app = usecase.As<IProductElement>().Root.As<IApplication>();
                dialog.SetServices(app.Design.Services.Service);
                dialog.ServiceItemsFillFunction = svc => svc.Contract.Commands.Command.Select(c => c.InstanceName);
                if (picker.ShowDialog() == true)
                {
                    var svc = app.Design.Services.Service.FirstOrDefault(s => s.InstanceName == dialog.SelectedService);
                    if (svc == null)
                    {
                        svc = app.Design.Services.CreateService(dialog.SelectedService);
                    }
                    var command = svc.Contract.Commands.Command.FirstOrDefault(c => c.InstanceName == dialog.SelectedComponent);
                    if (command == null)
                    {
                        command = svc.Contract.Commands.CreateCommand(dialog.SelectedComponent,
                            cmd => cmd.DoNotAutogenerateComponents = true,
                            true);

                        var processorName = Component.TryGetComponentName(command.InstanceName + "Handler", svc);
                        var processorComponent = svc.Components.CreateComponent(processorName);
                        
                        processorComponent.Subscribe(command);
                        this.CurrentElement.As<IUseCase>().AddRelatedElement(processorComponent.As<IProductElement>());
                    }

                    this.CurrentElement.As<IUseCase>().AddRelatedElement(command.As<IProductElement>());
                }
            }
        }
    }
}

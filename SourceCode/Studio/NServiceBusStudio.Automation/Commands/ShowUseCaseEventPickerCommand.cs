using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Patterning.Runtime;
using System.Windows.Input;
using NServiceBusStudio.Automation.Dialog;
using AbstractEndpoint.Automation.Dialog;
using NServiceBusStudio.Automation.CustomSolutionBuilder;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Show the UseCase Event Picker dialog")]
    [Category("General")]
    [Description("Show the UseCase Event Picker dialog.")]
    [CLSCompliant(false)]
    public class ShowUseCaseEventPickerCommand : FeatureCommand
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
                dialog.Title = "Add Event to Use Case";
                dialog.SetComponentLabel("Event Name:");
                var app = usecase.As<IProductElement>().Root.As<IApplication>();
                dialog.SetServices(app.Design.Services.Service);
                dialog.ServiceItemsFillFunction = svc => svc.Contract.Events.Event.Select(c => c.InstanceName);
                if (picker.ShowDialog() == true)
                {
                    var svc = app.Design.Services.Service.FirstOrDefault(s => s.InstanceName == dialog.SelectedService);
                    if (svc == null)
                    {
                        svc = app.Design.Services.CreateService(dialog.SelectedService);
                    }
                    var @event = svc.Contract.Events.Event.FirstOrDefault(c => c.InstanceName == dialog.SelectedComponent);
                    if (@event == null)
                    {
                        @event = svc.Contract.Events.CreateEvent(dialog.SelectedComponent, null, true);
                        var senderName = Component.TryGetComponentName(@event.InstanceName + "Publisher", svc);
                        var processorComponent = svc.Components.CreateComponent(senderName);
                        
                        processorComponent.Publish(@event);
                        this.CurrentElement.As<IUseCase>().AddRelatedElement(processorComponent.As<IProductElement>());
                    }

                    this.CurrentElement.As<IUseCase>().AddRelatedElement(@event.As<IProductElement>());
                }
            }
        }
    }
}

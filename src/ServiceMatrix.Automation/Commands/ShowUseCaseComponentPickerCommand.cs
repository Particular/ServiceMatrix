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
    using Command = NuPattern.Runtime.Command;
    using CustomSolutionBuilder = NServiceBusStudio.Automation.CustomSolutionBuilder.CustomSolutionBuilder;

    [DisplayName("Show the UseCase Component Picker dialog")]
    [Category("General")]
    [Description("Show the UseCase Component Picker dialog.")]
    [CLSCompliant(false)]
    public class ShowUseCaseComponentPickerCommand : Command
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
        public CustomSolutionBuilder CustomSolutionBuilderInstance { get; set; }

        public override void Execute()
        {
            using (new MouseCursor(Cursors.Arrow))
            {
                var picker = WindowFactory.CreateDialog<UseCaseComponentPicker>();
                var usecase = CurrentElement.As<IUseCase>();
                var dialog = picker as UseCaseComponentPicker;
                var app = usecase.As<IProductElement>().Root.As<IApplication>();
                dialog.SetServices(app.Design.Services.Service);
                dialog.ServiceItemsFillFunction = svc => svc.Components.Component.Select(c => c.InstanceName);
                if (picker.ShowDialog() == true)
                {
                    var svc = app.Design.Services.Service.FirstOrDefault(s => s.InstanceName == dialog.SelectedService);
                    if (svc == null)
                    {
                        svc = app.Design.Services.CreateService(dialog.SelectedService);
                    }
                    var component = svc.Components.Component.FirstOrDefault(c => c.InstanceName == dialog.SelectedComponent);
                    if (component == null)
                    {
                        component = svc.Components.CreateComponent(dialog.SelectedComponent);
                    }

                    CurrentElement.As<IUseCase>().AddRelatedElement(component.As<IProductElement>());
                }
            }
        }
    }
}

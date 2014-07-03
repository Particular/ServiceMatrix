using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using NServiceBusStudio.Automation.Dialog;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.Commands
{
    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    [DisplayName("Show a Service Picker Dialog")]
    [Category("General")]
    [Description("Shows a Service Picker dialog where new services may be created and chosen, and then adds the current event to those services.")]
    [CLSCompliant(false)]
    public class ShowSubscriberPicker : NuPattern.Runtime.Command
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
        public IEvent CurrentElement
        {
            get;
            set;
        }

        /// <summary>
        /// Executes this commmand.
        /// </summary>
        /// <remarks></remarks>
        public override void Execute()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var services = CurrentElement.Parent.Parent.Parent.Parent.Service;
            var existingServiceNames = services.Select(e => e.InstanceName).ToList();

            var viewModel = new ServicePickerViewModel(existingServiceNames);

            var picker = WindowFactory.CreateDialog<ServicePicker>(viewModel);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    foreach (var selectedElement in viewModel.SelectedItems)
                    {
                        var selectedService = services.FirstOrDefault(e => string.Equals(e.InstanceName, selectedElement, StringComparison.InvariantCultureIgnoreCase));
                        if (selectedService == null)
                        {
                            selectedService = CurrentElement.Parent.Parent.Parent.Parent.CreateService(selectedElement);
                        }

                        var component = selectedService.Components.CreateComponent(string.Format("{0}Handler", CurrentElement.InstanceName));
                        component.Subscribes.CreateLink(CurrentElement);
                    }
                }
            }
            // TODO: Implement command automation code
            //	TODO: Use tracer.Warning() to note expected and recoverable errors
            //	TODO: Use tracer.Verbose() to note internal execution logic decisions
            //	TODO: Use tracer.Info() to note key results of execution
            //	TODO: Raise exceptions for all other errors
        }
    }
}

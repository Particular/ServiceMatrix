using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using NServiceBusStudio.Automation.Dialog;
using NServiceBusStudio.Automation.Extensions;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern;
using NuPattern.Presentation;
using NuPattern.Runtime;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio.Automation.Commands
{
    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    [DisplayName("Show an Event Picker Dialog for Publishing")]
    [Category("General")]
    [Description("Shows a dialog where the user can choose or create an event, and adds a publish link to it.")]
    [CLSCompliant(false)]
    public class ShowEventTypePicker : NuPattern.Runtime.Command
    {
        /// <summary>
        /// Gets or sets the Window Factory, used to create a Window Dialog.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        private IDialogWindowFactory WindowFactory { get; set; }

        [Required]
        [Import(AllowDefault = true)]
        private IUriReferenceService UriService { get; set; }

        [Required]
        [Import(AllowDefault = true)]
        private ISolution Solution { get; set; }

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

        public IComponent CurrentComponent
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

            CurrentComponent = CurrentElement.As<IComponent>();
            if (CurrentComponent == null)
            {
                CurrentComponent = CurrentElement.Parent.As<IComponent>();
            }

            var events = CurrentComponent.Parent.Parent.Contract.Events.Event;
            var eventNames = events.Select(e => e.InstanceName).ToList();

            var viewModel = new ElementPickerViewModel(eventNames)
            {
                Title = "Publish Event",
                MasterName = "Event name"
            };

            var picker = WindowFactory.CreateDialog<ElementPicker>(viewModel);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    var selectedElement = viewModel.SelectedItem;
                    var selectedEvent =
                        events.FirstOrDefault(e => string.Equals(e.InstanceName, selectedElement, StringComparison.InvariantCultureIgnoreCase));
                    if (selectedEvent == null)
                    {
                        selectedEvent = CurrentComponent.Parent.Parent.Contract.Events.CreateEvent(selectedElement);
                    }

                    CurrentComponent.Publishes.CreateLink(selectedEvent);

                    // Code Generation Guidance
                    if (CurrentComponent.UnfoldedCustomCode)
                    {
                        var userCode = (UserCodeChangeRequired)WindowFactory.CreateDialog<UserCodeChangeRequired>();
                        userCode.UriService = UriService;
                        userCode.Solution = Solution;
                        userCode.Component = CurrentComponent;
                        userCode.Code = String.Format("var {0} = new {1}.{2}();\r\nBus.Publish({0});",
                            selectedEvent.CodeIdentifier.LowerCaseFirstCharacter(),
                            selectedEvent.Parent.Namespace,
                            selectedEvent.CodeIdentifier);
                        userCode.ShowDialog();
                    }
                }
            }
            // Make initial trace statement for this command
            //tracer.Info(
            //    "Executing ShowElementTypePicker on current element '{0}' with AProperty '{1}'", this.CurrentElement.InstanceName, this.ElementType);

            //	TODO: Use tracer.Warning() to note expected and recoverable errors
            //	TODO: Use tracer.Verbose() to note internal execution logic decisions
            //	TODO: Use tracer.Info() to note key results of execution
            //	TODO: Raise exceptions for all other errors
        }
    }
}

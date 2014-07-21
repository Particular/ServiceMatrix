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
using NuPattern.Diagnostics;
using NuPattern.Presentation;
using NuPattern.Runtime;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio.Automation.Commands
{
    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    [DisplayName("Show an Command Picker Dialog for Sending")]
    [Category("General")]
    [Description("Shows a dialog where the user can choose or create a command, and adds a publish link to it.")]
    [CLSCompliant(false)]
    public class ShowCommandTypePicker : NuPattern.Runtime.Command
    {
        private static readonly ITracer tracer = Tracer.Get<ShowCommandTypePicker>();

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

        private IComponent CurrentComponent
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
            var createSenderComponent = false; // At Component Level, do not create sender

            if (CurrentComponent == null)
            {
                CurrentComponent = CurrentElement.Parent.As<IComponent>();
                createSenderComponent = true;
            }

            var commands = CurrentComponent.Parent.Parent.Contract.Commands.Command;
            var commandNames = commands.Select(e => e.InstanceName).ToList();

            var viewModel = new ElementPickerViewModel(commandNames)
            {
                Title = "Send Command",
                MasterName = "Command name"
            };

            var picker = WindowFactory.CreateDialog<ElementPicker>(viewModel);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    var selectedElement = viewModel.SelectedItem;
                    var selectedCommand =
                        commands.FirstOrDefault(e => string.Equals(e.InstanceName, selectedElement, StringComparison.InvariantCultureIgnoreCase));
                    if (selectedCommand == null)
                    {
                        selectedCommand = 
                            CurrentComponent.Parent.Parent.Contract.Commands.CreateCommand(selectedElement, c => c.DoNotAutogenerateSenderComponent = !createSenderComponent);
                    }

                    CurrentComponent.Publishes.CreateLink(selectedCommand);

                    // Code Generation Guidance
                    if (CurrentComponent.UnfoldedCustomCode)
                    {
                        var userCode = (UserCodeChangeRequired)WindowFactory.CreateDialog<UserCodeChangeRequired>();
                        userCode.UriService = UriService;
                        userCode.Solution = Solution;
                        userCode.Component = CurrentComponent;
                        userCode.Code = String.Format("var {0} = new {1}.{2}();\r\nBus.Send({0});",
                            selectedCommand.CodeIdentifier.LowerCaseFirstCharacter(),
                            selectedCommand.Parent.Namespace,
                            selectedCommand.CodeIdentifier);

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

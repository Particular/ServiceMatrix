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

        /// <summary>
        /// Executes this commmand.
        /// </summary>
        /// <remarks></remarks>
        public override void Execute()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var currentComponent = CurrentElement.As<IComponent>();
            var service = currentComponent.Parent.Parent;

            var viewModel = new ServiceAndCommandPickerViewModel(service);

            var picker = WindowFactory.CreateDialog<ServiceAndCommandPicker>(viewModel);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    var selectedCommand = viewModel.SelectedCommand;

                    // Figure out if new or existing command
                    var newCommand = false;
                    var command = service.Contract.Commands.Command.FirstOrDefault(x => x.InstanceName == selectedCommand);
                    if (command == null)
                    {
                        newCommand = true;
                        command = service.Contract.Commands.CreateCommand(selectedCommand);
                    }

                    // Link command to current component
                    currentComponent.Publishes.CreateLink(command);

                    // Assign handler if command
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

                            SagaHelper.CheckAndPromptForSagaUpdate(handlerComponent, WindowFactory);
                        }
                    }

                    // Code Generation Guidance
                    if (currentComponent.UnfoldedCustomCode)
                    {
                        var userCode = (UserCodeChangeRequired)WindowFactory.CreateDialog<UserCodeChangeRequired>();
                        userCode.UriService = UriService;
                        userCode.Solution = Solution;
                        userCode.Component = currentComponent;
                        userCode.Code = String.Format("var {0} = new {1}.{2}();\r\nBus.Send({0});",
                            command.CodeIdentifier.LowerCaseFirstCharacter(),
                            command.Parent.Namespace,
                            command.CodeIdentifier);

                        userCode.ShowDialog();
                    }
                }
            }
        }
    }
}
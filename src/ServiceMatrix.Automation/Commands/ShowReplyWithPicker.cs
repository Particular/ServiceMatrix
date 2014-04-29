namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using NuPattern;
    using NuPattern.Runtime;
    using NServiceBusStudio.Automation.Dialog;
    using NuPattern.Diagnostics;
    using System.Windows;
    using NuPattern.VisualStudio.Solution;
    using Command = NuPattern.Runtime.Command;

    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    [DisplayName("Show a Reply With Picker Dialog for replying a Command")]
    [Category("General")]
    [Description("Shows a dialog where the user can choose or create a message, and use it as an answer.")]
    [CLSCompliant(false)]
    public class ShowReplyWithPicker : Command
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

        private NServiceBusStudio.IComponent CurrentComponent
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
            CurrentComponent = CurrentElement.As<NServiceBusStudio.IComponent>();
            if (CurrentComponent == null)
            {
                CurrentComponent = CurrentElement.Parent.As<NServiceBusStudio.IComponent>();
            }

            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            IProcessedCommandLink processedCommandLink;

            if (CurrentComponent.Subscribes.ProcessedCommandLinks.Count() > 1)
            {
                // TODO: Show picker for Command selection
                processedCommandLink = CurrentComponent.Subscribes.ProcessedCommandLinks.First(x => x.ProcessedCommandLinkReply == null);
            }
            else
            {
                processedCommandLink = CurrentComponent.Subscribes.ProcessedCommandLinks.First(x => x.ProcessedCommandLinkReply == null);
            }

            if (processedCommandLink.ProcessedCommandLinkReply == null)
            {
                // Create Message used for Response
                var service = CurrentComponent.Parent.Parent;
                var message = service.Contract.Messages.CreateMessage (processedCommandLink.CommandReference.Value.CodeIdentifier + "Response");

                // Set Message as ReplyWith for the ProcessedCommandLink
                processedCommandLink.CreateProcessedCommandLinkReply(message.InstanceName, r => r.MessageReference.Value = message);

                // Set Message as HandleMessage for the SenderComponent
                var senderComponent = CurrentComponent.Parent.Component.FirstOrDefault(c => c.Publishes.CommandLinks.Any(cl => cl.CommandReference.Value == processedCommandLink.CommandReference.Value));
                if (senderComponent != null)
                {
                    senderComponent.Subscribes.CreateHandledMessageLink(message.InstanceName, h => h.MessageReference.Value = message);

                    if (senderComponent.Subscribes.ProcessedCommandLinks.Any() ||
                        senderComponent.Subscribes.SubscribedEventLinks.Any())
                    {
                        var result = MessageBox.Show(String.Format("Convert ‘{0}’ to saga to correlate between request and response?", senderComponent.CodeIdentifier), "ServiceMatrix - Saga recommendation", MessageBoxButton.OKCancel);
                        if (result == MessageBoxResult.OK)
                        {
                            new ShowComponentSagaStarterPicker()
                            {
                                WindowFactory = WindowFactory,
                                CurrentElement = senderComponent
                            }.Execute();
                        }
                    }

                    // Code Generation Guidance
                    if (CurrentComponent.UnfoldedCustomCode)
                    {
                        var userCode = WindowFactory.CreateDialog<UserCodeChangeRequired>() as UserCodeChangeRequired;
                        userCode.UriService = UriService;
                        userCode.Solution = Solution;
                        userCode.Component = CurrentComponent;
                        userCode.Code = String.Format("var response = new {1}.{0}();\r\nBus.Reply(response);", message.CodeIdentifier, message.Parent.Namespace);

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

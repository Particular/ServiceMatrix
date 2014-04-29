namespace NServiceBusStudio.Automation.Commands
{
    using NuPattern.Runtime;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Command = NuPattern.Runtime.Command;

    public class ConditionalProcessorComponentCommand : Command
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Command to execute")]
        [Description("This command will be executed if the property value matches the expected value.")]
        public string CommandName { get; set; }

        public override void Execute()
        {
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);

            var component = CurrentElement.As<NServiceBusStudio.IComponent>();

            if (component.IsProcessor)
            {
                var command = CurrentElement.AutomationExtensions.First(a => a.Name == CommandName);
                command.Execute();
            }
        }
    }
}

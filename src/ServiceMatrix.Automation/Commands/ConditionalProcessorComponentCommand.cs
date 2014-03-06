using NuPattern.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Commands
{
    public class ConditionalProcessorComponentCommand : NuPattern.Runtime.Command
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

            var component = this.CurrentElement.As<IComponent>();

            if (component.IsProcessor)
            {
                var command = this.CurrentElement.AutomationExtensions.First(a => a.Name == this.CommandName);
                command.Execute();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Execute Command If Boolean Property Has Expected Value")]
    [Category("General")]
    [Description("Execute Command If Boolean Property Has Expected Value.")]
    [CLSCompliant(false)]
    public class ConditionalCommand : NuPattern.Runtime.Command
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Expected Value For Property")]
        [Description("The Property value should match this expected boolean value to execute the command.")]
        public Boolean ExpectedValue { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Property Name to match")]
        [Description("The value of this property should match the expected value to execute the command and should be boolean.")]
        public string PropertyName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Command to execute")]
        [Description("This command will be executed if the property value matches the expected value.")]
        public string CommandName { get; set; }

        public override void Execute()
        {
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);

            bool currentValue = (bool)(this.CurrentElement.Properties.FirstOrDefault(p => p.DefinitionName == this.PropertyName).Value);

            if (currentValue == this.ExpectedValue)
            {
                var command = this.CurrentElement.AutomationExtensions.First(a => a.Name == this.CommandName);
                command.Execute();
            }
        }
    }
}

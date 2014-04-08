using NuPattern.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Conditions
{
    [CLSCompliant(false)]
    [DisplayName("Component Is Command Handler")]
    [Category("General")]
    [Description("True if the component is processing a command.")]
    public class IsComponentCommandProcessorCondition : Condition
    {
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

        public override bool Evaluate()
        {
            var component = Model.Helpers.GetComponentFromLinkedElement(this.CurrentElement);

            if (component != null)
            {
                return (component.Subscribes.ProcessedCommandLinks.Any(x => x.ProcessedCommandLinkReply == null));
            }
            else
            {
                return false;
            }
        }
    }
}

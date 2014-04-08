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
    [DisplayName("SetUnfoldedCustomCode")]
    [Description("SetUnfoldedCustomCode")]
    [CLSCompliant(false)]
    public class SetUnfoldedCustomCode : NuPattern.Runtime.Command
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

        public override void Execute()
        {
            var component = this.CurrentElement.As<IComponent>();
            component.UnfoldedCustomCode = true;
        }
    
    }
}

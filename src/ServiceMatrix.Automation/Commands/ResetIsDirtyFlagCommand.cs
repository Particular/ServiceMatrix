using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands
{
    public class ResetIsDirtyFlagCommand : NuPattern.Runtime.Command
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
            CurrentElement.As<IApplication>().IsDirty = false;
        }
    }
}

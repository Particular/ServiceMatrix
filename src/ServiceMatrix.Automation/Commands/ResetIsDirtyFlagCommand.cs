namespace NServiceBusStudio.Automation.Commands
{
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using NuPattern.Runtime;
    using Command = NuPattern.Runtime.Command;

    public class ResetIsDirtyFlagCommand : Command
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

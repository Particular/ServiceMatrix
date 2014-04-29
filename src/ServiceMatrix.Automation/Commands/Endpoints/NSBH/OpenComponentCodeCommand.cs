namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("NService Bus Host OpenComponentCodeCommand")]
    [Description("NService Bus Host OpenComponentCodeCommand")]
    [CLSCompliant(false)]
    public class OpenComponentCodeCommand : Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            var componentLink = CurrentElement.As<INServiceBusHostComponentLink>();

            var automation = componentLink.ComponentReference.Value.As<IProductElement>().AutomationExtensions.FirstOrDefault(a => a.Name == "OpenCode");
            if (automation != null)
            {
                automation.Execute();
            }
            
        }
    }
}

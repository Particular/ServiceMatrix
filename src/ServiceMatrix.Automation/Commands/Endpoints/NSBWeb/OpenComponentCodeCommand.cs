namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBWeb
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("WebEndpoint OpenComponentCodeCommand")]
    [Description("WebEndpoint OpenComponentCodeCommand")]
    [CLSCompliant(false)]
    public class OpenComponentCodeCommand : Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            var componentLink = CurrentElement.As<INServiceBusWebComponentLink>();

            var automation = componentLink.ComponentReference.Value.As<IProductElement>().AutomationExtensions.FirstOrDefault(a => a.Name == "OpenCode");
            if (automation != null)
            {
                automation.Execute();
            }

        }
    }
}

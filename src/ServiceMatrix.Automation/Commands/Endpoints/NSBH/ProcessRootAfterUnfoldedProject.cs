namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Process After Unfold On Root")]
    [Description("Process After Unfold On Root Application")]
    [CLSCompliant(false)]
    public class ProcessRootAfterUnfoldedProject : Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            var automation = CurrentElement.Root.AutomationExtensions.First(x => x.Name == "SetStartUpProjects");
            automation.Execute();
            automation = CurrentElement.Root.AutomationExtensions.First(x => x.Name == "CollapseFolders");
            automation.Execute();
        }
    }
}

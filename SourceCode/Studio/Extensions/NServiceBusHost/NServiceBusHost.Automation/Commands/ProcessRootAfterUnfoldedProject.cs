using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;

namespace NServiceBusHost.Automation.Commands
{
    [DisplayName("Process After Unfold On Root")]
    [Description("Process After Unfold On Root Application")]
    [CLSCompliant(false)]
    public class ProcessRootAfterUnfoldedProject : FeatureCommand
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            var automation = this.CurrentElement.Root.AutomationExtensions.First(x => x.Name == "SetStartUpProjects");
            automation.Execute();
            automation = this.CurrentElement.Root.AutomationExtensions.First(x => x.Name == "CollapseFolders");
            automation.Execute();
        }
    }
}

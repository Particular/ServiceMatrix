using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NServiceBusStudio.Automation.Commands
{
    /// <summary>
    /// Do Nothing command (just useful as placeholder)
    /// </summary>
    [DisplayName("Do Nothing")]
    [Description("Do Nothing command (just useful as placeholder)")]
    [CLSCompliant(false)]
    public class DoNothingCommand : NuPattern.Runtime.Command
    {
        public override void Execute()
        {
        }
    }
}

namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel;
    using NuPattern.Runtime;

    /// <summary>
    /// Do Nothing command (just useful as placeholder)
    /// </summary>
    [DisplayName("Do Nothing")]
    [Description("Do Nothing command (just useful as placeholder)")]
    [CLSCompliant(false)]
    public class DoNothingCommand : Command
    {
        public override void Execute()
        {
        }
    }
}

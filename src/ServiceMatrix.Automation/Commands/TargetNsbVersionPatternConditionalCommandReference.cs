using System;

namespace NServiceBusStudio.Automation.Commands
{
    /// <summary>
    /// Command reference element to be shown int he command reference editor
    /// </summary>
    public class TargetNsbVersionPatternConditionalCommandReference
    {
        public Guid CommandId { get; set; }

        public string NsbVersionPattern { get; set; }
    }
}
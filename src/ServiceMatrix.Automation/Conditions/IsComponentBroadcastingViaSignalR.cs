namespace NServiceBusStudio.Automation.Conditions
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using NuPattern.Runtime;

    [CLSCompliant(false)]
    [DisplayName("Is Component Broadcasting via SignalR")]
    [Category("ServiceMatrix")]
    [Description("Matches the component's IsBroadcastingViaSignalR property to the provided value")]

    public class IsComponentBroadcastingViaSignalR : Condition
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        
        [Required]
        [DisplayName(@"IsBroadcastingViaSignalR State on Component Must Be")]
        public bool IsBroadcastingViaSignalR { get; set; }


        /// <summary>
        ///Matches the current state of the component's signalr integration with that of the provided value
        /// </summary>
        /// <returns>true if values match, false otherwise</returns>
        public override bool Evaluate()
        {
            var component = CurrentElement.As<NServiceBusStudio.IComponent>();
            return component.IsBroadcastingViaSignalR == IsBroadcastingViaSignalR;
        }
    }
}

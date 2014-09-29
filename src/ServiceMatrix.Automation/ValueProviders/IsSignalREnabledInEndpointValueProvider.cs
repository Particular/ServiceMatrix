namespace NServiceBusStudio.Automation.ValueProviders
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using NuPattern.Runtime;

    [CLSCompliant(false)]
    [DisplayName("Endpoint Has Components Broadcasting via SignalR")]
    [Category("ServiceMatrix")]
    [Description("Endpoint Has Components Broadcasting via SignalR")]
  
    public class IsSignalREnabledInEndpointValueProvider : ValueProvider
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override object Evaluate()
        {
            var nserviceBusMVC = CurrentElement.As<INServiceBusMVC>();
            return nserviceBusMVC.NServiceBusMVCComponents.NServiceBusMVCComponentLinks.Any(m => m.ComponentReference.Value != null && m.ComponentReference.Value.IsBroadcastingViaSignalR);
        }
    }
}

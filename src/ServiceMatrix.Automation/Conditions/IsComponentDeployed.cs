namespace NServiceBusStudio.Automation.Conditions
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using NuPattern.Runtime;

    [CLSCompliant(false)]
    [DisplayName("Check to see if the component is deployed into an endpoint or not.")]
    [Category("ServiceMatrix")]
    [Description("Matches the component's IsDeployed property to the provided value")]
    public class IsComponentDeployed : Condition
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }


        [Required]
        [DisplayName(@"IsDeployed State on Component Must Be")]
        public bool IsDeployed { get; set; }


        /// <summary>
        ///Matches the current state of the component's signalr integration with that of the provided value
        /// </summary>
        /// <returns>true if values match, false otherwise</returns>
        public override bool Evaluate()
        {
            var app = CurrentElement.Root.As<IApplication>();
            var component = CurrentElement.As<NServiceBusStudio.IComponent>();

            return IsDeployed == app.Design.Endpoints.GetAll()
                  .Any(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component));
        }
    }
}

namespace NServiceBusStudio.Automation.Conditions
{
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using NuPattern.Runtime;
    using System;
    using System.ComponentModel;

    [CLSCompliant(false)]
    [DisplayName("Endpoint Is MVC Host")]
    [Category("General")]
    [Description("True if the this component is hosted inside a MVC endpoint")]
    public class IsMVCEndpoint : Condition
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }

        /// <summary>
        /// Checks to see if this current component is hosted within an MVC endpoint.
        /// </summary>
        /// <returns>True if the endpoint where this handler is hosted is an MVC endpoint</returns>
        public override bool Evaluate()
        {
            var mvcEndpoint = Model.Helpers.GetMvcEndpointFromLinkedElement(CurrentElement);
            if (mvcEndpoint != null)
            {
                return true;
            }
            return false;
        }
    }
}

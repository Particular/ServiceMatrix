namespace NServiceBusStudio.Automation.Conditions
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;

    [CLSCompliant(false)]
    [DisplayName("Component Is Not Deployed into an Endpoint")]
    [Category("General")]
    [Description("True if the component is not deployed into an Endpoint.")]
    public class IsNotComponentDeployedCondition : Condition
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

        public override bool Evaluate()
        {
            var app = CurrentElement.Root.As<IApplication>();
            var component = CurrentElement.As<IComponent>();

            return !app.Design.Endpoints.GetAll()
                    .Any(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component));
        }
    }
}


namespace NServiceBusStudio.Automation.Conditions
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;

    [CLSCompliant(false)]
    [DisplayName("Component Is Defined as Saga")]
    [Category("General")]
    [Description("True if the component is already defined as a Saga.")]
    public class IsSagaComponentCondition : Condition
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
            var component = CurrentElement.As<NServiceBusStudio.IComponent>();
            return component.IsSaga;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio.Automation.Conditions
{
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
            var component = this.CurrentElement.As<IComponent>();
            return component.IsSaga;
        }
    }
}


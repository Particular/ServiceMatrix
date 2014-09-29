using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NServiceBusStudio.Automation.Extensions;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.ValueProviders.Endpoints
{
    [CLSCompliant(false)]
    [DisplayName("Gets Message Conventions")]
    [Category("General")]
    [Description("Returns Message Conventions class body starting from 'namespace'.")]
    public class MessageConventionsProvider : ValueProvider
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

        public override object Evaluate()
        {
            return CurrentElement.GetMessageConventions();
        }
    }
}

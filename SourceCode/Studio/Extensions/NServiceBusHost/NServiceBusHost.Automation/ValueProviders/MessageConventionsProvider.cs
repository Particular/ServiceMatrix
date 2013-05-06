using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using NServiceBusStudio.Automation.Extensions;

namespace NServiceBusHost.Automation.ValueProviders
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
            return (this.CurrentElement as IProduct).GetMessageConventions();
        }
    }
}

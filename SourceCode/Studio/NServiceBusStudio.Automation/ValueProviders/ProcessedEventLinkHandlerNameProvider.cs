using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace NServiceBusStudio.Automation.ValueProviders
{
    [CLSCompliant(false)]
    [DisplayName("ProcessedEventLink Handler Name Provider")]
    [Category("General")]
    [Description("ProcessedEventLink Handler Name Provider")]
    public class ProcessedEventLinkHandlerNameProvider : ValueProvider
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
            var ieventLink = this.CurrentElement.As<ISubscribedEventLink>();
            return ieventLink.EventReference.Value == null ? ieventLink.Parent.Parent.CodeIdentifier + ".cs" : ieventLink.EventReference.Value.CodeIdentifier + "Handler.cs";
        }
    }
}

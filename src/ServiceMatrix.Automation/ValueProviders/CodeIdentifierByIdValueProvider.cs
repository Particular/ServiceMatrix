using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.ValueProviders
{
    [CLSCompliant(false)]
    [Category("General")]
    [Description("Returns a valid CodeIdentifier based on the element referenced.")]
    public class CodeIdentifierByIdValueProvider : ValueProvider
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
            return null;
        }
    }

    [DisplayName("CodeIdentifier By CommandId")]
    public class CodeIdentifierByCommandId : CodeIdentifierByIdValueProvider
    {
        public override object Evaluate()
        {
            var icommandLink = CurrentElement.As<IProcessedCommandLink>();
            return icommandLink.CommandReference.Value.CodeIdentifier;
        }
    }

    [DisplayName("CodeIdentifier By EventId")]
    public class CodeIdentifierByEventId : CodeIdentifierByIdValueProvider
    {
        public override object Evaluate()
        {
            var ieventLink = CurrentElement.As<ISubscribedEventLink>();
            return ieventLink.EventReference.Value == null? ieventLink.Parent.Parent.CodeIdentifier : ieventLink.EventReference.Value.CodeIdentifier;
        }
    }
}

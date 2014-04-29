namespace NServiceBusStudio.Automation.ValueProviders
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;

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
            var ieventLink = CurrentElement.As<ISubscribedEventLink>();
            return ieventLink.EventReference.Value == null ? ieventLink.Parent.Parent.CodeIdentifier + ".cs" : ieventLink.EventReference.Value.CodeIdentifier + "Handler.cs";
        }
    }
}

using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.ValueProviders
{
    public class ApplicationTargetNsbVersionValueProvider : ValueProvider
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
            return CurrentElement.Root.As<IApplication>().TargetNsbVersion;
        }
    }
}

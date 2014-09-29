using System;
using System.ComponentModel;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Conditions
{
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A <see cref="Condition"/> that evaluates to true id the two strings compare with given comparison.
    /// </summary>
    [DisplayName(@"Check target NServiceBus version")]
    [Description(@"Matches the application target NServiceBus version to the supplied value")]
    [Category(@"ServiceMatrix")]
    [CLSCompliant(false)]
    public class MatchesApplicationTargetNsbVersion : Condition
    {
        private static readonly ITracer tracer = Tracer.Get<MatchesApplicationTargetNsbVersion>();

        /// <summary>
        /// Creates a new instance of the <see cref="MatchesApplicationTargetNsbVersion"/> class.
        /// </summary>
        public MatchesApplicationTargetNsbVersion()
        {
            TargetNsbVersion = string.Empty;
        }

        /// <summary>
        /// Gets the target version number to compare.
        /// </summary>
        [Required]
        [DisplayName(@"Target NServiceBus version")]
        public string TargetNsbVersion { get; set; }

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
        /// Evaluates the result of the comparison.
        /// </summary>
        /// <returns></returns>
        public override bool Evaluate()
        {
            this.ValidateObject();

            var application = CurrentElement.Root.As<IApplication>();

            var result = string.Equals(TargetNsbVersion, application.TargetNsbVersion, StringComparison.Ordinal);

            tracer.Info(
                "Determined comparison of target NSB version between {0} and {1} as {2}", TargetNsbVersion, application.TargetNsbVersion, result);

            return result;
        }
    }
}

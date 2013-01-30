using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Design;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using NuPattern.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.Diagnostics;
using NuPattern.Library.Automation;

namespace NServiceBusStudio.Automation.TypeConverters
{
    /// <summary>
    /// A custom type converter for returning a list of System.String values.
    /// </summary>
    [DisplayName("ElementInfoConverter Custom Enumeration Type Converter")]
    [Category("General")]
    [Description("Returns a list of custom enumerations.")]
    [CLSCompliant(false)]
    public class ElementInfoConverter : StringConverter
    {
        private static readonly ITraceSource tracer = Tracer.GetSourceFor<ElementInfoConverter>();

        /// <summary>
        /// Determines whether this converter supports standard values.
        /// </summary>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Determines whether this converter only allows selection of items returned by <see cref="GetStandardValues"/>.
        /// </summary>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <summary>
        /// Returns the list of string values for the enumeration
        /// </summary>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            try
            {
                // Make initial trace statement for this converter
                tracer.TraceInformation(
                    "Determining values for this converter");

                var owner = context.Instance.As<CommandSettings>();

                var product = owner.Store.DefaultPartition.ElementDirectory.AllElements.OfType<IPatternInfo>().First();
                var elements = owner.Store.DefaultPartition.ElementDirectory.AllElements
                    .OfType<IElementInfo>()
                    .Select(e => e.Name)
                    .ToList();

                //	TODO: Use tracer.TraceWarning() to note expected and recoverable errors
                //	TODO: Use tracer.TraceVerbose() to note internal execution logic decisions
                //	TODO: Use tracer.TraceInformation() to note key results of execution
                //	TODO: Raise exceptions for all other errors

                return new StandardValuesCollection(elements);
            }
            catch (Exception ex)
            {
                // TODO: Only catch expected exceptions, and trace them before re-throwing.
                // TODO: Remove this 'catch' if no expections are expected
                tracer.TraceError(
                    ex, "Some error calculating or fetching values");

                throw;
            }
        }
    }
}

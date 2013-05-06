using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


using NuPattern.Runtime;

using System.Diagnostics;
using NuPattern.Library.Automation;
using NuPattern.Diagnostics;

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
        private static readonly ITracer tracer = Tracer.Get<ElementInfoConverter>();

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
                tracer.Info(
                    "Determining values for this converter");

                var owner = context.Instance.As<ICommandSettings>();

                var product = owner.Store.DefaultPartition.ElementDirectory.AllElements.OfType<IPatternInfo>().First();
                var elements = owner.Store.DefaultPartition.ElementDirectory.AllElements
                    .OfType<IElementInfo>()
                    .Select(e => e.Name)
                    .ToList();

                //	TODO: Use tracer.Warning() to note expected and recoverable errors
                //	TODO: Use tracer.Verbose() to note internal execution logic decisions
                //	TODO: Use tracer.Info() to note key results of execution
                //	TODO: Raise exceptions for all other errors

                return new StandardValuesCollection(elements);
            }
            catch (Exception ex)
            {
                // TODO: Only catch expected exceptions, and trace them before re-throwing.
                // TODO: Remove this 'catch' if no expections are expected
                tracer.Error(
                    ex, "Some error calculating or fetching values");

                throw;
            }
        }
    }
}

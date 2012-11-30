using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Design;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;

namespace NServiceBusStudio.Automation.TypeConverters
{
    /// <summary>
    /// A custom type converter for returning Transport values.
    /// </summary>
    [DisplayName("Transport Type Converter")]
    [Category("General")]
    [Description("Returns a list of custom enumerations.")]
    [CLSCompliant(false)]
    public class TransportTypeConverter : StringConverter
    {
        private static readonly ITraceSource tracer = Tracer.GetSourceFor<TransportTypeConverter>();

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
            return true;
        }

        /// <summary>
        /// Returns the list of string values for the enumeration
        /// </summary>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
			var items = new List<string>();

			try
			{
				// Make initial trace statement for this converter
				tracer.TraceInformation(
					"Determining values for this converter");

                foreach (var item in Enum.GetValues(typeof(TransportType)))
                {
                    items.Add(item.ToString());
                }

				return new StandardValuesCollection(items);
			}
			catch (Exception ex)
			{
				tracer.TraceError(
					ex, "Some error calculating or fetching values");

				throw;
			}
        }
    }
}

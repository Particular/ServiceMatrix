namespace NServiceBusStudio.Automation.TypeConverters
{
    using System;
    using System.ComponentModel;
    using NServiceBusStudio.Automation.Model;

    /// <summary>
    /// A custom type converter for returning major versions of NServiceBus.
    /// </summary>
    [DisplayName("Target NSB Version Type Converter")]
    [Category("General")]
    [Description("Returns a list of major versions of NServiceBus")]
    [CLSCompliant(false)]
    public class TargetNsbVersionTypeConverter : StringConverter
    {
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
            return new StandardValuesCollection(new[]
            {
                TargetNsbVersion.Version5,
                TargetNsbVersion.Version4
            });
        }
    }
}

using System;
using System.ComponentModel;
using System.Globalization;
using NServiceBusStudio.Automation.Properties;

namespace NServiceBusStudio.Automation.TypeConverters
{
    public class InvariantCultureTimeSpanStringConverter : StringConverter
    {
        internal static readonly InvariantCultureTimeSpanStringConverter Instance = new InvariantCultureTimeSpanStringConverter();

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;
            if (stringValue != null)
            {
                var input = stringValue.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    return value;
                }

                // Try to parse. Use the invariant culture instead of the current one.
                TimeSpan timeSpan;
                if (TimeSpan.TryParse(input, CultureInfo.InvariantCulture, out timeSpan))
                {
                    return value;
                }

                var exception = new FormatException(string.Format(CultureInfo.CurrentCulture, Resources.TimeSpanConverter_InvalidString, stringValue));
                throw exception;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
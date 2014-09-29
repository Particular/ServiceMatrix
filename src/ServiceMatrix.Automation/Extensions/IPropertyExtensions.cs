namespace NServiceBusStudio.Automation.Extensions
{
    using NuPattern.Runtime;

    public static class IPropertyExtensions
    {
        /// <summary>
        /// Attempt to get the property value as a string
        /// </summary>
        /// <param name="property"></param>
        /// <param name="defaultValue">return this default if convert fails</param>
        
        public static string TryConvertValueToString(this IProperty property, string defaultValue)
        {
            try
            {
                return property.Value.ToString();
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}

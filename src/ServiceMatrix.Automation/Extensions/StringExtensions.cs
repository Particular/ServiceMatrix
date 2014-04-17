namespace NServiceBusStudio.Automation.Extensions
{
    public static class StringExtensions
    {
        public static string LowerCaseFirstCharacter(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;
            if (inputString.Length == 1) return inputString.ToLower();

            return string.Concat(char.ToLowerInvariant(inputString[0]), inputString.Substring(1));
        }
    }
}

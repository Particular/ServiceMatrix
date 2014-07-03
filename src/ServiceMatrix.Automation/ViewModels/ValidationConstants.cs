namespace NServiceBusStudio.Automation.ViewModels
{
    static class ValidationConstants
    {
        /// <summary>
        /// Regex for C# identifier.
        /// </summary>
        public const string IdentifierPattern = 
            @"^[\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}][\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*$";

        /// <summary>
        /// Regex for compound C# identifier (e.g namespace)
        /// </summary>
        public const string CompoundIdentifierPattern =
            @"^[\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}][\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*(?:\.[\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}][\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*)*$";

        public const int IdentifierMinLength = 1;
        public const int IdentifierMaxLength = 30;
        public const int CompoundIdentifierMaxLength = 50;
    }
}
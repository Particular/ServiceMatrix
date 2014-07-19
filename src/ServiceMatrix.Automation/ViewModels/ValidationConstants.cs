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
        public const string InvalidIdentifierMessage = "The value entered is not properly formatted. Accepted values are valid class identifier names. As a general guideline use active verbs for Commands, for example, SubmitOrder. For Events, use passive tense for Events, for example, OrderSubmitted.";
        public const string InvalidCompoundIdentifierMessage = "The value entered is not properly formatted. Accepted values are valid class identifer names but can be separated by a '.', for example, Shipping.PerishableGoods";
    }
}
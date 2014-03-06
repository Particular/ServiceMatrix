namespace Rhino.Licensing
{
    using System;
    using System.Runtime.CompilerServices;

    public class StringLicenseValidator : AbstractLicenseValidator
    {
        public StringLicenseValidator(string publicKey, string license)
            : base(publicKey)
        {
            this.License = license;
        }

        protected override string License { get; set; }
    }
}

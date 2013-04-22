namespace Rhino.Licensing
{
    using System;
    using System.IO;

    public class LicenseValidator : AbstractLicenseValidator
    {
        private string inMemoryLicense;
        private readonly string licensePath;

        public LicenseValidator(string publicKey, string licensePath)
            : base(publicKey)
        {
            this.licensePath = licensePath;
        }

        public override void AssertValidLicense()
        {
            if (!File.Exists(this.licensePath))
            {
                Log.TraceWarning("Could not find license file: {0}", new object[] { this.licensePath });
                throw new LicenseFileNotFoundException();
            }
            base.AssertValidLicense();
        }

        protected override string License
        {
            get
            {
                if (this.inMemoryLicense == null)
                {
                }
                return File.ReadAllText(this.licensePath);
            }
            set
            {
                try
                {
                    File.WriteAllText(this.licensePath, value);
                }
                catch (Exception exception)
                {
                    this.inMemoryLicense = value;
                    Log.TraceWarning("Could not write new license value, using in memory model instead", exception);
                }
            }
        }
    }
}

namespace Rhino.Licensing
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class LicenseFileNotFoundException : RhinoLicensingException
    {
        public LicenseFileNotFoundException()
        {
        }

        public LicenseFileNotFoundException(string message)
            : base(message)
        {
        }

        protected LicenseFileNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public LicenseFileNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

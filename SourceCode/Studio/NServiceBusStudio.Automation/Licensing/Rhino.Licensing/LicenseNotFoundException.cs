namespace Rhino.Licensing
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class LicenseNotFoundException : RhinoLicensingException
    {
        public LicenseNotFoundException()
        {
        }

        public LicenseNotFoundException(string message)
            : base(message)
        {
        }

        protected LicenseNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public LicenseNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

namespace Rhino.Licensing
{
    using System;
    using System.Runtime.Serialization;

    public class LicenseExpiredException : RhinoLicensingException
    {
        public LicenseExpiredException()
        {
        }

        public LicenseExpiredException(string message)
            : base(message)
        {
        }

        public LicenseExpiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public LicenseExpiredException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

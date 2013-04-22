namespace Rhino.Licensing
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class RhinoLicensingException : Exception
    {
        protected RhinoLicensingException()
        {
        }

        protected RhinoLicensingException(string message)
            : base(message)
        {
        }

        protected RhinoLicensingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        protected RhinoLicensingException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

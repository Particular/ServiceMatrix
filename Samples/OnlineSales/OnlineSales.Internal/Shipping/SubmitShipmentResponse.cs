using System;

namespace OnlineSales.Internal.Messages.Shipping
{
    public class SubmitShipmentResponse
    {
        public Guid ReferenceNumber { get; set; }
        public string TrackingNumber { get; set; }

    }
}

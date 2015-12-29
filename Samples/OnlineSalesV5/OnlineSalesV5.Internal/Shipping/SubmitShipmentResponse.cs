using System;

namespace OnlineSalesV5.Internal.Messages.Shipping
{
    public class SubmitShipmentResponse
    {
      public string TrackingNumber { get; set; }
      public Guid ReferenceNumber { get; set; }
    }
}

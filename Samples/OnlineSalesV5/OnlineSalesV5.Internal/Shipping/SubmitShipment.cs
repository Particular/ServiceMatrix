using System;

namespace OnlineSalesV5.Internal.Commands.Shipping
{
    public class SubmitShipment
    {
      public Guid ReferenceNumber { get; set; }
      public string ShippingChoice { get; set; }
    }
}

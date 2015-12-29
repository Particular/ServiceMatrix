using System;

namespace OnlineSales.Internal.Commands.Shipping
{
    public class SubmitShipment
    {
        public Guid ReferenceNumber { get; set; }
        public string ShippingChoice { get; set; }

    }
}

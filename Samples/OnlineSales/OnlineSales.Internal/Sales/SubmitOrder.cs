using System;

namespace OnlineSales.Internal.Commands.Sales
{
    public class SubmitOrder
    {
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string ShippingChoice { get; set; }
    }
}

using System;

namespace OnlineSalesV5.Contracts.Sales
{
    public class OrderAccepted
    {
      public Guid OrderId { get; set; }
      public string CustomerName { get; set; }
      public string AccountNumber { get; set; }
      public decimal Amount { get; set; }
      public string ShippingChoice { get; set; }
    }
}

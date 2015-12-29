using System;
using NServiceBus;
using OnlineSalesV5.Contracts.Sales;
using OnlineSalesV5.Contracts.Billing;
using NServiceBus.Saga;


namespace OnlineSalesV5.Shipping
{
    public partial class OrderAcceptedHandlerSagaData
    {
      public Guid OrderId { get; set; }
      public string ShippingChoice { get; set; }
      public string TraceNumber { get; set; }
    }
}

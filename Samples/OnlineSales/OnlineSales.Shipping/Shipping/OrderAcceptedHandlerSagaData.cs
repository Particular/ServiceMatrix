using System;
using NServiceBus;
using OnlineSales.Contracts.Sales;
using NServiceBus.Saga;


namespace OnlineSales.Shipping
{
    public partial class OrderAcceptedHandlerSagaData
    {
        [Unique]
        public Guid OrderId { get; set; }
        public string TraceNumber { get; set; }
        public string ShippingChoice { get; set; }
    }
}

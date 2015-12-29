using System;
using NServiceBus;
using OnlineSalesV5.Internal.Commands.Billing;
using OnlineSalesV5.Contracts.Sales;
using OnlineSalesV5.Internal.Messages.Billing;
using NServiceBus.Saga;


namespace OnlineSalesV5.Billing
{
    public partial class OrderAcceptedHandlerSagaData
    {
      public Guid OrderId { get; set; }
      public string AuthCode { get; set; }
    }
}

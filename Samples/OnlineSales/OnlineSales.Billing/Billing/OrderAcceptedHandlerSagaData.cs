using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Billing;
using OnlineSales.Contracts.Sales;
using OnlineSales.Internal.Messages.Billing;
using NServiceBus.Saga;


namespace OnlineSales.Billing
{
    public partial class OrderAcceptedHandlerSagaData
    {
       //Since this is all about Billing, this saga will keep the auth code billing details.
       [Unique]
        public Guid OrderId { get; set; }
        public string AuthCode { get; set; }
        
    }
}

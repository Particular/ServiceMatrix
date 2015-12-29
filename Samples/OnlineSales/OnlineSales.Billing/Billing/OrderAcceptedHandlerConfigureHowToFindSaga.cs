using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Billing;
using OnlineSales.Contracts.Sales;
using OnlineSales.Internal.Messages.Billing;
using NServiceBus.Saga;


namespace OnlineSales.Billing
{
    public partial class OrderAcceptedHandler
    {
        public override void ConfigureHowToFindSaga()
        {
            
            //I modified this to map the response referenceID (which I filled with the orderID0 to the order id
            ConfigureMapping<SubmitPaymentResponse>(m => m.ReferenceNumber).ToSaga(s => s.OrderId);
                        
            // If you add new messages to be handled by your saga, you will need to manually add a call to ConfigureMapping for them.
        }
    }
}

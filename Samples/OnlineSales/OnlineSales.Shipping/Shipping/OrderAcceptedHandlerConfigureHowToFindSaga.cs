using System;
using NServiceBus;
using OnlineSales.Contracts.Sales;
using OnlineSales.Contracts.Billing;
using OnlineSales.Internal.Commands.Shipping;
using OnlineSales.Internal.Messages.Shipping;
using NServiceBus.Saga;


namespace OnlineSales.Shipping
{
    public partial class OrderAcceptedHandler
    {
        public override void ConfigureHowToFindSaga()
        {
            
            //I need to specify how to coorelate the events that arrive to this saga since I don't know which will arrive first.
            //NOTE: I don't map the response from the shipping service.  It will be coorelated automatically since it is a response to a request that originated with this saga. 

            ConfigureMapping<OrderAccepted>(m => m.OrderId).ToSaga(s => s.OrderId);
            ConfigureMapping<BillingCompleted>(m => m.OrderId).ToSaga(s => s.OrderId);
         
            // If you add new messages to be handled by your saga, you will need to manually add a call to ConfigureMapping for them.
        }
    }
}

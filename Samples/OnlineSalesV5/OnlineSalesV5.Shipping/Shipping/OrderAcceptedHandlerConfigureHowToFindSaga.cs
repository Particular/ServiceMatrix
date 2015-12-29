using System;
using NServiceBus;
using OnlineSalesV5.Contracts.Sales;
using OnlineSalesV5.Contracts.Billing;
using NServiceBus.Saga;


namespace OnlineSalesV5.Shipping
{
    public partial class OrderAcceptedHandler
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderAcceptedHandlerSagaData> mapper)
        {
            
            mapper.ConfigureMapping<OrderAccepted>(m => m.OrderId).ToSaga(s => s.OrderId);
            mapper.ConfigureMapping<BillingCompleted>(m => m.OrderId).ToSaga(s => s.OrderId);

            
            // If you add new messages to be handled by your saga, you will need to manually add a call to ConfigureMapping for them.
        }
    }
}

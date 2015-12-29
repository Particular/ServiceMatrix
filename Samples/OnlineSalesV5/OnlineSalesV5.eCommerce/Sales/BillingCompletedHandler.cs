using System;
using NServiceBus;
using OnlineSalesV5.Contracts.Billing;


namespace OnlineSalesV5.Sales
{
    public partial class BillingCompletedHandler
    {
		
        partial void HandleImplementation(BillingCompleted message)
        {
            // TODO: BillingCompletedHandler: Add code to handle the BillingCompleted message.
            Console.WriteLine("Sales received " + message.GetType().Name);
        }

    }
}

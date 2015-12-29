using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Sales;


namespace OnlineSales.Sales
{
    public partial class SubmitOrderHandler
    {
		
        partial void HandleImplementation(SubmitOrder message)
        {
            // TODO: SubmitOrderHandler: Add code to handle the SubmitOrder message.
            Console.WriteLine("Sales received  " + message.GetType().Name);
            
            var orderAccepted = new OnlineSales.Contracts.Sales.OrderAccepted();
            
            //This was added by.  We need to assign the order id and translate data
            orderAccepted.OrderId = Guid.NewGuid();
            orderAccepted.CustomerName = message.CustomerName;
            orderAccepted.AccountNumber = message.AccountNumber;
            orderAccepted.Amount = message.Amount;
            orderAccepted.ShippingChoice = message.ShippingChoice;


            Console.WriteLine("Sales Assigned order for customer {0} with amount {1} and order id {2}", message.CustomerName, message.Amount, orderAccepted.OrderId);


            Bus.Publish(orderAccepted);
        }

    }
}

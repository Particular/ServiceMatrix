using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Shipping;
using OnlineSales.Contracts.Sales;
using OnlineSales.Contracts.Billing;
using OnlineSales.Internal.Messages.Shipping;
using NServiceBus.Saga;


namespace OnlineSales.Shipping
{
    public partial class OrderAcceptedHandler
    {
		
        partial void HandleImplementation(OrderAccepted message)
        {
            // TODO: OrderAcceptedHandler: Add code to handle the OrderAccepted message.
            Console.WriteLine("Shipping received OrderAccepted for Order Id {0} for customer {1}", message.OrderId,message.CustomerName);

            Data.OrderId = message.OrderId;
            Data.ShippingChoice = message.ShippingChoice;
            ProcessShipment();
           
        }

        partial void HandleImplementation(BillingCompleted message)
        {
            // TODO: OrderAcceptedHandler: Add code to handle the BillingCompleted message.
            Console.WriteLine("Shipping received " + message.GetType().Name);

            Data.OrderId = message.OrderId;
            ProcessShipment();
           
        }

        
        private void ProcessShipment()
        { //we need to have received both OrderAccepted and Billing Completed before we ship.

            if (Data.BillingCompleted != null && Data.OrderAccepted != null)
            {
                Console.WriteLine("Billing Completed for order {0}.  Processing Shipment.", Data.OrderId);

               var submitShipment = new OnlineSales.Internal.Commands.Shipping.SubmitShipment();
               submitShipment.ReferenceNumber = Data.OrderId;
               submitShipment.ShippingChoice = Data.ShippingChoice;
                Bus.Send(submitShipment);
            
            }


        }
        
        partial void HandleImplementation(SubmitShipmentResponse message)
        {
            Data.TraceNumber = message.TrackingNumber;
            Console.WriteLine("Shipment response received with trace number {0}. Completing Saga",Data.TraceNumber);

            MarkAsComplete();
        }
        
        
        
        partial void AllMessagesReceived()
        {
            //System.Console.WriteLine("All the messages are received");
        }

    }
}

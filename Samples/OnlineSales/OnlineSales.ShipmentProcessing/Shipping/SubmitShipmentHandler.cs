using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Shipping;


namespace OnlineSales.Shipping
{
    public partial class SubmitShipmentHandler
    {
		
        partial void HandleImplementation(SubmitShipment message)
        {
            // TODO: SubmitShipmentHandler: Add code to handle the SubmitShipment message.
            Console.WriteLine("Shipping received {0} with choice {1} for reference number {2}",message.GetType().Name,message.ShippingChoice,message.ReferenceNumber);
                        
            //This is where we'd call a payment webservice or something like that.  Instead we can just parrot a positive outcome. 
            
            var response = new OnlineSales.Internal.Messages.Shipping.SubmitShipmentResponse();
            response.TrackingNumber = "TRACKING NUMBER 123";
            response.ReferenceNumber = message.ReferenceNumber;

            Bus.Reply(response);
        }

    }
}

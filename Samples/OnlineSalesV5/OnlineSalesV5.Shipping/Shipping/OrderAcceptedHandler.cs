using System;
using NServiceBus;
using OnlineSalesV5.Contracts.Sales;
using OnlineSalesV5.Contracts.Billing;
using NServiceBus.Saga;
using OnlineSalesV5.Internal.Messages.Shipping;


namespace OnlineSalesV5.Shipping
{
  public partial class OrderAcceptedHandler
  {

    partial void HandleImplementation(OrderAccepted message)
    {
      // TODO: OrderAcceptedHandler: Add code to handle the OrderAccepted message.
      Console.WriteLine("Shipping received OrderAccepted for Order Id {0} for customer {1}", message.OrderId, message.CustomerName);

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
    {
      //we need to have received both OrderAccepted and Billing Completed before we ship.

      if (Data.BillingCompleted != null && Data.OrderAccepted != null)
      {
        Console.WriteLine("Billing Completed for order {0}.  Processing Shipment.", Data.OrderId);

        var submitShipment = new OnlineSalesV5.Internal.Commands.Shipping.SubmitShipment();
        submitShipment.ReferenceNumber = Data.OrderId;
        submitShipment.ShippingChoice = Data.ShippingChoice;
        Bus.Send(submitShipment);
      }
    }

    partial void HandleImplementation(SubmitShipmentResponse message)
    {
      Data.TraceNumber = message.TrackingNumber;
      Console.WriteLine("Shipment response received with trace number {0}. Completing Saga", Data.TraceNumber);

      MarkAsComplete();
    }

  }
}

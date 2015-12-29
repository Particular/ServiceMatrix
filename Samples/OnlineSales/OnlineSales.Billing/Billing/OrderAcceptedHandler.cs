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
		
        partial void HandleImplementation(OrderAccepted message)
        {
            // TODO: OrderAcceptedHandler: Add code to handle the OrderAccepted message.
            Console.WriteLine("Billing received Order for {0}. Sending Payment Request. ", message.CustomerName);

            //We can save some specific items.
            Data.OrderId = message.OrderId;

            
            var submitPayment = new OnlineSales.Internal.Commands.Billing.SubmitPayment();

            //translate values from the OrderAccepted to the payment request
            submitPayment.Account = message.AccountNumber;
            //use the assigned order ID as our reference number.
            submitPayment.ReferenceNumber = message.OrderId;
            submitPayment.Amount = message.Amount;

            Bus.Send(submitPayment);
        }


        partial void HandleImplementation(SubmitPaymentResponse message)
        {
            Data.AuthCode = message.AuthorizationCode;

            //With the payment response received will close the saga and publish billing complete. 
            var billingCompleted = new OnlineSales.Contracts.Billing.BillingCompleted();
            billingCompleted.OrderId = Data.OrderId;
            Console.WriteLine("Payment response received for order {0}.  Completing Saga and publishing Billing Complete.", Data.OrderId);
                       
            Bus.Publish(billingCompleted);

            MarkAsComplete();

            
        }


    }

    
}

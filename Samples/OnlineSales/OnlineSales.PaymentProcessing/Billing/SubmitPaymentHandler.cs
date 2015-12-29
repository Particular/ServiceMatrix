using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Billing;


namespace OnlineSales.Billing
{
    public partial class SubmitPaymentHandler
    {
		
        partial void HandleImplementation(SubmitPayment message)
        {
            // TODO: SubmitPaymentHandler: Add code to handle the SubmitPayment message.
            Console.WriteLine("Payment Processing received " + message.GetType().Name);

            var response = new OnlineSales.Internal.Messages.Billing.SubmitPaymentResponse();
            response.ReferenceNumber = Guid.NewGuid();
            response.AuthorizationCode = "OK";
            Console.WriteLine("Authorizing Payment for {0} and reference number {1}", message.Amount, message.ReferenceNumber);

            //throw new Exception("Something Bad Happened.");


            Bus.Reply(response);
        }

    }
}

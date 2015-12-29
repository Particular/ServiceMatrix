using System;

namespace OnlineSales.Internal.Messages.Billing
{
    public class SubmitPaymentResponse
    {
        public Guid ReferenceNumber { get; set; }
        public string AuthorizationCode { get; set; }
    }
}

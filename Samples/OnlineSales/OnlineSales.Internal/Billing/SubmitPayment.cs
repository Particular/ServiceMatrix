using System;

namespace OnlineSales.Internal.Commands.Billing
{
    public class SubmitPayment
    {
        public Guid ReferenceNumber { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }

    }
}

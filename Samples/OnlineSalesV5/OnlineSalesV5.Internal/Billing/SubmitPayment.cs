using System;

namespace OnlineSalesV5.Internal.Commands.Billing
{
    public class SubmitPayment
    {
      public string Account { get; set; }
      public Guid ReferenceNumber { get; set; }
      public decimal Amount { get; set; }
    }
}

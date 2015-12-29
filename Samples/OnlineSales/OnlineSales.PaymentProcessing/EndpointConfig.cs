using System;
using NServiceBus;
 
namespace OnlineSales.PaymentProcessing
{
  public partial class EndpointConfig:IWantToRunWhenBusStartsAndStops
  {

      public void Start()
      {
          System.Console.Title = "PaymentProcessing Endpoint";

      }

      public void Stop()
      {
          //throw new NotImplementedException();
      }
  }
}

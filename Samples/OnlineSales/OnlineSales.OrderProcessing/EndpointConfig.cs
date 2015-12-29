using System;
using NServiceBus;
 
namespace OnlineSales.OrderProcessing
{
  public partial class EndpointConfig:IWantToRunWhenBusStartsAndStops
  {

      public void Start()
      {
          Console.Title = "OrderProcessing Endpoint";
      }

      public void Stop()
      {
          //throw new NotImplementedException();
      }
  }
}

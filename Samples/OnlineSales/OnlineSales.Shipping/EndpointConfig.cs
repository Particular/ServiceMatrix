using System;
using NServiceBus;
 
namespace OnlineSales.Shipping
{
  public partial class EndpointConfig    :IWantToRunWhenBusStartsAndStops
  {

      public void Start()
      {
          System.Console.Title = "Shipping Endpoint";
      }

      public void Stop()
      {
          //throw new NotImplementedException();
      }
  }
}

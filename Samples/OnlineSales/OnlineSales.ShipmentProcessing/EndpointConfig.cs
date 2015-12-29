using System;
using NServiceBus;
 
namespace OnlineSales.ShipmentProcessing
{
  public partial class EndpointConfig:IWantToRunWhenBusStartsAndStops

  {

      public void Start()
      {
          System.Console.Title = "ShipmentProcessing Endpoint";
      }

      public void Stop()
      {
          //throw new NotImplementedException();
      }
  }
}

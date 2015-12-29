using System;
using NServiceBus;
 
namespace OnlineSales.Billing
{
  public partial class EndpointConfig:IWantToRunWhenBusStartsAndStops
  {


      public void Start()
      {
          //Demonstrating how to add your own interface implemtations
          System.Console.Title = "Billing Endpoint";
      }

      public void Stop()
      {
         // throw new NotImplementedException();
      }
  }
}

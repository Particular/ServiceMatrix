using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;


namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBMVC
{
    using System.Linq;

    [DisplayName("Execute ModifyRouteConfigForSignalRCommand in MVCEndpoint")]
    [Description("Will invoke ExecuteModifyRouteConfigForSignalR command in the associated MVC Endpoint")]
    [CLSCompliant(false)]
    public class ExecuteModifyRouteConfigForSignalRCommand : NuPattern.Runtime.Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            // Get the associated MVC Endpoint
            var mvcEndpointElement = Model.Helpers.GetMvcEndpointFromLinkedElement(CurrentElement).As<IProductElement>();

            // Find the ModifyRouteConfigForSignalRCommand command to execute
            var commandToExecute = mvcEndpointElement.AutomationExtensions.First(c => c.Name.Equals("ModifyRouteConfigForSignalR"));
            commandToExecute.Execute();

            // Set the Broadcasting via SignalR flag on the component level to true;
            CurrentElement.As<NServiceBusStudio.IComponent>().IsBroadcastingViaSignalR = true; 
        }
    }
}

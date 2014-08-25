using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;


namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBMVC
{
    using System.Linq;

    [DisplayName("Remove all SignalR related references from the MVCEndpoint")]
    [Description("Will invoke RemoveReferencesForSignalR and RemoveHubsFromRouteConfig command in the associated MVC Endpoint")]
    [CLSCompliant(false)]
    public class ExecuteRemoveReferencesForSignalRInEndpointCommand : NuPattern.Runtime.Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            // Get the associated MVC Endpoint
            var mvcEndpointElement = Model.Helpers.GetMvcEndpointFromLinkedElement(CurrentElement).As<IProductElement>();

            // Find the ExecuteRemoveReferencesForSignalR command to execute
            var commandToExecute = mvcEndpointElement.AutomationExtensions.First(c => c.Name.Equals("RemoveReferencesForSignalR"));
            commandToExecute.Execute();
        
        }
    }
}

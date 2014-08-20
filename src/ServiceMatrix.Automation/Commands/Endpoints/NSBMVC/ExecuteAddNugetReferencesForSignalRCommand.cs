using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;


namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBMVC
{
    using System.Linq;

    [DisplayName("Execute AddNugetReferencesForSignalRCommand in MVCEndpoint")]
    [Description("Will invoke AddNugetReferencesForSignalRCommand in the associated MVC Endpoint")]
    [CLSCompliant(false)]
    public class ExecuteAddNugetReferencesForSignalRCommand : NuPattern.Runtime.Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            // Get the associated MVC Endpoint
            var mvcEndpointElement = Model.Helpers.GetMvcEndpointFromLinkedElement(CurrentElement).As<IProductElement>();

            // Find the AddNugetReferencesForSignalRCommand command to execute
            var commandToExecute = mvcEndpointElement.AutomationExtensions.First(c => c.Name.Equals("AddNugetReferencesForSignalRCommand"));
            commandToExecute.Execute();

            // Set the Broadcasting via SignalR flag on the component level to true;
            CurrentElement.As<NServiceBusStudio.IComponent>().IsBroadcastingViaSignalR = true;
         
        }
    }
}

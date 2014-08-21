namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using NServiceBusStudio.Automation.Extensions;
    using NuPattern.Runtime;
    using NuPattern.VisualStudio.Solution;

    public class RemoveSignalRComponentCodeCommand : NuPattern.Runtime.Command
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { private get; set; }

        [Import(AllowDefault = true)]
        public ISolution Solution { get; set; }


        public override void Execute()
        {
            // Get the associated MVC Endpoint
            var mvcEndpoint = Model.Helpers.GetMvcEndpointFromLinkedElement(CurrentElement);
            var component = CurrentElement.As<NServiceBusStudio.IComponent>();
            var references = component.References;
            
            var service = component.Parent.Parent;
            
            // Delete the hub
            var hubPathName = String.Format("{0}\\Infrastructure\\{1}\\{2}Hub.cs", mvcEndpoint.Namespace, service.InstanceName, component.InstanceName);
            var hubItem = Solution.FindItem(hubPathName);
            if (hubItem != null)
            {
                hubItem.Delete();
            }

            // Delete the broadcasting handler
            var broadcastHandlerPathName = String.Format("{0}\\Infrastructure\\{1}\\Broadcast{2}.cs", mvcEndpoint.Namespace, service.InstanceName, component.InstanceName);
            var broadcastHandlerItem = Solution.FindItem(broadcastHandlerPathName);
            if (broadcastHandlerItem != null)
            {
                broadcastHandlerItem.Delete();
            }

            // Set the Broadcasting via SignalR flag on the component level to false;
            CurrentElement.As<NServiceBusStudio.IComponent>().IsBroadcastingViaSignalR = false; 
    
        }
    }
}

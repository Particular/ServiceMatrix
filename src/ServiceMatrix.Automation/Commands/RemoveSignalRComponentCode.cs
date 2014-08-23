namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using NuPattern;
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

        [Required]
        [Import(AllowDefault = true)]
        private IUriReferenceService UriService { get; set; }

        public override void Execute()
        {   
            // TODO: Resolve the commandId automatically instead of passing a Guid here.
            // For example if I obtain the command like below Name and Execute() method. How do I get the command Id??
            // var command = CurrentElement.AutomationExtensions.First(c => c.Name.Equals("GenerateSignalRHub"));
            
            // Delete the hub artifact reference
            DeleteSolutionItemsInComponentForCommand("c9a984c3-c0d6-46f5-a050-6d5be1b736ab");  // --> This is the Guid for the commandId for GenerateSignalRHub command 
            
            // Delete the broadcast Handler artifact
            DeleteSolutionItemsInComponentForCommand("c6563779-ff3e-40aa-99ac-361aa452505e"); // --> This is the Guid for the commandId for GenerateSignalRBroadcastHandler command
            
            // Set the Broadcasting via SignalR flag on the component level to false;
            CurrentElement.As<NServiceBusStudio.IComponent>().IsBroadcastingViaSignalR = false; 
    
        }

        void DeleteSolutionItemsInComponentForCommand(string commandId)
        {
            var component = CurrentElement.As<NServiceBusStudio.IComponent>();
            var artifactLink = component.References.First(t => t.Tag.Contains(commandId));
            var solutionItem = UriService.TryResolveUri<IItemContainer>(new Uri(artifactLink.Value));
            
            // Delete the actual solution item and its reference in the component.
            solutionItem.Delete();
            artifactLink.Delete();
        }
    }
}

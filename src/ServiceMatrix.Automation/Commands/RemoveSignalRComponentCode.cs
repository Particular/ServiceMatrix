namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using NuPattern;
    using NuPattern.Library.Automation;
    using NuPattern.Runtime;
    using NuPattern.VisualStudio.Solution;

    public class RemoveSignalRComponentCodeCommand : Command
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
            var itemsToRemove = new [] {
                "GenerateSignalRHub",  
                "GenerateSignalRBroadcastHandler", 
                "GenerateHighlightCSS", 
                "GenerateHighlightJS", 
                "GenerateGuidanceCSS"
            };

            foreach (var item in itemsToRemove)
            {
                var command = (IAutomationExtension<ICommandSettings>) CurrentElement.AutomationExtensions.FirstOrDefault(c => c.Name.Equals(item, StringComparison.OrdinalIgnoreCase));
                if (command == null)
                {
                    throw new Exception(string.Format("{0} does not map to an existing command", item));
                }
                var commandId = command.Settings.Id.ToString("D");
                DeleteSolutionItemsInComponentForCommand(commandId);
            }

            // Set the Broadcasting via SignalR flag on the component level to false;
            CurrentElement.As<IComponent>().IsBroadcastingViaSignalR = false; 
        }

        void DeleteSolutionItemsInComponentForCommand(string commandId)
        {
            var component = CurrentElement.As<IComponent>();
            var artifactLink = component.References.First(t => t.Tag.Contains(commandId));
            var solutionItem = UriService.TryResolveUri<IItemContainer>(new Uri(artifactLink.Value));
            
            // Delete the actual solution item and its reference in the component.
            solutionItem.Delete();
            artifactLink.Delete();
        }
    }
}

namespace NServiceBusStudio.Automation.Commands
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using Command = NuPattern.Runtime.Command;

    public class CreateLibraryCodeLinkCommand : Command
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }
        
        public override void Execute()
        {
            var lr = CurrentElement.As<ILibraryReference>();
            if (lr != null)
            {
                var component = lr.Parent.Parent;
                if (component.DeployedTo != null)
                {
                    component.EndpointDefined(null);
                }
            }
           
        }
    }
}

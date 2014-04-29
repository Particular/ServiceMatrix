namespace NServiceBusStudio.Automation.Commands
{
    using NuPattern.Runtime;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("SetUnfoldedCustomCode")]
    [Description("SetUnfoldedCustomCode")]
    [CLSCompliant(false)]
    public class SetUnfoldedCustomCode : Command
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }

        public override void Execute()
        {
            var app = CurrentElement.Root.As<IApplication>();
            var component = CurrentElement.As<NServiceBusStudio.IComponent>();

            var isDeployed = app.Design.Endpoints.GetAll()
                    .Any(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component));

            if (!isDeployed)
            {
                return;
            }
            
            component.UnfoldedCustomCode = true;
        }
    
    }
}

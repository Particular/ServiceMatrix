namespace NServiceBusStudio.Automation.Commands
{
    using NuPattern.Runtime;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using Command = NuPattern.Runtime.Command;

    public class RaisesOnInstantiatedComponent : Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            var app = CurrentElement.Root.As<IApplication>();
            app.RaiseOnInstantiatedComponent(CurrentElement.As<IComponent>());
        }
    }
}

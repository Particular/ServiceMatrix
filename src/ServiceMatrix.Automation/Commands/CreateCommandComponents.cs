namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Diagnostics;
    using NuPattern.Runtime;
    using ICommand = NServiceBusStudio.ICommand;

    [DisplayName("Create Command's Components")]
    [Description("Creates the Command's Sender component and Command's handler component.")]
    [CLSCompliant(false)]
    public class CreateCommandComponents : Command
    {
        private static readonly ITracer tracer = Tracer.Get<CreateCommandComponents>();

        [Required]
        [Import(AllowDefault = true)]
        public ICommand Command { get; set; }

        public override void Execute()
        {
            if (!Command.DoNotAutogenerateSenderComponent)
            {
                CreateComponent(String.Format("{0}Sender", Command.InstanceName),
                            c => c.Publishes.CreateLink(Command));
            }
            CreateComponent(String.Format("{0}Handler", Command.InstanceName),
                            c => c.Subscribes.CreateLink(Command));
        }

        private void CreateComponent(string componentName, Action<NServiceBusStudio.IComponent> componentInitializer)
        {
            Command.Parent.Parent.Parent.Components.CreateComponent(componentName, componentInitializer);
        }
    }
}

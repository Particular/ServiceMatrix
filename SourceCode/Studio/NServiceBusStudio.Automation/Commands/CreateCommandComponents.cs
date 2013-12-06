using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Diagnostics;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Create Command's Components")]
    [Description("Creates the Command's Sender component and Command's Processor component.")]
    [CLSCompliant(false)]
    public class CreateCommandComponents : NuPattern.Runtime.Command
    {
        private static readonly ITracer tracer = Tracer.Get<CreateCommandComponents>();

        [Required]
        [Import(AllowDefault = true)]
        public ICommand Command { get; set; }

        public override void Execute()
        {
            CreateComponent(String.Format("{0}Sender", this.Command.InstanceName),
                           (c) => c.Publishes.CreateLink(this.Command));
            CreateComponent(String.Format("{0}Processor", this.Command.InstanceName),
                            (c) => c.Subscribes.CreateLink(this.Command));
        }

        private void CreateComponent(string componentName, Action<IComponent> componentInitializer)
        {
            this.Command.Parent.Parent.Parent.Components.CreateComponent(componentName, componentInitializer);
        }
    }
}

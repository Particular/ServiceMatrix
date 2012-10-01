using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Create Command's Components")]
    [Description("Creates the Command's Sender component and Command's Processor component.")]
    [CLSCompliant(false)]
    public class CreateCommandComponents : FeatureCommand
    {
        private static readonly ITraceSource tracer = Tracer.GetSourceFor<CreateCommandComponents>();

        [Required]
        [Import(AllowDefault = true)]
        public ICommand Command { get; set; }

        public override void Execute()
        {
            CreateComponent(String.Format("{0}Sender", this.Command.InstanceName),
                           (c) => c.Publishes.CreateCommandLink(this.Command.InstanceName, p => p.CommandReference.Value = this.Command));
            CreateComponent(String.Format("{0}Processor", this.Command.InstanceName),
                            (c) => c.Subscribes.CreateProcessedCommandLink(this.Command.InstanceName, p => p.CommandReference.Value = this.Command));
        }

        private void CreateComponent(string componentName, Action<IComponent> componentInitializer)
        {
            this.Command.Parent.Parent.Parent.Components.CreateComponent(componentName, componentInitializer);
        }
    }
}

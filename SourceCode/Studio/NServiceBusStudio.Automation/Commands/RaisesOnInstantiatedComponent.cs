using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using NuPattern.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Commands
{
    public class RaisesOnInstantiatedComponent : FeatureCommand
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            var app = this.CurrentElement.Root.As<IApplication>();
            app.RaiseOnInstantiatedComponent(this.CurrentElement.As<IComponent>());
        }
    }
}

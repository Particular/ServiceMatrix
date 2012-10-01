using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Patterning.Library.Automation;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;

namespace NServiceBusStudio.Automation.Infrastructure
{
    public class InnerCommandAutomation : IAutomationExtension
    {
        FeatureCommand command;

        public InnerCommandAutomation(FeatureCommand command)
        {
            this.command = command;
        }

        public void Execute(IDynamicBindingContext context)
        {
            Execute();
        }

        public void Execute()
        {
            this.command.Execute();
        }

        public string Name { get; set; }
        public IProductElement Owner { get; set; }
    }
}

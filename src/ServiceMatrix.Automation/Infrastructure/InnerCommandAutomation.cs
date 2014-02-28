using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Library.Automation;
using NuPattern.Runtime;


namespace NServiceBusStudio.Automation.Infrastructure
{
    public class InnerCommandAutomation : IAutomationExtension
    {
        NuPattern.Runtime.Command command;

        public InnerCommandAutomation(NuPattern.Runtime.Command command)
        {
            this.command = command;
        }

        public void Execute(NuPattern.Runtime.Bindings.IDynamicBindingContext context)
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

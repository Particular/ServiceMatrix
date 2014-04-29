using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Library.Automation;
using NuPattern.Runtime;


namespace NServiceBusStudio.Automation.Infrastructure
{
    using NuPattern.Runtime.Bindings;
    using Command = NuPattern.Runtime.Command;

    public class InnerCommandAutomation : IAutomationExtension
    {
        Command command;

        public InnerCommandAutomation(Command command)
        {
            this.command = command;
        }

        public void Execute(IDynamicBindingContext context)
        {
            Execute();
        }

        public void Execute()
        {
            command.Execute();
        }

        public string Name { get; set; }
        public IProductElement Owner { get; set; }
    }
}

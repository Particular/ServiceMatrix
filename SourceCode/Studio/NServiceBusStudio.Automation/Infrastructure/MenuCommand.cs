using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Infrastructure
{
    public class MenuCommand : IAutomationExtension, IAutomationMenuCommand
    {
        private Action execute;

        public MenuCommand(IProductElement owner, string text, Action execute)
        {
            this.Owner = owner;
            this.Name = "MenuAutomation-" + Guid.NewGuid().ToString();
            this.Visible = this.Enabled = true;
            this.Text = text;
            this.execute = execute;
        }

        public void Execute(IDynamicBindingContext context)
        {
            this.Execute();
        }

        public void Execute()
        {
            using (var tx = this.Owner.BeginTransaction())
            {
                this.execute.Invoke();
                // Only commits the tx if the invoke succeeded.
                tx.Commit();
            }
        }

        public string Name { get; private set; }

        public IProductElement Owner { get; private set; }

        public string IconPath { get; set; }

        public long SortOrder { get; set; }

        public bool Enabled { get; set; }

        public string Text { get; set; }

        public bool Visible { get; set; }
    }
}

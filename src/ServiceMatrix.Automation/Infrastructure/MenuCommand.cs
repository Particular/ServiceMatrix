namespace NServiceBusStudio.Automation.Infrastructure
{
    using System;
    using NuPattern.Runtime;
    using NuPattern.Runtime.Automation;
    using NuPattern.Runtime.Bindings;

    public class MenuCommand : IAutomationExtension, IAutomationMenuCommand
    {
        private Action execute;

        public MenuCommand(IProductElement owner, string text, Action execute)
        {
            Owner = owner;
            Name = "MenuAutomation-" + Guid.NewGuid().ToString();
            Visible = Enabled = true;
            Text = text;
            this.execute = execute;
        }

        public void Execute(IDynamicBindingContext context)
        {
            Execute();
        }

        public void Execute()
        {
            using (var tx = Owner.BeginTransaction())
            {
                execute.Invoke();
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

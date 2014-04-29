namespace NServiceBusStudio
{
    using System;
    using System.Linq;
    using System.Windows;

    partial class Command : IRenameRefactoring
    {
        public string Namespace
        {
            get { return Parent.Namespace; }
        }

        partial void Initialize()
        {
            AsElement().Deleting += (sender, eventargs) =>
            {
                // Find Component Links to the deleted Component
                var root = AsElement().Root.As<IApplication>();
                
                var commandLinks = root.Design.Services.Service.SelectMany(s => s.Components.Component.SelectMany (c => c.Publishes.CommandLinks.Where (cl => cl.CommandReference.Value == this))).ToList();
                commandLinks.ForEach(cl => cl.Delete());

                var processedCommandLinks = root.Design.Services.Service.SelectMany(s => s.Components.Component.SelectMany(c => c.Subscribes.ProcessedCommandLinks.Where(cl => cl.CommandReference.Value == this))).ToList();
                processedCommandLinks.ForEach(cl => cl.Delete());

                // Remove related components
                var result = MessageBox.Show("Do you want to delete the related Components?", "ServiceMatrix - Delete related Components", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteComponent(String.Format("{0}Sender", InstanceName));
                    DeleteComponent(String.Format("{0}Handler", InstanceName));
                }
            };
        }

        private void DeleteComponent(string componentName)
        {
            var component = Parent.Parent.Parent.
                                 Components.
                                 Component.
                                 FirstOrDefault(x => x.InstanceName == componentName);

            if (component != null)
                component.Delete();
        }
    }
}

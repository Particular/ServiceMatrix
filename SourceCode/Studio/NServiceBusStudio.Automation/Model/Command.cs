using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel.Composition;
using NServiceBusStudio.Automation.Infrastructure;
using NServiceBusStudio.Automation.Extensions;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;

namespace NServiceBusStudio
{
    partial class Command : IRenameRefactoring
    {
        public string Namespace
        {
            get { return this.Parent.Namespace; }
        }

        partial void Initialize()
        {
            this.AsElement().Deleting += (s, e) =>
            {
                var result = MessageBox.Show("Do you want to delete the related Components?", "Delete related Components", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteComponent(String.Format("{0}Sender", this.InstanceName));
                    DeleteComponent(String.Format("{0}Processor", this.InstanceName));
                }
            };
        }

        private void DeleteComponent(string componentName)
        {
            var component = this.Parent.Parent.Parent.
                                 Components.
                                 Component.
                                 FirstOrDefault(x => x.InstanceName == componentName);

            if (component != null)
                component.Delete();
        }
    }
}

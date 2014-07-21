using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.VisualStudio.Shell;
using NuPattern;
using NuPattern.Presentation;
using ServiceMatrix.Diagramming;

namespace NServiceBusStudio.Automation.Commands
{
    public class ShowNewDiagramCommand : NuPattern.Runtime.Command
    {
        [Import(typeof(SVsServiceProvider))]
        public IServiceProvider ServiceProvider { get; set; }

        public override void Execute()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var diagramsWindowManager = ServiceProvider.TryGetService<IDiagramsWindowsManager>();
            if (diagramsWindowManager != null)
            {
                using (new MouseCursor(Cursors.Arrow))
                {
                    diagramsWindowManager.Show();
                }
            }
        }
    }
}

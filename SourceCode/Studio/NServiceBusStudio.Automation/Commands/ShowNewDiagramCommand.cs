using AbstractEndpoint;
using NuPattern.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBusStudio.Automation.Dialog;
using System.Windows.Input;
using NuPattern.Presentation;
using NuPattern;
using ServiceMatrix.Diagramming.Views;
using Microsoft.VisualStudio.Shell;
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

            var diagramsWindowManager = this.ServiceProvider.TryGetService<IDiagramsWindowsManager>();
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

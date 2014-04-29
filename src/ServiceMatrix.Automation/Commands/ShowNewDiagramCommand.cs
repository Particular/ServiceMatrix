namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using NuPattern.Presentation;
    using NuPattern;
    using Microsoft.VisualStudio.Shell;
    using ServiceMatrix.Diagramming;

    using Command = NuPattern.Runtime.Command;

    public class ShowNewDiagramCommand : Command
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

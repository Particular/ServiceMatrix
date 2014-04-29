namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Shell;
    using NuPattern.Runtime;
    using ServiceMatrix.Diagramming.ViewModels;
    using NServiceBusStudio.Automation.Infrastructure;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("On Application Loaded")]
    public class OnApplicationLoadedCommand : Command
    {
        [Import(typeof(SVsServiceProvider))]
        public IServiceProvider ServiceProvider { get; set; }

        [Import]
        public IPatternWindows PatternWindows { get; set; }

        [Import]
        public ServiceMatrixDiagramAdapter ServiceMatrixDiagramAdapter { get; set; }

        [Import]
        public RemoveEmptyAddMenus RemoveEmptyAddMenus { get; set; }


        public override void Execute()
        {
            PatternWindows.ShowSolutionBuilder(ServiceProvider);

            new ShowNewDiagramCommand () { ServiceProvider = ServiceProvider }.Execute();

            ServiceMatrixDiagramAdapter.WireSolution(ServiceProvider);
            RemoveEmptyAddMenus.WireSolution(ServiceProvider);
        }
    }
}

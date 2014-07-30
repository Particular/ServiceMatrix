using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using NServiceBusStudio.Automation.Infrastructure;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("On Application Loaded")]
    public class OnApplicationLoadedCommand : NuPattern.Runtime.Command
    {
        [Import(typeof(SVsServiceProvider))]
        public IServiceProvider ServiceProvider { get; set; }

        [Import]
        public IPatternWindows PatternWindows { get; set; }

        [Import]
        public RemoveEmptyAddMenus RemoveEmptyAddMenus { get; set; }

        public override void Execute()
        {
            PatternWindows.ShowSolutionBuilder(ServiceProvider);

            new ShowNewDiagramCommand { ServiceProvider = ServiceProvider }.Execute();

            RemoveEmptyAddMenus.WireSolution(ServiceProvider);
        }
    }
}
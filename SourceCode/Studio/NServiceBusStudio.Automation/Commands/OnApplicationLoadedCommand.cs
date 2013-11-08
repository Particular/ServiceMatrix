using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using NuPattern.Runtime.Shell.ToolWindows;
using NuPattern.Runtime;
using ServiceMatrix.Diagramming.ViewModels;

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
        public ServiceMatrixDiagramAdapter ServiceMatrixDiagramAdapter { get; set; }

        public override void Execute()
        {
            this.PatternWindows.ShowSolutionBuilder(this.ServiceProvider);

            new ShowNewDiagramCommand () { ServiceProvider = this.ServiceProvider }.Execute();

            this.ServiceMatrixDiagramAdapter.WireSolution(this.ServiceProvider);
        }
    }
}

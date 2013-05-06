using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using NuPattern.Runtime.Shell.ToolWindows;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("On Application Loaded")]
    public class OnApplicationLoadedCommand : NuPattern.Runtime.Command
    {
        [Import(typeof(SVsServiceProvider))]
        public IServiceProvider ServiceProvider { get; set; }

        public override void Execute()
        {
            var ptw = this.ServiceProvider.GetService(typeof(IPackageToolWindow)) as IPackageToolWindow;
            ptw.ShowWindow<SolutionBuilderToolWindow>(true);
        }
    }
}

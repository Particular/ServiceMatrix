using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using NuPattern.Runtime.Shell;
using Microsoft.VisualStudio.Shell;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("On Application Loaded")]
    public class OnApplicationLoadedCommand : FeatureCommand
    {
        [Import(typeof(SVsServiceProvider))]
        public IServiceProvider ServiceProvider { get; set; }

        public override void Execute()
        {
            var ptw = this.ServiceProvider.GetService(typeof(IPackageToolWindow)) as IPackageToolWindow;
            ptw.ShowWindow<SolutionBuilderToolWindow>();
        }
    }
}

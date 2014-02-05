using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Composition;
using NServiceBusStudio.Automation.Extensions;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;
using AbstractEndpoint;
using System.IO;
using NuPattern.VisualStudio.Solution;
using NuGet.VisualStudio;
using NuPattern.VisualStudio;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Add Infrastructure Project References")]
    [Description("Add references in the Infrastructure Project to the required projects")]
    [CLSCompliant(false)]
    public class AddInfrastructureProjectReferences : NuPattern.Runtime.Command
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }

        [Import]
        public IVsPackageInstaller VsPackageInstaller { get; set; }

        [Import]
        public IStatusBar StatusBar { get; set; }

        public override void Execute()
        {
            var app = this.CurrentElement.Root.As<IApplication>();
            var infraproject = this.CurrentElement.GetProject();

            if (infraproject != null)
            {
                if (!infraproject.HasReference("NServiceBus"))
                {
                    infraproject.InstallNuGetPackage(VsPackageInstaller, StatusBar, "NServiceBus.Interfaces", app.NuGetPackageVersionNServiceBus);
                    infraproject.InstallNuGetPackage(VsPackageInstaller, StatusBar, "NServiceBus", app.NuGetPackageVersionNServiceBus);
                }
            }
        }
    }
}

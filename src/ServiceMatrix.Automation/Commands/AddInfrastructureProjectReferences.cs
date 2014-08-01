using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NServiceBusStudio.Automation.Extensions;
using NServiceBusStudio.Automation.Model;
using NServiceBusStudio.Automation.NuGetExtensions;
using NuGet.VisualStudio;
using NuPattern.Runtime;
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
        public IVsPackageInstallerServices VsPackageInstallerServices { get; set; }

        [Import]
        public IStatusBar StatusBar { get; set; }

        public override void Execute()
        {
            var app = CurrentElement.Root.As<IApplication>();
            var infraproject = CurrentElement.GetProject();

            if (infraproject != null)
            {
                var nuGetHelper = NuGetVersionHelper.CreateHelperFor(infraproject);

                // Get the target Nsb version. NServiceBus.Interfaces in deprecated from v5
                if (!infraproject.HasReference("NServiceBus") && app.TargetNsbVersion == TargetNsbVersion.Version4)
                {
                    infraproject.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, nuGetHelper, "NServiceBus.Interfaces", app.GetTargetNsbVersion(CurrentElement));
                    infraproject.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, nuGetHelper, "NServiceBus", app.GetTargetNsbVersion(CurrentElement));
                }
                if (!infraproject.HasReference("NServiceBus") && app.TargetNsbVersion == TargetNsbVersion.Version5)
                {
                    infraproject.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, nuGetHelper, "NServiceBus", app.GetTargetNsbVersion(CurrentElement));
                }
            }
        }
    }
}

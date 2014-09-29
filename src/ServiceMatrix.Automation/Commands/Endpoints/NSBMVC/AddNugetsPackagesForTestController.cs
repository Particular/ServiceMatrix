using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NuGet.VisualStudio;
using NuPattern.Runtime;
using NuPattern.VisualStudio;

namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBMVC
{
    using System.Linq;
    using Extensions;
    using NuPattern.ComponentModel;

    // This command exists to support projects created prior to ServiceMatrix 2.2
    // Prior to 2.2 the MVC project did not have references to jQuery or Microsoft.jQuery.Unobtrusive.Ajax
    
    [DisplayName("WebMVCEndpoint AddNugetReferencesForTestControllerCommand")]
    [Description("WebMVCEndpoint Add Nuget References For TestController")]
    [CLSCompliant(false)]
    public class AddNugetsPackagesForTestController : NuPattern.Runtime.Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        [Import]
        public IVsPackageInstaller VsPackageInstaller { get; set; }

        [Import]
        public IVsPackageInstallerServices VsPackageInstallerServices { get; set; }

        [Import]
        public IStatusBar StatusBar { get; set; }

        public override void Execute()
        {
            var nserviceBusMVC = CurrentElement.As<INServiceBusMVC>();
            if (nserviceBusMVC == null)
                return;

            var project = CurrentElement.GetProject();
            if (project == null)
            {
                return;
            }

            if (!nserviceBusMVC.TestControllerSupportDeployed)
            {
                project.InstallNugetPackageForSpecifiedVersion(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "jQuery", "1.7.1");
                project.InstallNugetPackageForSpecifiedVersion(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "Microsoft.jQuery.Unobtrusive.Ajax", "2.0.30506.0");
                nserviceBusMVC.TestControllerSupportDeployed = true;
            }
        }
    }
}

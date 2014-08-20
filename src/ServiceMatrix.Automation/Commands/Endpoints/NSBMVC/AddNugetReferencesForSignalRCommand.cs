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
    using NServiceBusStudio.Automation.Extensions;
    using NServiceBusStudio.Automation.Model;

    [DisplayName("WebMVCEndpoint AddNugetReferencesForSignalRCommand")]
    [Description("WebMVCEndpoint Add Nuget References For SignalR")]
    [CLSCompliant(false)]
    public class AddSignalRNugetPackages : NuPattern.Runtime.Command
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
            if (!nserviceBusMVC.IsSignalREnabled)
            {
                var project = CurrentElement.GetProject();
                if (project == null)
                {
                    return;
                }

                project.InstallNugetPackageForSpecifiedVersion(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "Owin", "1.0");
                project.InstallNugetPackageForSpecifiedVersion(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "Microsoft.Web.Infrastructure", "1.0.0.0");
                project.InstallNugetPackageForSpecifiedVersion(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "Microsoft.Owin.Host.SystemWeb", "1.0.0");
                project.InstallNugetPackageForSpecifiedVersion(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "Microsoft.AspNet.SignalR.Core", "1.0.0");
                project.InstallNugetPackageForSpecifiedVersion(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "Microsoft.AspNet.SignalR.Owin", "1.0.0");
                project.InstallNugetPackageForSpecifiedVersion(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "Microsoft.AspNet.SignalR.JS", "1.0.0");
                project.InstallNugetPackageForSpecifiedVersion(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "Microsoft.AspNet.SignalR", "1.0.0");
            }
        }
    }
}

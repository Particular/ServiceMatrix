namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using NServiceBusStudio.Automation.Extensions;
    using System.ComponentModel.DataAnnotations;
    using NuPattern.Runtime;
    using NuGet.VisualStudio;
    using NuPattern.VisualStudio;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Add Infrastructure Project References")]
    [Description("Add references in the Infrastructure Project to the required projects")]
    [CLSCompliant(false)]
    public class AddInfrastructureProjectReferences : Command
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
            var infraproject = CurrentElement.GetProject();

            if (infraproject != null)
            {
                if (!infraproject.HasReference("NServiceBus"))
                {
                    infraproject.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Interfaces");
                    infraproject.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus");
                }
            }
        }
    }
}

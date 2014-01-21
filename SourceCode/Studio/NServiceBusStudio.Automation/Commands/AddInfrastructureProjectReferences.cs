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

        public override void Execute()
        {
            var infraproject = this.CurrentElement.GetProject();

            if (infraproject != null)
            {
                if (!infraproject.HasReference("NServiceBus"))
                {
                    InstallPackage(infraproject, "NServiceBus");
                }
            }
        }

        private void InstallPackage(IProject project, string package)
        {
            try
            {
                try
                {
                    var packageSources = "https://go.microsoft.com/fwlink/?LinkID=206669";
                    this.VsPackageInstaller.InstallPackage(packageSources,
                                                           project.As<EnvDTE.Project>(),
                                                           package,
                                                           default(Version),
                                                           false);
                }
                catch (InvalidOperationException)
                {
                    var fallbackPackageSource = "http://builds.nservicebus.com/guestAuth/app/nuget/v1/FeedService.svc";
                    this.VsPackageInstaller.InstallPackage(fallbackPackageSource,
                                                           project.As<EnvDTE.Project>(),
                                                           package,
                                                           default(Version),
                                                           false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("NuGet Package {0} cannot be installed.", package), ex);
            }
        }

    }
}

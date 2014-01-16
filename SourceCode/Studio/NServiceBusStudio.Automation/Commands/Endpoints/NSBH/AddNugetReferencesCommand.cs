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
using NServiceBusStudio;
using NuGet.VisualStudio;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    [DisplayName("Add Nuget Project References")]
    [Description("Add references in the Endpoint Project to the required nuget projects")]
    [CLSCompliant(false)]
    public class AddNugetReferencesCommand : NuPattern.Runtime.Command
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

        public bool IgnoreHost { get; set; }

        public override void Execute()
        {
            var project = this.CurrentElement.GetProject();
            if (project == null)
            {
                return;
            }

            //<Reference Include="NServiceBus" />
            //<Reference Include="NServiceBus.Core" />
            //<Reference Include="NServiceBus.Host" />

            if (!project.HasReference("NServiceBus"))
            {
                InstallPackage(project, "NServiceBus");

                if (!this.IgnoreHost)
                {
                    InstallPackage(project, "NServiceBus.Host");
                }
            }

            var app = this.CurrentElement.Root.As<IApplication>();

            //<Reference Include="NServiceBus.ActiveMQ" />
            if (app.Transport == TransportType.ActiveMQ.ToString()) 
            {
                if (!project.HasReference("NServiceBus.ActiveMQ"))
                {
                    InstallPackage(project, "NServiceBus.ActiveMQ");
                }
            }
            else 
            {
                project.RemoveReference ("Apache.NMS");
                project.RemoveReference ("Apache.NMS.ActiveMQ");
                project.RemoveReference ("NServiceBus.Transports.ActiveMQ");
            }

            //<Reference Include="NServiceBus.Transports.RabbitMQ" />
            if (app.Transport == TransportType.RabbitMQ.ToString())
            {
                if (!project.HasReference("NServiceBus.Transports.RabbitMQ"))
                {
                    InstallPackage(project, "NServiceBus.Transports.RabbitMQ");
                }
            }
            else
            {
                project.RemoveReference("RabbitMQ.Client");
                project.RemoveReference("NServiceBus.Transports.RabbitMQ");
            }

            //<Reference Include="NServiceBus.Transports.SqlServer" />
            if (app.Transport == TransportType.SqlServer.ToString())
            {
                if (!project.HasReference("NServiceBus.Transports.SqlServer"))
                {
                    InstallPackage(project, "NServiceBus.Transports.SqlServer");
                }
            }
            else
            {
                project.RemoveReference("NServiceBus.Transports.SqlServer");
            }

            //<Reference Include="ServiceControl.Plugin.DebugSession" />
            if (!String.IsNullOrEmpty(app.ServiceControlInstanceURI))
            {
                if (!project.HasReference("ServiceControl.Plugin.DebugSession"))
                {
                    InstallPackage(project, "ServiceControl.Plugin.DebugSession");
                }
            }
            else
            {
                project.RemoveReference("ServiceControl.Plugin.DebugSession");
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

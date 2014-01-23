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
                project.InstallNuGetPackage(VsPackageInstaller, "NServiceBus");

                if (!this.IgnoreHost)
                {
                    project.InstallNuGetPackage(VsPackageInstaller, "NServiceBus.Host");
                }
            }

            var app = this.CurrentElement.Root.As<IApplication>();

            //<Reference Include="NServiceBus.ActiveMQ" />
            if (app.Transport == TransportType.ActiveMQ.ToString()) 
            {
                if (!project.HasReference("NServiceBus.ActiveMQ"))
                {
                    project.InstallNuGetPackage(VsPackageInstaller, "Apache.NMS");
                    project.InstallNuGetPackage(VsPackageInstaller, "NServiceBus.ActiveMQ");
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
                if (!project.HasReference("NServiceBus.RabbitMQ"))
                {
                    project.InstallNuGetPackage(VsPackageInstaller, "RabbitMQ.Client");
                    project.InstallNuGetPackage(VsPackageInstaller, "NServiceBus.RabbitMQ");
                }
            }
            else
            {
                project.RemoveReference("RabbitMQ.Client");
                project.RemoveReference("NServiceBus.Transports.RabbitMQ");
            }

            //<Reference Include="NServiceBus.SqlServer" />
            if (app.Transport == TransportType.SqlServer.ToString())
            {
                if (!project.HasReference("NServiceBus.SqlServer"))
                {
                    project.InstallNuGetPackage(VsPackageInstaller, "NServiceBus.SqlServer");
                }
            }
            else
            {
                project.RemoveReference("NServiceBus.SqlServer");
            }

            //<Reference Include="ServiceControl.Plugin.DebugSession" />
            //<Reference Include="ServiceControl.Plugin.Heartbeat" />
            //<Reference Include="ServiceControl.Plugin.CustomChecks" />
            if (!String.IsNullOrEmpty(app.ServiceControlInstanceURI))
            {
                if (!project.HasReference("ServiceControl.Plugin.DebugSession"))
                {
                    project.InstallNuGetPackage(VsPackageInstaller, "ServiceControl.Plugin.DebugSession");
                }

                if (!project.HasReference("ServiceControl.Plugin.Heartbeat"))
                {
                    project.InstallNuGetPackage(VsPackageInstaller, "ServiceControl.Plugin.Heartbeat");
                }

                if (!project.HasReference("ServiceControl.Plugin.CustomChecks"))
                {
                    project.InstallNuGetPackage(VsPackageInstaller, "ServiceControl.Plugin.CustomChecks");
                }
            }
            else
            {
                project.RemoveReference("ServiceControl.Plugin.DebugSession");
                project.RemoveReference("ServiceControl.Plugin.Heartbeat");
                project.RemoveReference("ServiceControl.Plugin.CustomChecks");
            }
        }
    }
}

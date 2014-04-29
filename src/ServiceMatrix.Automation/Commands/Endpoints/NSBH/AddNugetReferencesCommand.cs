namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
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

    [DisplayName("Add Nuget Project References")]
    [Description("Add references in the Endpoint Project to the required nuget projects")]
    [CLSCompliant(false)]
    public class AddNugetReferencesCommand : Command
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

        public bool IgnoreHost { get; set; }

        public override void Execute()
        {
            var app = CurrentElement.Root.As<IApplication>();
            var project = CurrentElement.GetProject();
            if (project == null)
            {
                return;
            }

            //<Reference Include="NServiceBus" />
            //<Reference Include="NServiceBus.Core" />
            //<Reference Include="NServiceBus.Host" />

            if (!project.HasReference("NServiceBus"))
            {
                project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Interfaces");
                project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus");

                if (!IgnoreHost)
                {
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Host");
                }
                else
                {
                    // This is needed for AspNet MVC Integration for the time being. 
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Autofac");
                }
            }

            //<Reference Include="NServiceBus.Transports.RabbitMQ" />
            if (app.Transport == TransportType.RabbitMQ.ToString())
            {
                if (!project.HasReference("NServiceBus.RabbitMQ"))
                {
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.RabbitMQ");
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
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.SqlServer");
                }
            }
            else
            {
                project.RemoveReference("NServiceBus.Transports.SQLServer");
            }

            //<Reference Include="NServiceBus.Azure.Transports.WindowsAzureStorageQueues" />
            if (app.Transport == TransportType.AzureQueues.ToString())
            {
                if (!project.HasReference("NServiceBus.Azure.Transports.WindowsAzureStorageQueues"))
                {
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Azure.Transports.WindowsAzureStorageQueues");
                }
            }
            else
            {
                project.RemoveReference("NServiceBus.Azure.Transports.WindowsAzureStorageQueues");
            }

            //<Reference Include="NServiceBus.Azure.Transports.WindowsAzureServiceBus" />
            if (app.Transport == TransportType.AzureServiceBus.ToString())
            {
                if (!project.HasReference("NServiceBus.Azure.Transports.WindowsAzureServiceBus"))
                {
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Azure.Transports.WindowsAzureServiceBus");
                }
            }
            else
            {
                project.RemoveReference("NServiceBus.Azure.Transports.WindowsAzureServiceBus");
            }

            //<Reference Include="ServiceControl.Plugin.DebugSession" />
            //<Reference Include="ServiceControl.Plugin.Heartbeat" />
            //<Reference Include="ServiceControl.Plugin.CustomChecks" />
            if (!String.IsNullOrEmpty(app.ServiceControlInstanceURI))
            {
                if (!project.HasReference("ServiceControl.Plugin.DebugSession"))
                {
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "ServiceControl.Plugin.DebugSession");
                }

                if (!project.HasReference("ServiceControl.Plugin.Heartbeat"))
                {
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "ServiceControl.Plugin.Heartbeat");
                }

                if (!project.HasReference("ServiceControl.Plugin.CustomChecks"))
                {
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "ServiceControl.Plugin.CustomChecks");
                }

                if (!project.HasReference("ServiceControl.Plugin.SagaAudit"))
                {
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "ServiceControl.Plugin.SagaAudit");
                }
            }
            else
            {
                project.RemoveReference("ServiceControl.Plugin.DebugSession");
                project.RemoveReference("ServiceControl.Plugin.Heartbeat");
                project.RemoveReference("ServiceControl.Plugin.CustomChecks");
                project.RemoveReference("ServiceControl.Plugin.SagaAudit");
            }
        }
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NServiceBusStudio.Automation.Extensions;
using NuGet.VisualStudio;
using NuPattern.Runtime;
using NuPattern.VisualStudio;

namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    using System.Collections.Generic;
    using Model;
    using NuPattern;
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

            // NuGet packages are installed explicitly to prevent nuget from resolving dependencies itself.
            // Packages are not uninstalled to improve performance. Instead, project references are removed.

            //<Reference Include="NServiceBus" />
            //<Reference Include="NServiceBus.Core" />
            //<Reference Include="NServiceBus.Host" />

            var targetNsbVersion = app.GetTargetNsbVersion(CurrentElement);

            if (!project.HasReference("NServiceBus"))
            {
                if (app.TargetNsbVersion == TargetNsbVersion.Version4)
                {
                    // NServiceBus.Interfaces is needed only for Major Version 4
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Interfaces", targetNsbVersion);
                }
                project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus", targetNsbVersion);

                if (!IgnoreHost)
                {
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Host", targetNsbVersion);
                }
                else
                {
                    // This is needed for AspNet MVC Integration for the time being. 
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Autofac", targetNsbVersion);
                }
            }

            //<Reference Include="NServiceBus.Transports.RabbitMQ" />
            if (app.Transport == TransportType.RabbitMQ.ToString())
            {
                if (!project.HasReference("NServiceBus.RabbitMQ"))
                {
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.RabbitMQ", targetNsbVersion);
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
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.SqlServer", targetNsbVersion);
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
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Azure.Transports.WindowsAzureStorageQueues", targetNsbVersion);
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
                    project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, "NServiceBus.Azure.Transports.WindowsAzureServiceBus", targetNsbVersion);
                }
            }
            else
            {
                project.RemoveReference("NServiceBus.Azure.Transports.WindowsAzureServiceBus");
            }

            var availablePlugins = new List<string>();
            if (app.TargetNsbVersion == TargetNsbVersion.Version5)
            {
                availablePlugins.Add("ServiceControl.Plugin.Nsb5.DebugSession");
                availablePlugins.Add("ServiceControl.Plugin.Nsb5.Heartbeat");
                availablePlugins.Add("ServiceControl.Plugin.Nsb5.CustomChecks");
                availablePlugins.Add("ServiceControl.Plugin.Nsb5.SagaAudit");
            }
            if (app.TargetNsbVersion == TargetNsbVersion.Version4)
            {
                availablePlugins.Add("ServiceControl.Plugin.Nsb4.DebugSession");
                availablePlugins.Add("ServiceControl.Plugin.Nsb4.Heartbeat");
                availablePlugins.Add("ServiceControl.Plugin.Nsb4.CustomChecks");
                availablePlugins.Add("ServiceControl.Plugin.Nsb4.SagaAudit");
            }
            
            if (!String.IsNullOrEmpty(app.ServiceControlInstanceURI))
            {
                foreach (var plugin in availablePlugins)
                {
                    if (!project.HasReference(plugin))
                    {
                        project.InstallNuGetPackage(VsPackageInstallerServices, VsPackageInstaller, StatusBar, plugin, targetNsbVersion);
                    }   
                }
            }
            else
            {
                foreach (var plugin in availablePlugins)
                {
                    project.RemoveReference(plugin);
                }
            }
        }
    }
}

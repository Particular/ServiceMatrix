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

        [Required]
        [Import(AllowDefault = true)]
        public IAbstractEndpoint Endpoint { get; set; }

        public bool IgnoreHost { get; set; }

        public override void Execute()
        {
            var basePath = Endpoint.Project.PhysicalPath;

            //<Reference Include="NServiceBus" />
            //<Reference Include="NServiceBus.Core" />
            //<Reference Include="NServiceBus.Host" />

            Endpoint.Project.DownloadNuGetPackages();

            if (!Endpoint.Project.HasReference("NServiceBus"))
            {
                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\NServiceBus.Interfaces.{1}\lib\net40\NServiceBus.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));

                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\NServiceBus.{1}\lib\net40\NServiceBus.Core.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));

                if (!this.IgnoreHost)
                {
                    Endpoint.Project.AddReference(
                        string.Format(@"{0}\packages\NServiceBus.Host.{1}\lib\net40\NServiceBus.Host.exe",
                        System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                        this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
                }
            }

            var app = Endpoint.As<IProductElement>().Root.As<IApplication>();

            //<Reference Include="NServiceBus.ActiveMQ" />
            if (app.Transport == TransportType.ActiveMQ.ToString()) 
            {
                  
                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\Apache.NMS.1.5.1\lib\net40\Apache.NMS.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
               

                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\Apache.NMS.ActiveMQ.1.5.6\lib\net40\Apache.NMS.ActiveMQ.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
                
                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\NServiceBus.ActiveMQ.{1}\lib\net40\NServiceBus.Transports.ActiveMQ.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
            }
            else 
            {
                Endpoint.Project.RemoveReference ("Apache.NMS");
                Endpoint.Project.RemoveReference ("Apache.NMS.ActiveMQ");
                Endpoint.Project.RemoveReference ("NServiceBus.Transports.ActiveMQ");
            }

            //<Reference Include="NServiceBus.Transports.RabbitMQ" />
            if (app.Transport == TransportType.RabbitMQ.ToString())
            {
                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\RabbitMQ.Client.3.0.0\lib\net30\RabbitMQ.Client.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));

                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\NServiceBus.RabbitMQ.{1}\lib\net40\NServiceBus.Transports.RabbitMQ.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
            }
            else
            {
                Endpoint.Project.RemoveReference("RabbitMQ.Client");
                Endpoint.Project.RemoveReference("NServiceBus.Transports.RabbitMQ");
            }

            //<Reference Include="NServiceBus.Transports.SqlServer" />
            if (app.Transport == TransportType.SqlServer.ToString())
            {
                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\NServiceBus.SqlServer.{1}\lib\net40\NServiceBus.Transports.SqlServer.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
            }
            else
            {
                Endpoint.Project.RemoveReference("NServiceBus.Transports.SqlServer");
            }

            if (!String.IsNullOrEmpty(app.ServiceControlInstanceURI))
            {
                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\ServiceControl.EndpointPlugin.{1}\lib\net40\ServiceControl.EndpointPlugin.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().ServiceControlEndpointPluginVersion));

                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\ServiceControl.EndpointPlugin.{1}\lib\net40\ServiceControl.EndpointPlugin.Messages.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().ServiceControlEndpointPluginVersion));
            }
            else
            {
                Endpoint.Project.RemoveReference("ServiceControl.EndpointPlugin");
                Endpoint.Project.RemoveReference("ServiceControl.EndpointPlugin.Messages");
            }
        }
    }
}

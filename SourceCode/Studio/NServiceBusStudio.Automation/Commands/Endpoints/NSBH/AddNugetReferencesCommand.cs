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

       
        public bool IgnoreHost { get; set; }

        public override void Execute()
        {
            var project = this.CurrentElement.GetProject();
            if (project == null)
            {
                return;
            }

            var basePath = project.PhysicalPath;
            
            //<Reference Include="NServiceBus" />
            //<Reference Include="NServiceBus.Core" />
            //<Reference Include="NServiceBus.Host" />


            // project.DownloadNuGetPackages();

            if (!project.HasReference("NServiceBus"))
            {
                project.DownloadNuGetPackages("NServiceBus");
                project.DownloadNuGetPackages("NServiceBus.Interfaces");

                project.AddReference(
                    string.Format(@"{0}\packages", System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath))),
                    "NServiceBus.dll");

                project.AddReference(
                    string.Format(@"{0}\packages", System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath))),
                    "NServiceBus.Core.dll");

                if (!this.IgnoreHost)
                {
                    project.DownloadNuGetPackages("NServiceBus.Host");

                    project.AddReference(
                        string.Format(@"{0}\packages", System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath))),
                        "NServiceBus.Host.exe");
                }
            }

            var app = this.CurrentElement.Root.As<IApplication>();

            //<Reference Include="NServiceBus.ActiveMQ" />
            if (app.Transport == TransportType.ActiveMQ.ToString()) 
            {

                project.DownloadNuGetPackages("NServiceBus.ActiveMQ");
                  
                project.AddReference(
                    string.Format(@"{0}\packages", System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath))),
                    "Apache.NMS.dll");

                project.AddReference(
                    string.Format(@"{0}\packages", System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath))),
                    "Apache.NMS.ActiveMQ");

                project.AddReference(
                    string.Format(@"{0}\packages", System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath))),
                    "NServiceBus.Transports.ActiveMQ");
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
                project.DownloadNuGetPackages("NServiceBus.RabbitMQ");

                project.AddReference(
                    string.Format(@"{0}\packages", System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath))),
                    "RabbitMQ.Client.dll");

                project.AddReference(
                    string.Format(@"{0}\packages", System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath))),
                    "NServiceBus.Transports.RabbitMQ.dll");
            }
            else
            {
                project.RemoveReference("RabbitMQ.Client");
                project.RemoveReference("NServiceBus.Transports.RabbitMQ");
            }

            //<Reference Include="NServiceBus.Transports.SqlServer" />
            if (app.Transport == TransportType.SqlServer.ToString())
            {
                project.DownloadNuGetPackages("NServiceBus.SqlServer");
                
                project.AddReference(
                    string.Format(@"{0}\packages", System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath))),
                    "NServiceBus.Transports.SqlServer.dll");
            }
            else
            {
                project.RemoveReference("NServiceBus.Transports.SqlServer");
            }

            if (!String.IsNullOrEmpty(app.ServiceControlInstanceURI))
            {
                project.DownloadNuGetPackages("ServiceControl.Plugin.DebugSession");

                project.AddReference(
                    string.Format(@"{0}\packages", System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath))),
                    "ServiceControl.Plugin.DebugSession.dll");
            }
            else
            {
                project.RemoveReference("ServiceControl.Plugin.DebugSession");
            }
        }
    }
}

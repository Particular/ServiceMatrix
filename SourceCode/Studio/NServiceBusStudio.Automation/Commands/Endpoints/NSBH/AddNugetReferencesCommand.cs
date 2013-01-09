using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel.Composition;
using NServiceBusStudio.Automation.Extensions;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.Patterning.Runtime;
using AbstractEndpoint;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using NServiceBusStudio;

namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    [DisplayName("Add Nuget Project References")]
    [Description("Add references in the Endpoint Project to the required nuget projects")]
    [CLSCompliant(false)]
    public class AddNugetReferencesCommand : FeatureCommand
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
                    string.Format(@"{0}\packages\NServiceBus.ActiveMQ.{1}\lib\net40\NServiceBus.ActiveMQ.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
            }
            else 
            {
                Endpoint.Project.RemoveReference ("NServiceBus.ActiveMQ");
            }

            //<Reference Include="NServiceBus.SqlServer" />
            if (app.Transport == TransportType.SqlServer.ToString())
            {
                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\NServiceBus.SqlServer.{1}\lib\net40\NServiceBus.SqlServer.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
            }
            else
            {
                Endpoint.Project.RemoveReference("NServiceBus.SqlServer");
            }
        }
    }
}

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
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using NServiceBusStudio;

namespace WebEndpoint.Automation.Commands
{
    [DisplayName("Add WebEndpoint Nuget Project References")]
    [Description("Add references in the Endpoint Project to the required nuget projects")]
    [CLSCompliant(false)]
    public class WebEndpointAddNugetReferences : FeatureCommand
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

        public override void Execute()
        {
            var basePath = Endpoint.Project.PhysicalPath;

            //<Reference Include="NServiceBus" />
            //<Reference Include="NServiceBus.Core" />


            if (!Endpoint.Project.HasReference("NServiceBus"))
            {
                Endpoint.Project.DownloadNuGetPackages();
                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\NServiceBus.Interfaces.{1}\lib\net40\NServiceBus.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));

                Endpoint.Project.AddReference(
                    string.Format(@"{0}\packages\NServiceBus.{1}\lib\net40\NServiceBus.Core.dll",
                    System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath)),
                    this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
            }
        }

    }
}

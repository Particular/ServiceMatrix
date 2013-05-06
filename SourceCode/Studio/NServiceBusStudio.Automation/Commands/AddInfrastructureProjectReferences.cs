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

        public override void Execute()
        {
            var infraproject = this.CurrentElement.GetProject();

            if (infraproject != null)
            {
                if (!infraproject.HasReference("NServiceBus"))
                {
                    var solutionPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(infraproject.PhysicalPath));
                    infraproject.DownloadNuGetPackages();

                    infraproject.AddReference(
                        string.Format(@"{0}\packages\NServiceBus.Interfaces.{1}\lib\net40\NServiceBus.dll",
                        solutionPath,
                        this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
                }
            }
        }

    }
}

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
using System.IO;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Add Infrastructure Project References")]
    [Description("Add references in the Infrastructure Project to the required projects")]
    [CLSCompliant(false)]
    public class AddInfrastructureProjectReferences : FeatureCommand
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
                        string.Format(@"{0}\packages\NServiceBus.{1}\lib\net40\NServiceBus.dll",
                        solutionPath,
                        this.CurrentElement.Root.As<IApplication>().NServiceBusVersion));
                }
            }
        }

    }
}

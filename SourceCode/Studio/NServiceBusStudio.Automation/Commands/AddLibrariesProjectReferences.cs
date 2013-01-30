using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel.Composition;
using NServiceBusStudio.Automation.Extensions;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;
using AbstractEndpoint;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using System.IO;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Add Library Project References")]
    [Description("Add references in the Library Project to the required projects")]
    [CLSCompliant(false)]
    public class AddLibraryProjectReferences : FeatureCommand
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
            var libraryproject = this.CurrentElement.GetProject();
            var app = this.CurrentElement.Product.As<Application>();

            if (libraryproject != null)
            {
                if (!libraryproject.HasReference(app.ContractsProjectName))
                {
                    libraryproject.AddReference(app.Design.ContractsProject.AsElement().GetProject());
                }

                if (!libraryproject.HasReference(app.InternalMessagesProjectName))
                {
                    libraryproject.AddReference(app.Design.InternalMessagesProject.AsElement().GetProject());
                }
            }
        }

    }
}

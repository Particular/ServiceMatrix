using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NServiceBusStudio.Automation.Extensions;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Add Library Project References")]
    [Description("Add references in the Library Project to the required projects")]
    [CLSCompliant(false)]
    public class AddLibraryProjectReferences : NuPattern.Runtime.Command
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

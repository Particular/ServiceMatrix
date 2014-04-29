namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using NServiceBusStudio.Automation.Extensions;
    using System.ComponentModel.DataAnnotations;
    using NuPattern.Runtime;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Add Library Project References")]
    [Description("Add references in the Library Project to the required projects")]
    [CLSCompliant(false)]
    public class AddLibraryProjectReferences : Command
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
            var libraryproject = CurrentElement.GetProject();
            var app = CurrentElement.Product.As<Application>();

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

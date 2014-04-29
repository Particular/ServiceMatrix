namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using NuPattern.Runtime.References;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Handles a New Library Element added")]
    [Description("Checks if the project exists and adds all the needed artifacts")]
    [CLSCompliant(false)]
    public class LibraryAddedCommand : Command
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
            // Initialize Project
            var libraries = CurrentElement.Root.As<IApplication>().Design.Libraries;
            libraries.Namespace = libraries.As<IProductElement>().Root.As<IApplication>().CodeIdentifier;
            if (libraries.As<IProductElement>().References.All(r => r.Kind != ReferenceKindConstants.ArtifactLink))
            {
                Application.SelectSolution();
                libraries.As<IProductElement>().AutomationExtensions.First(x => x.Name == "UnfoldLibrariesProject").Execute();
                CurrentElement.Root.AutomationExtensions.First(x => x.Name == "SetStartUpProjects").Execute();
            }

            // Try to set parameters for Service Library unfolding
            var serviceLibrary = CurrentElement.As<IServiceLibrary>();
            if (serviceLibrary != null)
            {
                serviceLibrary.Parent.Namespace = libraries.As<IProductElement>().Root.As<IApplication>().InstanceName
                    + "." + serviceLibrary.Parent.Parent.CodeIdentifier;
                serviceLibrary.FilePath = libraries.As<IProductElement>().Root.As<IApplication>().InstanceName + "." + libraries.As<IProductElement>().Root.As<IApplication>().ProjectNameCode
                    + "\\" + serviceLibrary.Parent.Parent.CodeIdentifier;
            }
            else
            {
                // Try to set parameters for Infrastructure Library Unfolding
                var infrastructureLibrary = CurrentElement.As<ILibrary>();
                if (infrastructureLibrary != null)
                {
                    infrastructureLibrary.Parent.Namespace = libraries.As<IProductElement>().Root.As<IApplication>().InstanceName;
                    infrastructureLibrary.FilePath = libraries.As<IProductElement>().Root.As<IApplication>().InstanceName + "." + libraries.As<IProductElement>().Root.As<IApplication>().ProjectNameCode;
                }
            }

        }
    }
}

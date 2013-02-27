using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using NuPattern.Extensibility.References;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Handles a New Library Element added")]
    [Description("Checks if the project exists and adds all the needed artifacts")]
    [CLSCompliant(false)]
    public class LibraryAddedCommand : FeatureCommand
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
            var libraries = this.CurrentElement.Root.As<IApplication>().Design.Libraries;
            libraries.Namespace = libraries.As<IProductElement>().Root.As<IApplication>().CodeIdentifier;
            if (!libraries.As<IProductElement>().References.Any(r => r.Kind == ReferenceKindConstants.ArtifactLink))
            {
                Application.SelectSolution();
                libraries.As<IProductElement>().AutomationExtensions.First(x => x.Name == "UnfoldLibrariesProject").Execute();
                this.CurrentElement.Root.AutomationExtensions.First(x => x.Name == "SetStartUpProjects").Execute();
            }

            // Try to set parameters for Service Library unfolding
            var serviceLibrary = this.CurrentElement.As<IServiceLibrary>();
            if (serviceLibrary != null)
            {
                serviceLibrary.Parent.Namespace = libraries.As<IProductElement>().Root.As<IApplication>().InstanceName
                    + "." + serviceLibrary.Parent.Parent.CodeIdentifier;
                serviceLibrary.FilePath = libraries.As<IProductElement>().Root.As<IApplication>().InstanceName + ".Code"
                    + "\\" + serviceLibrary.Parent.Parent.CodeIdentifier;
            }
            else
            {
                // Try to set parameters for Infrastructure Library Unfolding
                var infrastructureLibrary = this.CurrentElement.As<ILibrary>();
                if (infrastructureLibrary != null)
                {
                    infrastructureLibrary.Parent.Namespace = libraries.As<IProductElement>().Root.As<IApplication>().InstanceName;
                    infrastructureLibrary.FilePath = libraries.As<IProductElement>().Root.As<IApplication>().InstanceName + ".Code";
                }
            }

        }
    }
}

namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using NuPattern;
    using Command = NuPattern.Runtime.Command;

    /// <summary>
    /// Do Nothing command (just useful as placeholder)
    /// </summary>
    [DisplayName("Package Config Files")]
    [Description("Creates a ZIP file containing all XML solution, so users can get help easily from Technical Support.")]
    [CLSCompliant(false)]
    public class PackageConfigFilesCommand : Command
    {
        /// <summary>
        /// Gets or sets the current Pattern Manager.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IPatternManager PatternManager
        {
            get;
            set;
        }

        public override void Execute()
        {
            // Get Root Pattern Elements 
            var rootElements = PatternManager.Products.SelectMany(x => x.Views.SelectMany(v => v.AllElements));
            // Get all Pattern Elements 
            rootElements.Traverse<IProductElement>(e => e.GetChildren());
        }
    }
}

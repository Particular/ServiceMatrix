using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using NuPattern;

namespace NServiceBusStudio.Automation.Commands
{
    /// <summary>
    /// Do Nothing command (just useful as placeholder)
    /// </summary>
    [DisplayName("Package Config Files")]
    [Description("Creates a ZIP file containing all XML solution, so users can get help easily from Technical Support.")]
    [CLSCompliant(false)]
    public class PackageConfigFilesCommand : NuPattern.Runtime.Command
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
            var rootElements = this.PatternManager.Products.SelectMany(x => x.Views.SelectMany(v => v.AllElements));
            // Get all Pattern Elements 
            var allElements = rootElements.Traverse<IProductElement>((e) => e.GetChildren());

            // Get related references
            //var allReferences = allElements.SelectMany(x => x.References).
            //                                Where (x => x.Tag);


            
        }
    }
}

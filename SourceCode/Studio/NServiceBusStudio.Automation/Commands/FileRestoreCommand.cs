using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using NuPattern.Library.Commands;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("File Restore Command")]
    [Description("If the file doesn't exist, it runs the command.")]
    public class FileRestoreCommand : FeatureCommand
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

        [Import(AllowDefault = true)]
        public ISolution Solution { get; set; }


        public string FileName { get; set; }
        
        public string FilePath { get; set; }
        
        public string CommandName { get; set; }

        public override void Execute()
        {
            var component = this.CurrentElement.As<IComponent>();

            if (component == null || component.IsSaga)
            {
                if (!this.Solution.Find(this.FilePath + "\\" + this.FileName).Any())
                {
                    var command = this.CurrentElement.AutomationExtensions.FirstOrDefault(e => e.Name == CommandName);
                    if (command != null)
                    {
                        command.Execute();
                    }
                }
            }
        }
    }
}

namespace NServiceBusStudio.Automation.Commands
{
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using NuPattern.VisualStudio.Solution;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("File Restore Command")]
    [Description("If the file doesn't exist, it runs the command.")]
    public class FileRestoreCommand : Command
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
            var component = CurrentElement.As<NServiceBusStudio.IComponent>();

            if (component == null || component.IsSaga)
            {
                if (!Solution.Find(FilePath + "\\" + FileName).Any())
                {
                    var command = CurrentElement.AutomationExtensions.FirstOrDefault(e => e.Name == CommandName);
                    if (command != null)
                    {
                        command.Execute();
                    }
                }
            }
        }
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.Patterning;
using Microsoft.VisualStudio.Patterning.Extensibility;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using NServiceBusStudio.Automation.TypeConverters;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Design;
using System.Drawing.Design;
using NServiceBusStudio.Automation.Dialog;
using System.Windows.Input;

namespace NServiceBusStudio.Automation.Commands
{
    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    [DisplayName("Show an Command Picker Dialog for Sending")]
    [Category("General")]
    [Description("Shows a dialog where the user can choose or create a command, and adds a publish link to it.")]
    [CLSCompliant(false)]
    public class ShowCommandTypePicker : FeatureCommand
    {
        private static readonly ITraceSource tracer = Tracer.GetSourceFor<ShowCommandTypePicker>();

        /// <summary>
        /// Gets or sets the Window Factory, used to create a Window Dialog.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        private IDialogWindowFactory WindowFactory
        {
            get;
            set;
        }

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

        private IComponent CurrentComponent
        {
            get;
            set;
        }

        /// <summary>
        /// Executes this commmand.
        /// </summary>
        /// <remarks></remarks>
        public override void Execute()
        {
            this.CurrentComponent = this.CurrentElement.As<IComponent>();
            if (this.CurrentComponent == null)
            {
                this.CurrentComponent = this.CurrentElement.Parent.As<IComponent>();
            }

            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var commands = CurrentComponent.Parent.Parent.Contract.Commands.Command;
            var commandNames = commands.Select(e => e.InstanceName);

            var picker = WindowFactory.CreateDialog<ElementPicker>() as IElementPicker;

            picker.Elements = commandNames.ToList();

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().Value)
                {
                    var selectedElement = picker.SelectedItem;
                    var selectedCommand = default(ICommand);
                    if (commandNames.Contains(selectedElement))
                    {
                        selectedCommand = commands.FirstOrDefault(e => string.Equals(e.InstanceName, selectedElement, StringComparison.InvariantCultureIgnoreCase));
                    }
                    else
                    {
                        selectedCommand = CurrentComponent.Parent.Parent.Contract.Commands.CreateCommand(selectedElement);
                    }

                    CurrentComponent.Publishes.CreateCommandLink(selectedCommand.InstanceName, p => p.CommandReference.Value = selectedCommand);
                }
            }
            // Make initial trace statement for this command
            //tracer.TraceInformation(
            //    "Executing ShowElementTypePicker on current element '{0}' with AProperty '{1}'", this.CurrentElement.InstanceName, this.ElementType);

            //	TODO: Use tracer.TraceWarning() to note expected and recoverable errors
            //	TODO: Use tracer.TraceVerbose() to note internal execution logic decisions
            //	TODO: Use tracer.TraceInformation() to note key results of execution
            //	TODO: Raise exceptions for all other errors
        }
    }
}

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
using System.Windows.Input;
using NServiceBusStudio.Automation.Dialog;

namespace NServiceBusStudio.Automation.Commands
{
    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    [DisplayName("Show an Event Picker Dialog for Subscription")]
    [Category("General")]
    [Description("Shows a dialog where the user can choose an Event and adds a Subscribe link to it.")]
    [CLSCompliant(false)]
    public class ShowEventSubscriptionPicker : FeatureCommand
    {
        private static readonly ITraceSource tracer = Tracer.GetSourceFor<ShowEventSubscriptionPicker>();

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
        public ISubscribes CurrentElement
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
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var events = CurrentElement.Parent.Parent.Parent.Parent.Service.SelectMany(s => s.Contract.Events.Event);
            var eventNames = events.Select(e => e.InstanceName);
            var picker = WindowFactory.CreateDialog<EventReadOnlyPicker>() as IElementPicker;

            picker.Elements = eventNames.Union(new string[] {AnyMessageSupport.TextForUI}).ToList();

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().Value)
                {
                    var selectedElement = picker.SelectedItem;
                    var selectedEvent = default(IEvent);
                    if (selectedElement == AnyMessageSupport.TextForUI)
                    {
                        CurrentElement.CreateSubscribedEventLink(AnyMessageSupport.TextForUI, p => p.EventReference.Value = null);
                    }
                    else if (eventNames.Contains(selectedElement))
                    {
                        selectedEvent = events.FirstOrDefault(e => string.Equals(e.InstanceName, selectedElement, StringComparison.InvariantCultureIgnoreCase));
                        CurrentElement.CreateSubscribedEventLink(selectedEvent.InstanceName, p => p.EventReference.Value = selectedEvent);
                    }
                }
            }

            // TODO: Implement command automation code
            //	TODO: Use tracer.TraceWarning() to note expected and recoverable errors
            //	TODO: Use tracer.TraceVerbose() to note internal execution logic decisions
            //	TODO: Use tracer.TraceInformation() to note key results of execution
            //	TODO: Raise exceptions for all other errors
        }
    }
}

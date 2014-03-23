using NServiceBusStudio.Automation.Dialog;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Show a Messages Picker Dialog to select which is a Saga Starter in a Component")]
    [Category("General")]
    [Description("Show a Messages Picker Dialog to select which is a Saga Starter in a Component")]
    [CLSCompliant(false)]
    public class ShowComponentSagaStarterPicker : NuPattern.Runtime.Command
    {
        private static readonly ITracer tracer = Tracer.Get<ShowComponentSagaStarterPicker>();

        /// <summary>
        /// Gets or sets the Window Factory, used to create a Window Dialog.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IDialogWindowFactory WindowFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IComponent CurrentElement
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

            if (CurrentElement.Subscribes.ProcessedCommandLinks.Count() + CurrentElement.Subscribes.SubscribedEventLinks.Count() <= 1)
            {
                CurrentElement.Subscribes.ProcessedCommandLinks.ForEach(x => x.StartsSaga = true);
                CurrentElement.Subscribes.SubscribedEventLinks.ForEach(x => x.StartsSaga = true);
                CurrentElement.IsSaga = true;
                return;
            }

            var existingMessageNames = new List<string>();
            existingMessageNames.AddRange(CurrentElement.Subscribes.ProcessedCommandLinks.Select(x => x.CommandReference.Value.InstanceName));
            existingMessageNames.AddRange(CurrentElement.Subscribes.SubscribedEventLinks.Select(x => x.EventReference.Value.InstanceName));

            // Filter those events that already processed by the component
            var existingMessageSagaStarterNames = new List<string>();
            existingMessageSagaStarterNames.AddRange(CurrentElement.Subscribes.ProcessedCommandLinks.Where(x => x.StartsSaga).Select(x => x.CommandReference.Value.InstanceName));
            existingMessageSagaStarterNames.AddRange(CurrentElement.Subscribes.SubscribedEventLinks.Where(x => x.StartsSaga).Select(x => x.EventReference.Value.InstanceName));

            var picker = WindowFactory.CreateDialog<SagaStarterPicker>() as IEventPicker;
            picker.Elements = new ObservableCollection<string>(existingMessageNames);
            picker.SelectedItems = existingMessageSagaStarterNames;
            picker.Title = "Select Saga Starter";

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().Value)
                {
                    foreach (var cl in CurrentElement.Subscribes.ProcessedCommandLinks)
	                {
	                    cl.StartsSaga = picker.SelectedItems.Contains (cl.CommandReference.Value.InstanceName);
	                }

                    foreach (var el in CurrentElement.Subscribes.SubscribedEventLinks)
                    {
                        el.StartsSaga = picker.SelectedItems.Contains(el.EventReference.Value.InstanceName);
                    }
                }
            }

            CurrentElement.IsSaga = true;

            // TODO: Implement command automation code
            //	TODO: Use tracer.Warning() to note expected and recoverable errors
            //	TODO: Use tracer.Verbose() to note internal execution logic decisions
            //	TODO: Use tracer.Info() to note key results of execution
            //	TODO: Raise exceptions for all other errors
        }
    }
}
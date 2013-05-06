using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using NServiceBusStudio.Automation.Dialog;
using System.Windows.Input;
using Microsoft.VisualStudio;
using System.Windows.Interop;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Show a Use Case Picker for adding an element reference")]
    [Category("General")]
    [Description("Show a Use Case Picker for adding an element reference")]
    [CLSCompliant(false)]
    public class AddReferenceToUseCaseCommand : NuPattern.Runtime.Command
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
            //var events = CurrentElement.Parent.Parent.Parent.Parent.Service.SelectMany(s => s.Contract.Events.Event);
            var usecases = this.CurrentElement.Root.As<IApplication>().Design.UseCases.UseCase;
            var picker = new ElementPicker() as IElementPicker;
            if (picker != null)
            {
                (picker as ElementPicker).SetDropdownEditable(false);
                (picker as ElementPicker).Owner = System.Windows.Application.Current.MainWindow;
                (picker as ElementPicker).WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            }

            picker.Elements = usecases
                                .Where(uc => !uc.UseCaseLinks.Any(l => l.LinkedElementId == this.CurrentElement.Id))
                                .Select(uc => uc.InstanceName).ToList();

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().Value)
                {
                    var selectedElement = picker.SelectedItem;
                    usecases.FirstOrDefault(uc => uc.InstanceName == selectedElement).AddRelatedElement(this.CurrentElement);
                }
            }

        }
    }
}

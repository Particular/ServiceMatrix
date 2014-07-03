using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using NServiceBusStudio.Automation.Dialog;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern;
using NuPattern.Presentation;
using NuPattern.Runtime;

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

        public override void Execute()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            //var events = CurrentElement.Parent.Parent.Parent.Parent.Service.SelectMany(s => s.Contract.Events.Event);
            var usecases = CurrentElement.Root.As<IApplication>().Design.UseCases.UseCase;
            var elements = usecases
                                .Where(uc => !uc.UseCaseLinks.Any(l => l.LinkedElementId == CurrentElement.Id))
                                .Select(uc => uc.InstanceName).ToList();

            var viewModel = new ElementPickerViewModel(elements) { DropDownEditable = false };
            var picker = WindowFactory.CreateDialog<ElementPicker>(viewModel);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    var selectedElement = viewModel.SelectedItem;
                    usecases.FirstOrDefault(uc => uc.InstanceName == selectedElement).AddRelatedElement(CurrentElement);
                }
            }
        }
    }
}

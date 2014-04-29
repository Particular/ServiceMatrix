namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using NServiceBusStudio.Automation.Dialog;
    using System.Windows.Input;
    using NuPattern.Presentation;
    using System.Windows;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Show a Use Case Picker for adding an element reference")]
    [Category("General")]
    [Description("Show a Use Case Picker for adding an element reference")]
    [CLSCompliant(false)]
    public class AddReferenceToUseCaseCommand : Command
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
            var usecases = CurrentElement.Root.As<IApplication>().Design.UseCases.UseCase;
            var picker = new ElementPicker() as IElementPicker;
            if (picker != null)
            {
                (picker as ElementPicker).SetDropdownEditable(false);
                (picker as ElementPicker).Owner = Application.Current.MainWindow;
                (picker as ElementPicker).WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            picker.Elements = usecases
                                .Where(uc => !uc.UseCaseLinks.Any(l => l.LinkedElementId == CurrentElement.Id))
                                .Select(uc => uc.InstanceName).ToList();

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().Value)
                {
                    var selectedElement = picker.SelectedItem;
                    usecases.FirstOrDefault(uc => uc.InstanceName == selectedElement).AddRelatedElement(CurrentElement);
                }
            }

        }
    }
}

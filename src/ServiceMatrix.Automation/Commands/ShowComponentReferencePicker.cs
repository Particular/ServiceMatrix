namespace NServiceBusStudio.Automation.Commands
{
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using AbstractEndpoint.Automation.Dialog;
    using NServiceBusStudio.Automation.Dialog;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using NuPattern;
    using NuPattern.Presentation;
    using Command = NuPattern.Runtime.Command;

    public class ShowComponentReferencePicker : Command
    {
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

        public override void Execute()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var element = CurrentElement.As<IComponent>();
            var app = CurrentElement.Root.As<IApplication>();

            var infrastructureLibraries = app.Design.Libraries.Library
                .Except(element.LibraryReferences.LibraryReference.Select(l => l.Library));

            var serviceLibraries = element.Parent.Parent.ServiceLibraries.ServiceLibrary
                .Except(element.LibraryReferences.LibraryReference.Select(l => l.ServiceLibrary));

            var picker = WindowFactory.CreateDialog<ComponentPicker>() as IServicePicker;

            picker.Elements = new ObservableCollection<string>(
                infrastructureLibraries.Select(l => string.Format("[Global] {0}", l.InstanceName))
                .Union(serviceLibraries.Select(l => string.Format("[Service] {0}", l.InstanceName)))
                );

            picker.Title = "Add Library References...";

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().Value)
                {
                    foreach (var selectedElement in picker.SelectedItems)
                    {
                        if (selectedElement.StartsWith("[Service]"))
                        {
                            var name = selectedElement.Substring(10);
                            var library = serviceLibraries.First(l => l.InstanceName == name);
                            element.LibraryReferences.CreateLibraryReference(name,
                                r =>
                                {
                                    r.LibraryId = library.As<IProductElement>().Id;
                                    r.ServiceLibrary = library;
                                });
                        }
                        else
                        {
                            var name = selectedElement.Substring(9);
                            var library = infrastructureLibraries.First(l => l.InstanceName == name);
                            element.LibraryReferences.CreateLibraryReference(name,
                                r =>
                                {
                                    r.LibraryId = library.As<IProductElement>().Id;
                                    r.Library = library;
                                });
                        }
                    }

                    // TODO: Try to add the library links
                    
                }
            }
        }
    }
}

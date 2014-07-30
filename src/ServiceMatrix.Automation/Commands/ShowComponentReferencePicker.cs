﻿using AbstractEndpoint.Automation.Dialog;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern;
using NuPattern.Presentation;
using NuPattern.Runtime;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;

namespace NServiceBusStudio.Automation.Commands
{
    public class ShowComponentReferencePicker : NuPattern.Runtime.Command
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

            var serviceLibraries = element.Parent.Parent.ServiceLibraries.ServiceLibrary
                .Except(element.LibraryReferences.LibraryReference.Select(l => l.ServiceLibrary));

            var infraestructureAndServicesAndLibraries = serviceLibraries.Select(l => string.Format("[Service] {0}", l.InstanceName)).ToList();

            var viewModel = new ComponentPickerViewModel(infraestructureAndServicesAndLibraries)
            {
                Title = "Add Library References..."
            };

            var picker = WindowFactory.CreateDialog<ComponentPicker>(viewModel);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    foreach (var selectedElement in viewModel.SelectedItems)
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
                    }

                    // TODO: Try to add the library links

                }
            }
        }
    }
}

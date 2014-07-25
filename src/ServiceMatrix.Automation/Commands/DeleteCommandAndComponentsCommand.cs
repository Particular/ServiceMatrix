using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using AbstractEndpoint.Automation.Dialog;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Presentation;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands
{
    public class DeleteCommandAndComponentsCommand : NuPattern.Runtime.Command
    {
        private static readonly ITracer tracer = Tracer.Get<DeleteCommandAndComponentsCommand>();

        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { private get; set; }

        /// <summary>
        /// Gets or sets the Window Factory, used to create a Window Dialog.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        IDialogWindowFactory WindowFactory { get; set; }

        public override void Execute()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var application = CurrentElement.Root.As<IApplication>();
            var command = CurrentElement.As<ICommand>();

            var handlerLinks =
                application.Design.Services.Service
                    .SelectMany(s =>
                        s.Components.Component.SelectMany(c => c.Subscribes.ProcessedCommandLinks.Where(l => l.CommandReference.Value == command)))
                    .ToList();
            var senderLinks =
                application.Design.Services.Service
                    .SelectMany(s =>
                        s.Components.Component.SelectMany(c => c.Publishes.CommandLinks.Where(l => l.CommandReference.Value == command)))
                    .ToList();

            var relatedComponents =
                handlerLinks.Select(l => l.Parent.Parent)
                    .Concat(senderLinks.Select(l => l.Parent.Parent))
                    .Where(c => c != null)
                    .Distinct()
                    .ToList();

            tracer.Verbose(
                "Prompting for confirmation of deletion for command '{0}' with related components {1}",
                command.InstanceName,
                string.Join(", ", relatedComponents.Select(c => "'" + c.InstanceName + "'")));

            var viewModel =
                new RelatedComponentsPickerViewModel(relatedComponents, command)
                {
                    Title = "Delete the component?"
                };

            var picker = WindowFactory.CreateDialog<RelatedComponentsPicker>(viewModel);
            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    var selectedComponents = viewModel.SelectedComponents.ToList();

                    tracer.Info(
                        "Deleting command '{0}' and related components {1}",
                        command.InstanceName,
                        string.Join(", ", selectedComponents.Select(c => "'" + c.InstanceName + "'")));

                    foreach (var component in selectedComponents)
                    {
                        component.Delete();
                    }

                    foreach (var link in handlerLinks)
                    {
                        link.Delete();
                    }

                    foreach (var link in senderLinks)
                    {
                        link.Delete();
                    }

                    command.Delete();
                }
                else
                {
                    tracer.Verbose("Deletion for command '{0}' cancelled", command.InstanceName);
                }
            }
        }
    }
}
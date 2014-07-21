using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using AbstractEndpoint.Automation.Dialog;
using NServiceBusStudio;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Presentation;
using NuPattern.Runtime;
using NuPattern.Runtime.ToolkitInterface;

namespace AbstractEndpoint.Automation.Commands
{
    [DisplayName("Show an Starting Endpoint Picker Dialog")]
    [Category("General")]
    [Description("Shows starting Endpoint Picker dialog.")]
    [CLSCompliant(false)]
    public class ShowStartedByPicker : NuPattern.Runtime.Command
    {
        private static readonly ITracer tracer = Tracer.Get<ShowComponentLinkPicker>();

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

            var element = CurrentElement.As<IUseCase>();
            var endpoints = CurrentElement.Root.As<IApplication>()
                .Design.Endpoints.GetAll()
                .Where(e => !element.EndpointsStartingUseCases.Contains(e.As<IToolkitInterface>() as IAbstractEndpoint));

            // Get endpoint names
            var existingEndpointNames = endpoints.Select(e => String.Format("{0}", e.As<IProductElement>().InstanceName));
            var elements = endpoints.Select(e => e.As<IProductElement>().InstanceName).ToList();

            var viewModel = new EndpointPickerViewModel(elements)
            {
                Title = element.InstanceName + " Started by..."
            };

            var picker = WindowFactory.CreateDialog<EndpointPicker>(viewModel);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    foreach (var selectedElement in viewModel.SelectedItems)
                    {
                        var selectedEndpoint = default(IAbstractEndpoint);
                        if (existingEndpointNames.Contains(selectedElement))
                        {
                            selectedEndpoint = endpoints.FirstOrDefault(e => String.Equals(String.Format("{0}", e.As<IProductElement>().InstanceName), selectedElement, StringComparison.InvariantCultureIgnoreCase));
                            element.AddEndpointStartingUseCase(selectedEndpoint);
                        }
                    }
                }
            }
        }
    }
}

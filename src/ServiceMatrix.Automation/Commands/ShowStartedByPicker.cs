using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NServiceBusStudio;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using AbstractEndpoint.Automation.Dialog;
using NServiceBusStudio.Automation.Dialog;
using System.Collections.ObjectModel;
using System.Windows.Input;
using NuPattern.Diagnostics;
using NuPattern;
using NuPattern.Runtime.ToolkitInterface;
using NuPattern.Presentation;

namespace AbstractEndpoint.Automation.Commands
{
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Show an Starting Endpoint Picker Dialog")]
    [Category("General")]
    [Description("Shows starting Endpoint Picker dialog.")]
    [CLSCompliant(false)]
    public class ShowStartedByPicker : Command
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
            var existingEndpointNames = endpoints.Select(e => String.Format("{0}", (e as IToolkitInterface).As<IProductElement>().InstanceName));
            var picker = WindowFactory.CreateDialog<EndpointPicker>() as IServicePicker;
            picker.Title = element.InstanceName + " Started by...";

            picker.Elements = new ObservableCollection<string>(endpoints.Select(e => e.As<IProductElement>().InstanceName));

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().Value)
                {
                    foreach (var selectedElement in picker.SelectedItems)
                    {
                        var selectedEndpoint = default(IAbstractEndpoint);
                        if (existingEndpointNames.Contains(selectedElement))
                        {
                            selectedEndpoint = endpoints.FirstOrDefault(e => String.Equals(String.Format("{0}", (e as IToolkitInterface).As<IProductElement>().InstanceName), selectedElement, StringComparison.InvariantCultureIgnoreCase));
                            element.AddEndpointStartingUseCase(selectedEndpoint);
                        }
                    }
                }
            }
        }
    }
}

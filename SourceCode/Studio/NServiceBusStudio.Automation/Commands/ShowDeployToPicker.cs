using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using System.ComponentModel.DataAnnotations;
using NServiceBusStudio;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.Patterning.Extensibility;
using AbstractEndpoint.Automation.Dialog;
using NServiceBusStudio.Automation.Dialog;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AbstractEndpoint.Automation.Commands
{
    [DisplayName("Show an Endpoint Picker Dialog")]
    [Category("General")]
    [Description("Shows a Endpoint Picker dialog where endpoints may chosen, and then linked to the component.")]
    [CLSCompliant(false)]
    public class ShowDeployToPicker : FeatureCommand
    {
        private static readonly ITraceSource tracer = Tracer.GetSourceFor<ShowComponentLinkPicker>();

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

            var element = CurrentElement.As<NServiceBusStudio.IComponent>();
            var endpoints = this.CurrentElement.Root.As<NServiceBusStudio.IApplication>().Design.Endpoints.GetAll();
            
            // Filter those endpoints that already have the component deploed
            endpoints = endpoints.Where(e => !e.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == element));

            // Get endpoint names
            var existingEndpointNames = endpoints.Select(e => String.Format("{0}", (e as IToolkitInterface).As<IProductElement>().InstanceName));
            var picker = WindowFactory.CreateDialog<EndpointPicker>() as IServicePicker;
            picker.Title = "Deploy to...";

            picker.Elements = new ObservableCollection<string>(existingEndpointNames);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().Value)
                {
                    // Add new endpoint
                    foreach (var selectedElement in picker.SelectedItems)
                    {
                        var selectedEndpoint = default(IAbstractEndpoint);
                        if (existingEndpointNames.Contains(selectedElement))
                        {
                            selectedEndpoint = endpoints.FirstOrDefault(e => String.Equals(String.Format("{0}", (e as IToolkitInterface).As<IProductElement>().InstanceName), selectedElement, StringComparison.InvariantCultureIgnoreCase));
                            element.DeployTo(selectedEndpoint);
                        }

                    }
                }
            }
        }
    }
}

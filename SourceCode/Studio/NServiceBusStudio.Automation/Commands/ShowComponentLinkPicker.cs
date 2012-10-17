using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.Patterning;
using Microsoft.VisualStudio.Patterning.Extensibility;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using NServiceBusStudio;
using System.Collections.ObjectModel;
using AbstractEndpoint.Automation.Dialog;
using NServiceBusStudio.Automation.Dialog;
using System.Windows.Input;

namespace AbstractEndpoint.Automation.Commands
{
    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    [DisplayName("Show a Service Component Dialog")]
    [Category("General")]
    [Description("Shows a Component Picker dialog where components may chosen, and then linked to the endpoint.")]
    [CLSCompliant(false)]
    public class ShowComponentLinkPicker : FeatureCommand
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

        /// <summary>
        /// Executes this commmand.
        /// </summary>
        /// <remarks></remarks>
        public override void Execute()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var element = CurrentElement.As<IAbstractEndpointComponents>();
            var components = CurrentElement.Root.As<NServiceBusStudio.IApplication>().Design.Services.Service.SelectMany(s => s.Components.Component)
                .Except(element.AbstractComponentLinks.Select(cl => cl.ComponentReference.Value));
            var existingServiceNames = components.Select(e => String.Format ("{0}.{1}", e.Parent.Parent.InstanceName, e.InstanceName));
            var picker = WindowFactory.CreateDialog<ComponentPicker>() as IServicePicker;
            
            picker.Elements = new ObservableCollection<string>(existingServiceNames);

            picker.Title = "Deploy components...";

            var currentEndpoint = this.CurrentElement.Parent.Parent.As<IAbstractEndpoint>();
            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().Value)
                {
                    foreach (var selectedElement in picker.SelectedItems)
                    {
                        var selectedCompoenent = default(NServiceBusStudio.IComponent);
                        if (existingServiceNames.Contains(selectedElement))
                        {
                            selectedCompoenent = components.FirstOrDefault(e => String.Equals(String.Format ("{0}.{1}", e.Parent.Parent.InstanceName, e.InstanceName), selectedElement, StringComparison.InvariantCultureIgnoreCase));
                        }

                        selectedCompoenent.DeployTo(currentEndpoint);
                        //var link = CurrentElement.As<IAbstractEndpointComponents>().CreateComponentLink(selectedCompoenent.InstanceName, e => e.ComponentReference.Value = selectedCompoenent);
                        //link.ComponentReference.Value.EndpointDefined(currentEndpoint);
                    }
                }
            }
        }
    }
}

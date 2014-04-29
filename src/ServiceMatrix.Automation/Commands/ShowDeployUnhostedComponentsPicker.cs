namespace AbstractEndpoint.Automation.Commands
{
    using System;
    using System.Linq;
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
    using NuPattern.Runtime.ToolkitInterface;
    using NuPattern;
    using NuPattern.Presentation;
    using System.Text.RegularExpressions;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Show an Endpoint Picker Dialog to deploy unhosted components")]
    [Category("General")]
    [Description("Shows a Endpoint Picker dialog where endpoints may chosen, and then linked to the unhosted component.")]
    [CLSCompliant(false)]
    public class ShowDeployUnhostedComponentsPicker : Command
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
            //var nserviceBusMVC = this.As<INServiceBusMVC>();
            //var components = nserviceBusMVC.NServiceBusMVCComponents.NServiceBusMVCComponentLinks.Select(cl => cl.As<NServiceBusMVCComponentLink>().ComponentReference.Value); 
            //components = components.Where(c => c.IsSender);

            //foreach (var component in components)
            //{
            //    component.CodeIdentifier
            //}



            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            var endpoints = CurrentElement.Root.As<IApplication>().Design.Endpoints.GetAll();
            
            // Filter those components that already have deploed
            var components = CurrentElement.Root.As<IApplication>().Design.Services.Service
                                 .SelectMany(s => s.Components.Component)
                                 .Where(c => !endpoints.Any(e => e.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == c)));

            // Get endpoint names
            var existingEndpointNames = endpoints.Select(e => String.Format("{0}", e.As<IProductElement>().InstanceName));
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
                        if (existingEndpointNames.Contains(selectedElement))
                        {
                            var selectedEndpoint = endpoints.FirstOrDefault(e => String.Equals(String.Format("{0}", e.As<IProductElement>().InstanceName), selectedElement, StringComparison.InvariantCultureIgnoreCase));
                            foreach (var component in components)
                            {
                                component.DeployTo(selectedEndpoint);
                            }
                        }
                        else
                        {
                            var regexMatch = Regex.Match(selectedElement, "(?'name'[^\\[]*?)\\[(?'type'[^\\]]*?)\\]");
                            var selectedName = regexMatch.Groups["name"].Value.Trim();
                            var selectedType = regexMatch.Groups["type"].Value.Trim();

                            var app = CurrentElement.Root.As<IApplication>();
                            var handler = default(EventHandler);
                            handler = new EventHandler((s, e) =>
                            {
                                foreach (var component in components)
                                {
                                    component.DeployTo((IAbstractEndpoint)s);
                                }
                                app.OnInstantiatedEndpoint -= handler;
                            });
                            app.OnInstantiatedEndpoint += handler;

                            if (selectedType == "NServiceBus ASP.NET MVC")
                            {
                                app.Design.Endpoints.CreateNServiceBusMVC(selectedName);
                            }
                            else if (selectedType == "NServiceBus ASP.NET Web Forms")
                            {
                                app.Design.Endpoints.CreateNServiceBusWeb(selectedName);
                            }
                            else
                            {
                                app.Design.Endpoints.CreateNServiceBusHost(selectedName);
                            }
                        }
                    }
                }
            }
        }
    }
}

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
    using System.Windows;
    using Command = NuPattern.Runtime.Command;
    using IComponent = NServiceBusStudio.IComponent;

    [DisplayName("Show an Endpoint Picker Dialog")]
    [Category("General")]
    [Description("Shows a Endpoint Picker dialog where endpoints may chosen, and then linked to the component.")]
    [CLSCompliant(false)]
    public class ShowDeployToPicker : Command
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

            var element = CurrentElement.As<IComponent>();
            var endpoints = CurrentElement.Root.As<IApplication>().Design.Endpoints.GetAll();

            if (element.Subscribes.ProcessedCommandLinks.Any() &&
                CurrentElement.Root.As<IApplication>().Design.Endpoints.GetAll()
                .Count(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == element)) >= 1)
            {
                var error = String.Format ("The command-processing component {0}.{1} is already deployed. Please, undeploy the component from the endpoint and try again.", element.Parent.Parent.InstanceName, element.InstanceName);
                MessageBox.Show(error, "ServiceMatrix - Processing Component already Deployed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            // Filter those endpoints that already have the component deploed
            endpoints = endpoints.Where(e => e.EndpointComponents.AbstractComponentLinks.All(cl => cl.ComponentReference.Value != element));

            // Get endpoint names
            var existingEndpointNames = endpoints.Select(e => String.Format("{0}", e.As<IProductElement>().InstanceName));
            var picker = WindowFactory.CreateDialog<EndpointPicker>() as IEndpointPicker;
            picker.Title = "Deploy to...";
            picker.ComponentName = element.InstanceName + " component";

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
                            element.DeployTo(selectedEndpoint);
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
                                element.DeployTo((IAbstractEndpoint)s);
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

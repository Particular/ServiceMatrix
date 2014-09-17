using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using AbstractEndpoint.Automation.Dialog;
using NServiceBusStudio;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Presentation;
using NuPattern.Runtime;
using NuPattern.Runtime.ToolkitInterface;
using IComponent = NServiceBusStudio.IComponent;

namespace AbstractEndpoint.Automation.Commands
{
    using NServiceBusStudio.Automation;

    [DisplayName("Show an Endpoint Picker Dialog")]
    [Category("General")]
    [Description("Shows a Endpoint Picker dialog where endpoints may chosen, and then linked to the component.")]
    [CLSCompliant(false)]
    public class ShowDeployToPicker : NuPattern.Runtime.Command
    {
        private static readonly ITracer tracer = Tracer.Get<ShowComponentLinkPicker>();

        /// <summary>
        /// Gets or sets the Window Factory, used to create a Window Dialog.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IDialogWindowFactory WindowFactory
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

        [Required]
        [Import(AllowDefault = true)]
        public IMessageBoxService MessageBoxService
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
                var error = String.Format("The command-processing component {0}.{1} is already deployed. Please, undeploy the component from the endpoint and try again.", element.Parent.Parent.InstanceName, element.InstanceName);
                MessageBox.Show(error, "ServiceMatrix - Processing Component already Deployed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filter those endpoints that already have the component deployed
            endpoints = endpoints.Where(e => !e.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == element));

            // Get endpoint names
            var existingEndpointNames = endpoints.Select(e => String.Format("{0}", e.As<IProductElement>().InstanceName)).ToList();

            var viewModel = new EndpointPickerViewModel(existingEndpointNames)
            {
                Title = "Deploy to...",
                ComponentName = element.InstanceName + " component"
            };

            var picker = WindowFactory.CreateDialog<EndpointPicker>(viewModel);

            using (new MouseCursor(Cursors.Arrow))
            {
                if (picker.ShowDialog().GetValueOrDefault())
                {
                    // Add new endpoint
                    foreach (var selectedElement in viewModel.SelectedItems)
                    {
                        IAbstractEndpoint selectedEndpoint;
                        if (existingEndpointNames.Contains(selectedElement))
                        {
                            selectedEndpoint = endpoints.FirstOrDefault(e => String.Equals(String.Format("{0}", e.As<IProductElement>().InstanceName), selectedElement, StringComparison.InvariantCultureIgnoreCase));
                            element.DeployTo(selectedEndpoint);

                            if( selectedEndpoint is INServiceBusMVC)
                            {
                                const string recommendationMessage = "Would you like to broadcast this via SignalR?";
                                var result = MessageBoxService.Show(recommendationMessage, "ServiceMatrix - SignalR Integration", MessageBoxButton.YesNo);
               
                                if (result == MessageBoxResult.Yes)
                                {
                                    var componentElement = element.As<IProductElement>();
                                    
                                    // Find the Broadcast via SignalR command to execute
                                    var commandToExecute = componentElement.AutomationExtensions.First(c => c.Name.Equals("OnBroadcastViaSignalRCommand"));
                                    commandToExecute.Execute();
                                }
                            }
                        }
                        else
                        {
                            var regexMatch = Regex.Match(selectedElement, "(?'name'[^\\[]*?)\\[(?'type'[^\\]]*?)\\]");
                            var selectedName = regexMatch.Groups["name"].Value.Trim();
                            var selectedType = regexMatch.Groups["type"].Value.Trim();

                            var app = CurrentElement.Root.As<IApplication>();
                            var handler = default(EventHandler);
                            handler = (s, e) =>
                            {
                                element.DeployTo((IAbstractEndpoint)s);
                                app.OnInstantiatedEndpoint -= handler;
                            };
                            app.OnInstantiatedEndpoint += handler;

                            if (selectedType == "NServiceBus ASP.NET MVC")
                            {
                                app.Design.Endpoints.CreateNServiceBusMVC(selectedName);

                                const string recommendationMessage = "Would you like to broadcast this via SignalR?";
                                var result = MessageBoxService.Show(recommendationMessage, "ServiceMatrix - SignalR Integration", MessageBoxButton.YesNo);
                                
                                if (result == MessageBoxResult.Yes)
                                {
                                    var componentElement = element.As<IProductElement>();

                                    // Find the Broadcast via SignalR command to execute
                                    var commandToExecute = componentElement.AutomationExtensions.First(c => c.Name.Equals("OnBroadcastViaSignalRCommand"));
                                    commandToExecute.Execute();
                                }
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

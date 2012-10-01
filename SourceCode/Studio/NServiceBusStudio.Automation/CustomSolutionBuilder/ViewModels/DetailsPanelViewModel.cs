using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractEndpoint;
using Microsoft.VisualStudio.Patterning.Runtime.UI;
using Microsoft.VisualStudio.Patterning.Runtime;
using System.Windows;
using NServiceBusStudio.Automation.CustomSolutionBuilder.Views;
using NServiceBusStudio.Automation.CustomSolutionBuilder.Infrastructure;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels
{
    public class DetailsPanelViewModel
    {
        public Action CleanDetails { get; set; }

        public Action<int, FrameworkElement> SetPanel { get; set; }

        public void BuildDetailsForEndpoint(IAbstractEndpoint endpoint, SolutionBuilderViewModel solutionBuilderModel)
        {
            var application = endpoint.As<IProductElement>().Root.As<IApplication>();
            var allcomponents = application.Design.Services.Service.SelectMany(s => s.Components.Component);
            var components = allcomponents.Where(c => endpoint.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference != null && cl.ComponentReference.Value == c));

            this.CleanDetails();

            // Components
            this.CreateComponentsPanel(solutionBuilderModel, components, 0);

            // Commands
            this.CreateCommandsPanel(solutionBuilderModel, components, 1);

            // Events
            this.CreateEventsPanel(solutionBuilderModel, components, 2);

            // UseCases
            this.CreateUseCasesPanel(solutionBuilderModel, endpoint.As<IProductElement>(), application, 3);
        }

        public void BuildDetailsForEvent(IEvent iEvent, SolutionBuilderViewModel solutionBuilderModel)
        {
            var application = iEvent.As<IProductElement>().Root.As<IApplication>();
            var componentsPublishing = application.Design.Services.Service
                .SelectMany(s => s.Components.Component.Where(c => c.Publishes.EventLinks.Any(el => el.EventReference.Value == iEvent)));
            var componentsSubscribing = application.Design.Services.Service
                .SelectMany(s => s.Components.Component.Where(c => c.Subscribes.SubscribedEventLinks.Any(el => el.EventReference.Value == iEvent)));
            var endpoints = application.Design.Endpoints.As<IAbstractElement>().Extensions.Select(h => h.As<IToolkitInterface>() as IAbstractEndpoint)
                .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => componentsPublishing.Contains(cl.ComponentReference.Value) || componentsSubscribing.Contains(cl.ComponentReference.Value)));

            this.CleanDetails();

            // Endpoints
            this.CreateEndpointsPanel(application, solutionBuilderModel, endpoints, 0);

            // Components
            this.CreateComponentsPanel(solutionBuilderModel, componentsPublishing, 1, "Published by ");
            this.CreateComponentsPanel(solutionBuilderModel, componentsSubscribing, 2, "Subscribed by ");

            // UseCases
            this.CreateUseCasesPanel(solutionBuilderModel, iEvent.As<IProductElement>(), application, 3);
        }

        public void BuildDetailsForCommand(ICommand command, SolutionBuilderViewModel solutionBuilderModel)
        {
            var application = command.As<IProductElement>().Root.As<IApplication>();
            var componentsSending = application.Design.Services.Service
                .SelectMany(s => s.Components.Component.Where(c => c.Publishes.CommandLinks.Any(cl => cl.CommandReference.Value == command)));
            var componentsReceiving = application.Design.Services.Service
                .SelectMany(s => s.Components.Component.Where(c => c.Subscribes.ProcessedCommandLinks.Any(cl => cl.CommandReference.Value == command)));
            var endpoints = application.Design.Endpoints.As<IAbstractElement>().Extensions.Select(h => h.As<IToolkitInterface>() as IAbstractEndpoint)
                .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => componentsSending.Contains(cl.ComponentReference.Value) || componentsReceiving.Contains(cl.ComponentReference.Value)));

            this.CleanDetails();

            // Endpoints
            this.CreateEndpointsPanel(application, solutionBuilderModel, endpoints, 0);

            // Components
            this.CreateComponentsPanel(solutionBuilderModel, componentsSending, 1, "Sent by ");
            this.CreateComponentsPanel(solutionBuilderModel, componentsReceiving, 2, "Received by ");

            // UseCases
            this.CreateUseCasesPanel(solutionBuilderModel, command.As<IProductElement>(), application, 3);
        }

        public void BuildDetailsForComponent(IComponent component, SolutionBuilderViewModel solutionBuilderModel)
        {
            var application = component.As<IProductElement>().Root.As<IApplication>();
            var components = new List<IComponent> { component };
            var endpoints = application.Design.Endpoints.As<IAbstractElement>().Extensions.Select(h => h.As<IToolkitInterface>() as IAbstractEndpoint)
                .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component));

            this.CleanDetails();

            // Endpoints
            this.CreateEndpointsPanel(application, solutionBuilderModel, endpoints, 0);

            // Commands
            this.CreateCommandsPanel(solutionBuilderModel, components, 1
                , component.Publishes.As<IProductElement>());

            // Events
            this.CreateEventsPanel(solutionBuilderModel, components, 2
                , component.Publishes.As<IProductElement>(), component.Subscribes.As<IProductElement>());

            // UseCases
            this.CreateUseCasesPanel(solutionBuilderModel, component.As<IProductElement>(), application, 3);

            // Libraries
            this.CreateLibrariesPanel(solutionBuilderModel, component.As<IProductElement>(), application, 4);
        }

        private void CreateEndpointsPanel(IApplication application, SolutionBuilderViewModel solutionBuilderModel, IEnumerable<IAbstractEndpoint> endpoints, int position)
        {
            var endpointsVM = new InnerPanelViewModel();
            endpointsVM.Title = "Deployed to the following Endpoints";
            endpointsVM.Items.Add(new InnerPanelTitle { Product = application.As<IProductElement>(), Text = application.InstanceName });
            foreach (var endpoint in endpoints)
            {
                endpointsVM.Items.Add(new InnerPanelItem { Product = endpoint.As<IProductElement>(), Text = endpoint.As<IProductElement>().InstanceName });
            }
            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, endpointsVM)));
        }

        private void CreateEventsPanel(SolutionBuilderViewModel solutionBuilderModel, IEnumerable<IComponent> components, int position
                                        , IProductElement published = null, IProductElement subscribed = null)
        {
            var eventsVM = new InnerPanelViewModel();
            eventsVM.Title = "Events";
            eventsVM.Items.Add(new InnerPanelTitle { Text = "Published", Product = published, MenuFilters = new string[] { "Publish Event" }, ForceText = true });
            foreach (var publish in components.SelectMany(c => c.Publishes.EventLinks.Select(cl => cl.EventReference.Value)).Distinct())
            {
                eventsVM.Items.Add(new InnerPanelItem { Product = publish.As<IProductElement>(), Text = publish.InstanceName });
            }
            eventsVM.Items.Add(new InnerPanelTitle { Text = "Subscribed", Product = subscribed, MenuFilters = new string[] { "Process Messages…" }, ForceText = true });
            foreach (var subscribe in components.SelectMany(c => c.Subscribes.SubscribedEventLinks.Select(cl => cl.EventReference.Value)).Distinct())
            {
                if (subscribe != null)
                {
                    eventsVM.Items.Add(new InnerPanelItem { Product = subscribe.As<IProductElement>(), Text = subscribe.InstanceName });
                }
                else
                {
                    eventsVM.Items.Add(new InnerPanelItem { Product = null, Text = "[All the messages]" });
                }
            }
            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, eventsVM)));
        }

        private void CreateCommandsPanel(SolutionBuilderViewModel solutionBuilderModel, IEnumerable<IComponent> components, int position
                                        , IProductElement sent = null, IProductElement received = null)
        {
            var commandsVM = new InnerPanelViewModel();
            commandsVM.Title = "Commands";
            commandsVM.Items.Add(new InnerPanelTitle { Text = "Sent", Product = sent, MenuFilters = new string[] { "Send Command" }, ForceText = true });
            foreach (var publish in components.SelectMany(c => c.Publishes.CommandLinks.Select(cl => cl.CommandReference.Value)).Distinct())
            {
                commandsVM.Items.Add(new InnerPanelItem { Product = publish.As<IProductElement>(), Text = publish.InstanceName });
            }
            commandsVM.Items.Add(new InnerPanelTitle { Text = "Received", ForceText = true });
            foreach (var subscribe in components.SelectMany(c => c.Subscribes.ProcessedCommandLinks.Select(cl => cl.CommandReference.Value)).Distinct())
            {
                commandsVM.Items.Add(new InnerPanelItem { Product = subscribe.As<IProductElement>(), Text = subscribe.InstanceName });
            }
            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, commandsVM)));
        }


        private void CreateUseCasesPanel(SolutionBuilderViewModel solutionBuilderModel, IProductElement product, IApplication application, int position)
        {
            var commandsVM = new InnerPanelViewModel();
            commandsVM.Title = "Use Cases";
            var useCases = application.Design.UseCases.UseCase.Where(uc => uc.UseCaseLinks.Any(ul => ul.LinkedElementId == product.Id))
                .Union(application.Design.UseCases.UseCase.Where(uc => uc.RelatedEndpoints.Any(ep => ep.As<IProductElement>().Id == product.Id)))
                .Distinct();

            commandsVM.Items.Add(new InnerPanelTitle { Text = "Participates in", Product = null, MenuFilters = new string[] { }, ForceText = true });
            foreach (var usecase in useCases)
            {
                commandsVM.Items.Add(new InnerPanelItem { Product = usecase.As<IProductElement>(), Text = usecase.InstanceName });
            }
            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, commandsVM)));
        }

        private void CreateLibrariesPanel(SolutionBuilderViewModel solutionBuilderModel, IProductElement product, IApplication application, int position)
        {
            var commandsVM = new InnerPanelViewModel();
            commandsVM.Title = "Libraries";
            var globalLibraries = application.Design.Libraries.Library.Where(l => 
                                            product.As<IComponent>().LibraryReferences.LibraryReference
                                                    .Any(lr => lr.LibraryId == l.As<IProductElement>().Id))
                                            .Select(l => l.As<IProductElement>());
            commandsVM.Items.Add(new InnerPanelTitle { Text = "Global Libraries", Product = null, MenuFilters = new string[] { }, ForceText = true });
            foreach (var gl in globalLibraries)
            {
                commandsVM.Items.Add(new InnerPanelItem { Product = gl, Text = gl.InstanceName });
            }
            var serviceLibraries = product.As<IComponent>().Parent.Parent.ServiceLibraries.ServiceLibrary.Where(l =>
                                            product.As<IComponent>().LibraryReferences.LibraryReference
                                                    .Any(lr => lr.LibraryId == l.As<IProductElement>().Id))
                                            .Select(l => l.As<IProductElement>());
            commandsVM.Items.Add(new InnerPanelTitle { Text = "Service Libraries", Product = null, MenuFilters = new string[] { }, ForceText = true });
            foreach (var sl in serviceLibraries)
            {
                commandsVM.Items.Add(new InnerPanelItem { Product = sl, Text = sl.InstanceName });
            }
            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, commandsVM)));
        }

        private void CreateComponentsPanel(SolutionBuilderViewModel solutionBuilderModel, IEnumerable<IComponent> components, int position, string Preffix = "")
        {
            var componentsVM = new InnerPanelViewModel();
            componentsVM.Title = Preffix + "Components";
            foreach (var service in components.Select(c => c.Parent.Parent).Distinct())
            {
                componentsVM.Items.Add(new InnerPanelTitle { Product = service.As<IProductElement>(), Text = service.InstanceName });
                foreach (var component in components.Where(c => c.Parent.Parent == service))
                {
                    componentsVM.Items.Add(new InnerPanelItem { Product = component.As<IProductElement>(), Text = component.InstanceName });
                }
            }
            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, componentsVM)));
        }


        private void CreateComponentsForUseCasePanel(SolutionBuilderViewModel solutionBuilderModel, IUseCase useCase, int position)
        {
            var components = useCase.RelatedComponents;
            var componentsVM = new InnerPanelViewModel();
            componentsVM.Title = "Components";
            var componentsNode = new InnerPanelTitle { 
                Product = useCase.As<IProductElement>(), 
                Text = "Components", 
                ForceText = true, 
                MenuFilters = new string[] { "Add Component" },
                IconPath = solutionBuilderModel.FindNodeFor(useCase.Parent.As<IProductElement>()).IconPath
            };
            componentsVM.Items.Add(componentsNode);
            foreach (var service in components.Select(c => c.Parent.Parent).Distinct())
            {
                componentsVM.Items.Add(new InnerPanelTitle { Product = service.As<IProductElement>(), Text = service.InstanceName });
                foreach (var component in components.Where(c => c.Parent.Parent == service))
                {
                    componentsVM.Items.Add(new InnerPanelItem { Product = component.As<IProductElement>(), Text = component.InstanceName });
                }
            }
            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, componentsVM)));
        }

        private void CreateCommandsForUseCasePanel(SolutionBuilderViewModel solutionBuilderModel, IUseCase useCase, int position)
        {
            var commands = useCase.RelatedCommands;
            var commandsVM = new InnerPanelViewModel();
            commandsVM.Title = "Commands";
            var commandsNode = new InnerPanelTitle
            {
                Product = useCase.As<IProductElement>(),
                Text = "Commands",
                ForceText = true,
                MenuFilters = new string[] { "Add Command" },
                IconPath = solutionBuilderModel.FindNodeFor(useCase.Parent.As<IProductElement>()).IconPath
            };
            commandsVM.Items.Add(commandsNode);
            foreach (var service in commands.Select(c => c.Parent.Parent.Parent).Distinct())
            {
                commandsVM.Items.Add(new InnerPanelTitle { Product = service.As<IProductElement>(), Text = service.InstanceName });
                foreach (var command in commands.Where(c => c.Parent.Parent.Parent == service))
                {
                    commandsVM.Items.Add(new InnerPanelItem { Product = command.As<IProductElement>(), Text = command.InstanceName });
                }
            }
            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, commandsVM)));
        }

        private void CreateEventsForUseCasePanel(SolutionBuilderViewModel solutionBuilderModel, IUseCase useCase, int position)
        {
            var events = useCase.RelatedEvents;
            var eventsVM = new InnerPanelViewModel();
            eventsVM.Title = "Events";
            var eventsNode = new InnerPanelTitle
            {
                Product = useCase.As<IProductElement>(),
                Text = "Events",
                ForceText = true,
                MenuFilters = new string[] { "Add Event" },
                IconPath = solutionBuilderModel.FindNodeFor(useCase.Parent.As<IProductElement>()).IconPath
            };
            eventsVM.Items.Add(eventsNode);
            foreach (var service in events.Select(c => c.Parent.Parent.Parent).Distinct())
            {
                eventsVM.Items.Add(new InnerPanelTitle { Product = service.As<IProductElement>(), Text = service.InstanceName });
                foreach (var @event in events.Where(c => c.Parent.Parent.Parent == service))
                {
                    eventsVM.Items.Add(new InnerPanelItem { Product = @event.As<IProductElement>(), Text = @event.InstanceName });
                }
            }
            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, eventsVM)));
        }

        private void CreateInfrastructurePanel(SolutionBuilderViewModel solutionBuilderModel, IInfrastructure infrastructure, int position)
        {
            var infrastructureVM = new InnerPanelViewModel();
            infrastructureVM.Title = "Infrastructure";

            infrastructureVM.Items.Add(new InnerPanelTitle { Product = infrastructure.As<IProductElement>(), Text = infrastructure.InstanceName });

            if (infrastructure.Security != null)
            {
                infrastructureVM.Items.Add(new InnerPanelTitle { Product = infrastructure.Security.As<IProductElement>(), Text = infrastructure.Security.InstanceName });
                if (infrastructure.Security.Authentication != null)
                {
                    infrastructureVM.Items.Add(new InnerPanelItem { Product = infrastructure.Security.Authentication.As<IProductElement>(), Text = infrastructure.Security.Authentication.InstanceName });
                }
            }

            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, infrastructureVM)));
        }

        public void BuildDetailsForApplication(IApplication application, SolutionBuilderViewModel solutionBuilderViewModel)
        {
            var allcomponents = application.Design.Services.Service.SelectMany(s => s.Components.Component);

            this.CleanDetails();

            this.SetPanel(0, new ApplicationPropertiesView { DataContext = application });

            //Infrastructure
            this.CreateInfrastructurePanel(solutionBuilderViewModel, application.Design.Infrastructure, 1);

            //// Components
            this.CreateComponentsPanel(solutionBuilderViewModel, allcomponents, 2);

            //// Commands
            //this.CreateCommandsPanel(solutionBuilderModel, components, 1);

            //// Events
            //this.CreateEventsPanel(solutionBuilderModel, components, 2);
        }

        internal void BuildDetailsForUseCase(IUseCase useCase, SolutionBuilderViewModel solutionBuilderModel)
        {
            var application = useCase.As<IProductElement>().Root.As<IApplication>();

            this.CleanDetails();

            // Endpoints
            this.CreateEndpointsUseCasePanel(application, solutionBuilderModel, useCase, 0);

            // Components
            this.CreateComponentsForUseCasePanel(solutionBuilderModel, useCase, 1);

            // Commands
            this.CreateCommandsForUseCasePanel(solutionBuilderModel, useCase, 2);

            // Events
            this.CreateEventsForUseCasePanel(solutionBuilderModel, useCase, 3);
        }

        internal void BuildDetailsForLibrary(IProductElement library, SolutionBuilderViewModel solutionBuilderModel)
        {
            var application = library.Root.As<IApplication>();

            this.CleanDetails();

            // Components
            this.CreateComponentsForLibrary(solutionBuilderModel, application, library, 1);
        }

        private void CreateComponentsForLibrary(SolutionBuilderViewModel solutionBuilderModel, IApplication application, IProductElement library, int position)
        {
            var componentsVM = new InnerPanelViewModel();
            componentsVM.Title = "Used By Components";

            foreach (var service in application.Design.Services.Service
                                        .Where(s => s.Components.Component
                                            .Any(c => c.LibraryReferences.LibraryReference
                                                .Any(lr => lr.LibraryId == library.Id))))
            {
                componentsVM.Items.Add(new InnerPanelTitle
                {
                    Product = service.As<IProductElement>(),
                    Text = service.InstanceName,
                    ForceText = true,
                    MenuFilters = new string[] { },
                    IconPath = solutionBuilderModel.FindNodeFor(service.As<IProductElement>()).IconPath
                });
                foreach (var component in service.Components.Component
                                            .Where(c => c.LibraryReferences.LibraryReference
                                                .Any(lr => lr.LibraryId == library.Id)))
                {
                    componentsVM.Items.Add(new InnerPanelItem
                    {
                        Product = component.As<IProductElement>(),
                        Text = component.InstanceName
                    });
                }
            }

            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, componentsVM)));
        }

        private void CreateEndpointsUseCasePanel(IApplication application, SolutionBuilderViewModel solutionBuilderModel, IUseCase useCase, int position)
        {
            var endpointsVM = new InnerPanelViewModel();
            endpointsVM.Title = "Deployed to the following Endpoints";
            endpointsVM.Items.Add(new InnerPanelTitle
            {
                Product = useCase.As<IProductElement>(),
                Text = "Started In",
                ForceText = true,
                MenuFilters = new string[] { "Add Started By Endpoint" },
                IconPath = solutionBuilderModel.FindNodeFor(useCase.Parent.As<IProductElement>()).IconPath
            });
            foreach (var endpoint in useCase.EndpointsStartingUseCases)
            {
                endpointsVM.Items.Add(new InnerPanelItem { Product = endpoint.As<IProductElement>(), Text = endpoint.As<IProductElement>().InstanceName });
            }
            endpointsVM.Items.Add(new InnerPanelTitle
            {
                Product = useCase.As<IProductElement>(),
                Text = "Goes through",
                ForceText = true,
                MenuFilters = new string[] { },
                IconPath = solutionBuilderModel.FindNodeFor(useCase.Parent.As<IProductElement>()).IconPath
            });
            foreach (var endpoint in useCase.RelatedEndpoints)
            {
                endpointsVM.Items.Add(new InnerPanelItem { Product = endpoint.As<IProductElement>(), Text = endpoint.As<IProductElement>().InstanceName });
            }
            this.SetPanel(position, new LogicalView(new LogicalViewModel(solutionBuilderModel, endpointsVM)));
        }

    }
}

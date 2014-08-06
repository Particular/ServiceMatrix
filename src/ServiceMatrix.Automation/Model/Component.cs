using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using AbstractEndpoint;
using EnvDTE;
using Microsoft.VisualStudio;
using NServiceBusStudio.Automation.Extensions;
using NServiceBusStudio.Automation.Model;
using NuPattern;
using NuPattern.Runtime;
using NuPattern.Runtime.References;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio
{
    partial interface IComponent : IProjectReferenced
    {
        void EndpointDefined(IAbstractEndpoint endpoint);
        IEnumerable<IAbstractEndpoint> DeployedTo { get; }
        void DeployTo(IAbstractEndpoint endpoint);
        void Publish(IEvent @event);
        void Subscribe(ICommand command);

        void AddLinks(IAbstractEndpoint endpoint);
        void RemoveLinks(IAbstractEndpoint endpoint);

        bool IsSender { get; }
        bool IsProcessor { get; }
        bool ProcessesMultipleMessages { get; }
    }

    partial class Component : IValidatableObject, IRenameRefactoring, IAdditionalRenameRefactorings
    {
        public bool IsSender
        {
            get
            {
                return Publishes.CommandLinks.Any() || Publishes.EventLinks.Any();
            }
        }

        public bool IsProcessor
        {
            get
            {
                return Subscribes.ProcessedCommandLinks.Any() || Subscribes.SubscribedEventLinks.Any();
            }
        }

        public bool ProcessesMultipleMessages
        {
            get
            {
                return (Subscribes.ProcessedCommandLinks.Count() + Subscribes.SubscribedEventLinks.Count()) > 1;
            }
        }

        public IProject Project
        {
            get { return AsElement().GetProject(); }
        }

        partial void Initialize()
        {
            AsElement().Deleting += (s, e) =>
            {
                foreach (var endpoint in DeployedTo)
                {
                    DeleteComponentLink(endpoint);
                }
            };

            InstanceNameChanged += (s, e) =>
            {
                foreach (var endpoint in DeployedTo)
                {
                    RemoveLinks(endpoint);
                    AddLinks(endpoint);
                }
            };
        }

        private void DeleteComponentLink(IAbstractEndpoint endpoint)
        {
            if (endpoint.EndpointComponents != null)
            {
                var componentLink = endpoint.EndpointComponents.AbstractComponentLinks.FirstOrDefault(cl => cl.ComponentReference.Value == this);
                if (componentLink != null)
                {
                    componentLink.As<IProductElement>().Delete();
                }
            }
        }

        List<IAbstractEndpoint> deployedTo;

        IEnumerable<IAbstractEndpoint> IComponent.DeployedTo { get { return DeployedTo; }}
        public IList<IAbstractEndpoint> DeployedTo
        {
            get
            {
                try
                {
                    return deployedTo ?? (deployedTo = new List<IAbstractEndpoint>(As<IProductElement>().Root.As<IApplication>().Design.Endpoints.GetAll()
                        .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this))));

                }
                catch
                {
                    return null;
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            var root = As<IProductElement>().Root;
            if (!(root.As<IApplication>().Design.Endpoints.GetAll()
                .Any(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this))))
            {
                result.Add(new ValidationResult(string.Format("{0}.{1} should be allocated to an endpoint.", Parent.Parent.InstanceName, InstanceName)));
            }

            if (IsSaga && !(Subscribes.ProcessedCommandLinks.Any(c => c.StartsSaga) || Subscribes.SubscribedEventLinks.Any(c => c.StartsSaga)))
            {
                result.Add(new ValidationResult(string.Format("{0}.{1} is marked as Saga, but no Message has been defined as the Saga starter.", Parent.Parent.InstanceName, InstanceName)));
            }

            if (Subscribes.ProcessedCommandLinks.Any() &&
                root.As<IApplication>().Design.Endpoints.GetAll()
                .Count(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this)) > 1)
            {
                result.Add(new ValidationResult(string.Format("{0}.{1} is a command-processing component, and it's deployed on more than one Endpoint. This scenario is not supported, undeploy the component from one of the endpoints.", Parent.Parent.InstanceName, InstanceName)));
            }

            return result;
        }

        public void EndpointDefined(IAbstractEndpoint newEndpoint)
        {
            if (newEndpoint != null && !DeployedTo.Contains(newEndpoint))
            {
                DeployedTo.Add(newEndpoint);
            }

            foreach (var endpoint in DeployedTo)
            {
                var service = Parent.Parent;
                var endpointProject = endpoint.Project;

                if (endpointProject == null)
                {
                    continue;
                }

                if (!endpoint.Project.Folders.Any(f => f.Name == service.CodeIdentifier))
                {
                    endpoint.Project.CreateFolder(service.CodeIdentifier);
                }

                foreach (var i in Subscribes.SubscribedEventLinks)
                {
                    i.AsElement().AutomationExtensions.First(x => x.Name == "OnInstantiateCommand").Execute();
                }
                foreach (var i in Subscribes.ProcessedCommandLinks)
                {
                    i.AsElement().AutomationExtensions.First(x => x.Name == "OnInstantiateCommand").Execute();
                }
                foreach (var i in Publishes.CommandLinks)
                {
                    i.AsElement().AutomationExtensions.First(x => x.Name == "OnInstantiateCommand").Execute();
                }
                foreach (var i in Publishes.EventLinks)
                {
                    i.AsElement().AutomationExtensions.First(x => x.Name == "OnInstantiateCommand").Execute();
                }

                if (IsSaga)
                {
                    AsElement().AutomationExtensions.First(x => x.Name == "UnfoldSagaConfigureHowToFindCode").Execute();
                    AsElement().AutomationExtensions.First(x => x.Name == "UnfoldSagaDataCode").Execute();
                }

                // Add Links for Referenced Libraries
                AddLinks(endpoint);
            }
        }

        public void AddLinks(IAbstractEndpoint endpoint)
        {
            var project = endpoint.Project;
            // TODO: Evaluate where this is being called. Removed the steps related to libraries.
            
        }

        public void RemoveLinks(IAbstractEndpoint endpoint)
        {
            var project = endpoint.Project;

            if (project == null)
                return;
            // TODO: Evaluate where this is being called. Removed the steps related to libraries.
            
        }

        
        // Looks for an available component name in the service
        public static string TryGetComponentName(string suggested, IService service)
        {
            var candidate = suggested;
            for (var i = 1; ; i++)
            {
                if (!service.Components.Component.Any(c => c.InstanceName == candidate))
                {
                    break;
                }
                candidate = suggested + i.ToString(CultureInfo.InvariantCulture).Trim();
            }

            return candidate;
        }

        public void DeployTo(IAbstractEndpoint endpoint)
        {
            if (!endpoint.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this))
            {
                var linkSource = endpoint.EndpointComponents.CreateComponentLink(String.Format("{0}.{1}", Parent.Parent.InstanceName, InstanceName), e => e.ComponentReference.Value = this);
                linkSource.ComponentReference.Value.EndpointDefined(endpoint);
            }
        }

        public void Publish(IEvent @event)
        {
            Publishes.CreateLink(@event);
        }

        public void Subscribe(ICommand command)
        {
            Subscribes.CreateLink(command);
        }

        public List<string> AdditionalOriginalInstanceNames
        {
            get { return new List<string> { String.Format("{0}SagaData", OriginalInstanceName) }; }
        }

        public List<string> AdditionalInstanceNames
        {
            get { return new List<string> { String.Format("{0}SagaData", InstanceName) }; }
        }
    }
}

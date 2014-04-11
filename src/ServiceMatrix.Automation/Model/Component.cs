using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Automation.Extensions;
using NuPattern.Runtime;
using AbstractEndpoint;
using System.ComponentModel.DataAnnotations;
using System.IO;
using EnvDTE;
using System.ComponentModel.Composition;
using NServiceBusStudio.Automation.Infrastructure;
using NuPattern.VisualStudio.Solution;
using NuPattern.Runtime.References;
using NuPattern;


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
    }

    partial class Component : IValidatableObject, IRenameRefactoring
    {
        public bool IsSender
        {
            get
            {
                return this.Publishes.CommandLinks.Any() ||
                       this.Publishes.EventLinks.Any();
            }
        }

        public bool IsProcessor
        {
            get
            {
                return this.Subscribes.ProcessedCommandLinks.Any() ||
                       this.Subscribes.SubscribedEventLinks.Any();
            }
        }

        public IProject Project
        {
            get { return this.AsElement().GetProject(); }
        }

        partial void Initialize()
        {
            this.AsElement().Deleting += (s, e) =>
            {
                foreach (var endpoint in this.DeployedTo)
                {
                    DeleteComponentLink(endpoint);   
                }
            };

            this.InstanceNameChanged += (s, e) =>
            {
                foreach (var endpoint in this.DeployedTo)
                {
                    this.RemoveLinks(endpoint);
                    this.AddLinks(endpoint);
                }
            };
        }

        private void DeleteComponentLink(IAbstractEndpoint endpoint)
        {
            if (endpoint.EndpointComponents != null)
            {
                var componentLink = endpoint.EndpointComponents.AbstractComponentLinks.FirstOrDefault(cl => cl.ComponentReference.Value == this);
                componentLink.As<IProductElement>().Delete();
            }
        }

        private List<IAbstractEndpoint> deployedTo = new List<IAbstractEndpoint>();
        public IEnumerable<IAbstractEndpoint> DeployedTo
        {
            get
            {
                try
                {
                    return deployedTo ??
                        this.As<IProductElement>().Root.As<IApplication>().Design.Endpoints.GetAll()
                        .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this));
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
            var root = this.As<IProductElement>().Root;
            if (!(root.As<NServiceBusStudio.IApplication>().Design.Endpoints.GetAll()
                .Any(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this))))
            {
                result.Add(new ValidationResult(string.Format("{0}.{1} should be allocated to an endpoint.", this.Parent.Parent.InstanceName, this.InstanceName)));
            }

            if (this.IsSaga && !(this.Subscribes.ProcessedCommandLinks.Any (c => c.StartsSaga) || this.Subscribes.SubscribedEventLinks.Any(c => c.StartsSaga)))
            {
                result.Add(new ValidationResult(string.Format("{0}.{1} is  marked as Saga, but no Message has been defined as the Saga starter.", this.Parent.Parent.InstanceName, this.InstanceName)));
            }

            if (this.Subscribes.ProcessedCommandLinks.Any() &&
                root.As<NServiceBusStudio.IApplication>().Design.Endpoints.GetAll()
                .Count(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this)) > 1)
            {
                result.Add(new ValidationResult(string.Format("{0}.{1} is a command-processing component, and it's deployed on more than one Endpoint. This scenario is not supported, undeploy the component from one of the endpoints.", this.Parent.Parent.InstanceName, this.InstanceName)));
            }

            return result;
        }

        public void EndpointDefined(IAbstractEndpoint newEndpoint)
        {
            if (newEndpoint != null && !this.deployedTo.Contains(newEndpoint))
            {
                this.deployedTo.Add(newEndpoint);
            }

            foreach (var endpoint in this.DeployedTo)
            {
                var service = this.Parent.Parent;
                var endpointProject = endpoint.Project;

                if (endpointProject == null)
                {
                    continue;
                }

                if (!endpoint.Project.Folders.Any(f => f.Name == service.CodeIdentifier))
                {
                    endpoint.Project.CreateFolder(service.CodeIdentifier);
                }

                foreach (var i in this.Subscribes.SubscribedEventLinks)
                {
                    i.AsElement().AutomationExtensions.First(x => x.Name == "OnInstantiateCommand").Execute();
                }
                foreach (var i in this.Subscribes.ProcessedCommandLinks)
                {
                    i.AsElement().AutomationExtensions.First(x => x.Name == "OnInstantiateCommand").Execute();
                }
                foreach (var i in this.Publishes.CommandLinks)
                {
                    i.AsElement().AutomationExtensions.First(x => x.Name == "OnInstantiateCommand").Execute();
                }
                foreach (var i in this.Publishes.EventLinks)
                {
                    i.AsElement().AutomationExtensions.First(x => x.Name == "OnInstantiateCommand").Execute();
                }

                if (this.IsSaga)
                {
                    this.AsElement().AutomationExtensions.First(x => x.Name == "UnfoldSagaConfigureHowToFindCode").Execute();
                    this.AsElement().AutomationExtensions.First(x => x.Name == "UnfoldSagaDataCode").Execute();
                }

                // Add Links for Referenced Libraries
                this.AddLinks(endpoint);
            }
        }

        public void AddLinks(IAbstractEndpoint endpoint)
        {
            var project = endpoint.Project;

            // 1. Add Links for References
            foreach (var libraryLink in this.LibraryReferences.LibraryReference)
            {
                IProductElement element = null;

                if (libraryLink.Library != null)
                {
                    if (libraryLink.Library.As<IProductElement>().References.Any(r => r.Kind == ReferenceKindConstants.ArtifactLink))
                    {
                        element = libraryLink.Library.As<IProductElement>();
                    }
                }
                else
                {
                    if (libraryLink.ServiceLibrary.As<IProductElement>().References.Any(r => r.Kind == ReferenceKindConstants.ArtifactLink))
                    {
                        element = libraryLink.ServiceLibrary.As<IProductElement>();
                    }
                }

                var suggestedPath = endpoint.CustomizationFuncs().BuildPathForComponentCode(endpoint, this.Parent.Parent, null, true);

                AddLinkToProject(project, element, suggestedPath, i => i.First());
            }
        }



        public void RemoveLinks(IAbstractEndpoint endpoint)
        {
            var project = endpoint.Project;

            if (project == null)
                return;

            // 1. Remove Links for References
            foreach (var libraryLink in this.LibraryReferences.LibraryReference)
            {
                IProductElement element = null;

                if (libraryLink.Library != null)
                {
                    if (libraryLink.Library.As<IProductElement>().References.Any(r => r.Kind == ReferenceKindConstants.ArtifactLink))
                    {
                        element = libraryLink.Library.As<IProductElement>();
                    }
                }
                else
                {
                    if (libraryLink.ServiceLibrary.As<IProductElement>().References.Any(r => r.Kind == ReferenceKindConstants.ArtifactLink))
                    {
                        element = libraryLink.ServiceLibrary.As<IProductElement>();
                    }
                }

                var suggestedPath = endpoint.CustomizationFuncs().BuildPathForComponentCode(endpoint, this.Parent.Parent, null, false);
                RemoveLinkFromProject(project, element.InstanceName + ".cs", suggestedPath);
            }
        }

        private static void AddLinkToProject(IProject project, IProductElement element, string suggestedPath, Func<IEnumerable<IItem>, IItem> filter)
        {
            var sourceFile = FindSourceItemForElement(element, filter);
            var container = project.As<EnvDTE.Project>().ProjectItems;
            container = FindProjectFolder(container, suggestedPath);

            if (container != null)
            {
                try
                {
                    container.AddFromFile(sourceFile.PhysicalPath);
                }
                catch { } // If the link is already in place we will ignore the exception
            }
        }

        private void RemoveLinkFromProject(IProject project, string fileName, string suggestedPath)
        {
            var container = project.As<EnvDTE.Project>().ProjectItems;
            container = FindProjectFolder(container, suggestedPath);

            if (container != null)
            {
                foreach (var file in container)
                {
                    if (file != null && file.As<EnvDTE.ProjectItem>().Name == fileName)
                    {
                        file.As<ProjectItem>().Delete();
                        break;
                    }
                }
            }
        }


        private static IItem FindSourceItemForElement(IProductElement element, Func<IEnumerable<IItem>, IItem> filter)
        {
            var references = ServiceProviderExtensions
                .GetService<IUriReferenceService>(element.Product.ProductState);

            var sourceFile = filter(SolutionArtifactLinkReference.GetResolvedReferences(element, references).OfType<IItem>());
            return sourceFile;
        }

        private static EnvDTE.ProjectItems FindProjectFolder(EnvDTE.ProjectItems container, string suggestedPath)
        {
            var path = suggestedPath.Split('\\').Skip(1);

            foreach (var stepName in path)
            {
                foreach (var folder in container)
                {
                    if (folder != null && folder.As<EnvDTE.ProjectItem>().Name == stepName)
                    {
                        container = folder.As<EnvDTE.ProjectItem>().ProjectItems;
                        break;
                    }
                }
            }
            
            return container;
        }

        // Looks for an available component name in the service
        public static string TryGetComponentName(string suggested, IService service)
        {
            var candidate = suggested;
            for (int i = 1; true; i++)
            {
                if (!service.Components.Component.Any(c => c.InstanceName == candidate))
                {
                    break;
                }
                candidate = suggested + i.ToString().Trim();
            }

            return candidate;
        }

        public void DeployTo(IAbstractEndpoint endpoint)
        {
            if (!endpoint.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this))
            {
                var linkSource = endpoint.EndpointComponents.CreateComponentLink(String.Format("{0}.{1}", this.Parent.Parent.InstanceName, this.InstanceName), e => e.ComponentReference.Value = this);
                linkSource.ComponentReference.Value.EndpointDefined(endpoint);
            }
        }
       
        public void Publish(IEvent @event)
        {
            this.Publishes.CreateLink(@event);
        }

        public void Subscribe(ICommand command)
        {
            this.Subscribes.CreateLink(command);
        }
    }
}

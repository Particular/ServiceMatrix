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

    partial class Component : IValidatableObject, IRenameRefactoring, IAdditionalRenameRefactorings
    {
        public bool IsSender
        {
            get
            {
                return Publishes.CommandLinks.Any() ||
                       Publishes.EventLinks.Any();
            }
        }

        public bool IsProcessor
        {
            get
            {
                return Subscribes.ProcessedCommandLinks.Any() ||
                       Subscribes.SubscribedEventLinks.Any();
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
                        As<IProductElement>().Root.As<IApplication>().Design.Endpoints.GetAll()
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
            var root = As<IProductElement>().Root;
            if (!(root.As<IApplication>().Design.Endpoints.GetAll()
                .Any(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this))))
            {
                result.Add(new ValidationResult(string.Format("{0}.{1} should be allocated to an endpoint.", Parent.Parent.InstanceName, InstanceName)));
            }

            if (IsSaga && !(Subscribes.ProcessedCommandLinks.Any (c => c.StartsSaga) || Subscribes.SubscribedEventLinks.Any(c => c.StartsSaga)))
            {
                result.Add(new ValidationResult(string.Format("{0}.{1} is  marked as Saga, but no Message has been defined as the Saga starter.", Parent.Parent.InstanceName, InstanceName)));
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
            if (newEndpoint != null && !deployedTo.Contains(newEndpoint))
            {
                deployedTo.Add(newEndpoint);
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

            // 1. Add Links for References
            foreach (var libraryLink in LibraryReferences.LibraryReference)
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

                var suggestedPath = endpoint.CustomizationFuncs().BuildPathForComponentCode(endpoint, Parent.Parent, null, true);

                AddLinkToProject(project, element, suggestedPath, i => i.First());
            }
        }



        public void RemoveLinks(IAbstractEndpoint endpoint)
        {
            var project = endpoint.Project;

            if (project == null)
                return;

            // 1. Remove Links for References
            foreach (var libraryLink in LibraryReferences.LibraryReference)
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

                var suggestedPath = endpoint.CustomizationFuncs().BuildPathForComponentCode(endpoint, Parent.Parent, null, false);
                RemoveLinkFromProject(project, element.InstanceName + ".cs", suggestedPath);
            }
        }

        private static void AddLinkToProject(IProject project, IProductElement element, string suggestedPath, Func<IEnumerable<IItem>, IItem> filter)
        {
            var sourceFile = FindSourceItemForElement(element, filter);
            var container = project.As<Project>().ProjectItems;
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
            var container = project.As<Project>().ProjectItems;
            container = FindProjectFolder(container, suggestedPath);

            if (container != null)
            {
                foreach (var file in container)
                {
                    if (file != null && file.As<ProjectItem>().Name == fileName)
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

        private static ProjectItems FindProjectFolder(ProjectItems container, string suggestedPath)
        {
            var path = suggestedPath.Split('\\').Skip(1);

            foreach (var stepName in path)
            {
                foreach (var folder in container)
                {
                    if (folder != null && folder.As<ProjectItem>().Name == stepName)
                    {
                        container = folder.As<ProjectItem>().ProjectItems;
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
            get { return new List<string>() { String.Format("{0}SagaData", OriginalInstanceName) }; }
        }

        public List<string> AdditionalInstanceNames
        {
            get { return new List<string>() { String.Format("{0}SagaData", InstanceName) }; }
        }
    }
}

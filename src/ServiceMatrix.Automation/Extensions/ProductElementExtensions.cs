namespace NServiceBusStudio.Automation.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using EnvDTE;
    using NuPattern.Runtime;
    using NServiceBusStudio.Automation.Infrastructure;
    using NuPattern.Runtime.References;
    using NuPattern;
    using NuPattern.Presentation;
    using NuPattern.VisualStudio.Solution;
    using System.Windows.Input;

    public static class ProductElementExtensions
    {
        public static void ShowHideProperty(this IProductElement element, string propertyName, bool isVisible)
        {
            var property = element.Properties.FirstOrDefault(x => x.DefinitionName == propertyName);
            if (property == null)
                return;
            
            var propertyInfo = property.Info as dynamic;
            if (propertyInfo == null)
                return;
            
            propertyInfo.IsVisible = isVisible;
        }

        public static IProject GetProject(this IProductElement element)
        {
            if (element == null)
                return null;

            try
            {
                var references = element.Product.ProductState.GetService<IUriReferenceService>();

                return SolutionArtifactLinkReference
                    .GetResolvedReferences(element, references)
                    .OfType<IProject>()
                    .FirstOrDefault();
            }
            catch { return null; }
        }

        public static bool RenameElement(this IProductElement element, IToolkitElement toolkitElement, IUriReferenceService uriService, RefactoringManager refactoringManager, Documents EnvDTEDocuments = null)
        {
            using (new MouseCursor(Cursors.Wait))
            {
                var renameRefactoring = toolkitElement as IRenameRefactoring;
                if (renameRefactoring != null)
                {
                    element.CloseDocuments(uriService, EnvDTEDocuments);
                    refactoringManager.RenameClass(renameRefactoring.Namespace, renameRefactoring.OriginalInstanceName, renameRefactoring.InstanceName);
                    element.RenameArtifactLinks(uriService, renameRefactoring.OriginalInstanceName, renameRefactoring.InstanceName);

                    var additionalRenameRefactorings = toolkitElement as IAdditionalRenameRefactorings;
                    if (additionalRenameRefactorings != null)
                    {
                        for (var i = 0; i < additionalRenameRefactorings.AdditionalInstanceNames.Count; i++)
                        {
                            refactoringManager.RenameClass(renameRefactoring.Namespace, additionalRenameRefactorings.AdditionalOriginalInstanceNames[i], additionalRenameRefactorings.AdditionalInstanceNames[i]);
                            element.RenameArtifactLinks(uriService, additionalRenameRefactorings.AdditionalOriginalInstanceNames[i], additionalRenameRefactorings.AdditionalInstanceNames[i]);
                        }   
                    }

                    return true;
                }

                var renameRefactoringNamespace = toolkitElement as IRenameRefactoringNamespace;
                if (renameRefactoringNamespace != null && toolkitElement.InstanceName != "" && toolkitElement is IService)
                {
                    var service = toolkitElement as IService;
                    service.Rename(uriService, refactoringManager);

                    //MessageBox.Show("The Service renaming is almost done. Please, re-open the solution to finish with the renaming.", "ServiceMatrix - Rename Service", MessageBoxButton.OK);
                    return true;
                }
                
                var renameRefactoringNotSupported = toolkitElement as IRenameRefactoringNotSupported;
                if (renameRefactoringNotSupported != null && toolkitElement.InstanceName != "")
                {
                    var result = MessageBox.Show("This element doesn't support code refactoring, you will need to update your code manually. Do you want to do the renaming anyway?", "ServiceMatrix - Rename element", MessageBoxButton.YesNo);
                    return result == MessageBoxResult.Yes;
                }
                
                return true;
            }
        }

        
        public static void RenameArtifactLinks(this IProductElement element, IUriReferenceService uriService, string currentName, string newName)
        {
            foreach (var referenceLink in element.References)
            {
                var item = default(IItemContainer);
                try
                {
                    item = uriService.ResolveUri<IItemContainer>(new Uri(referenceLink.Value));
                }
                catch { }

                if (item != null && 
                    item.Kind == ItemKind.Item &&
                    Path.GetFileNameWithoutExtension(item.Name) != newName)
                {
                    item.As<ProjectItem>().Name = item.Name.Replace(currentName, newName);
                }
            }
        }

        public static void RemoveArtifactLinks(this IProductElement element, IUriReferenceService uriService, ISolution solution)
        {
            using (new MouseCursor(Cursors.Wait))
            {
                foreach (var referenceLink in element.References)
                {
                    var item = default(IItemContainer);
                    try
                    {
                        item = uriService.ResolveUri<IItemContainer>(new Uri(referenceLink.Value));
                    }
                    catch { } // TODO: Figure out what specific exception needs to be caught!

                    if (item != null)
                    {
                        var physicalPath = item.PhysicalPath;

                        if (item.Kind == ItemKind.Project)
                        {
                            solution.As<Solution>().Remove(item.As<Project>());
                            Directory.Delete(Path.GetDirectoryName(physicalPath), true);
                        }
                        else if (item.Kind == ItemKind.Item)
                        {
                            item.As<ProjectItem>().Delete();
                            File.Delete(physicalPath);
                        }
                    }
                }
            }
        }

        public static void CloseDocuments(this IProductElement element, IUriReferenceService uriService, Documents EnvDTEDocuments)
        {
            var documents = EnvDTEDocuments.OfType<Document>();

            foreach (var referenceLink in element.References)
            {
                var item = default(IItemContainer);
                try
                {
                    item = uriService.ResolveUri<IItemContainer>(new Uri(referenceLink.Value));
                }
                catch { }

                if (item != null &&
                    item.Kind == ItemKind.Item &&
                    documents.Any (x => x.FullName == item.PhysicalPath))
                {
                    documents.First(x => x.FullName == item.PhysicalPath).Close(vsSaveChanges.vsSaveChangesYes);
                }
            }

        }
    }
}

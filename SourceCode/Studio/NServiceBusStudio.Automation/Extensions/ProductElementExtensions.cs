using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using Microsoft.VisualStudio.Patterning.Extensibility.References;
using System.IO;
using NServiceBusStudio.Automation.Infrastructure;
using System.Windows;

namespace NServiceBusStudio.Automation.Extensions
{
    public static class ProductElementExtensions
    {
        public static IProject GetProject(this IProductElement element)
        {
            if (element == null)
                return null;

            var references = element.Product.ProductState.GetService<IFxrUriReferenceService>();

            return SolutionArtifactLinkReference
                .GetResolvedReferences(element, references)
                .OfType<IProject>()
                .FirstOrDefault();
        }

        public static bool RenameElement(this IProductElement element, IToolkitElement toolkitElement, IFxrUriReferenceService uriService, RefactoringManager refactoringManager)
        {
            var renameRefactoring = toolkitElement as IRenameRefactoring;
            if (renameRefactoring != null)
            {
                refactoringManager.RenameClass(renameRefactoring.Namespace, renameRefactoring.OriginalInstanceName, renameRefactoring.InstanceName);
                element.RenameArtifactLinks(uriService, renameRefactoring.OriginalInstanceName, renameRefactoring.InstanceName);
                return true;
            }

            var renameRefactoringNotSupported = toolkitElement as IRenameRefactoringNotSupported;
            if (renameRefactoringNotSupported != null)
            {
                var result = MessageBox.Show("This element doesn't support code refactoring, you will need to update your code manually. Do you want to do the renaming anyway?", "Rename element", MessageBoxButton.YesNo);
                return result == MessageBoxResult.Yes;
            }

            return true;
        }

        public static void RenameArtifactLinks(this IProductElement element, IFxrUriReferenceService uriService, string currentName, string newName)
        {
            foreach (var referenceLink in element.References)
            {
                var item = default(IItemContainer);
                try
                {
                    item = uriService.ResolveUri<IItemContainer>(new Uri(referenceLink.Value));
                }
                catch { }

                if (item != null && item.Kind == ItemKind.Item)
                {
                    item.As<ProjectItem>().Name = item.Name.Replace (currentName, newName);
                }
            }
        }

        public static void RemoveArtifactLinks(this IProductElement element, IFxrUriReferenceService uriService, ISolution solution)
        {
            foreach (var referenceLink in element.References)
            {
                var item = default (IItemContainer);
                try
                {
                    item = uriService.ResolveUri<IItemContainer>(new Uri(referenceLink.Value));
                }
                catch { }

                if (item != null)
                {
                    var physicalPath = item.PhysicalPath;

                    if (item.Kind == ItemKind.Project)
                    {
                        solution.As<Solution>().Remove(item.As<Project>());
                        System.IO.Directory.Delete(Path.GetDirectoryName(physicalPath), true);
                    }
                    else if (item.Kind == ItemKind.Item)
                    {
                        item.As<ProjectItem>().Delete();
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
        }
    }
}

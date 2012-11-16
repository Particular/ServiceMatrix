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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using Microsoft.VisualStudio.Patterning.Extensibility.References;

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

    }
}

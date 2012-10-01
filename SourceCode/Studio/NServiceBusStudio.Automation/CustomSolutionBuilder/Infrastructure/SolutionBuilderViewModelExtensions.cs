using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Patterning.Runtime.UI;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Infrastructure
{
    public static class SolutionBuilderViewModelExtensions
    {
        public static ProductElementViewModel SearchInNodes(IEnumerable<ProductElementViewModel> nodesCollection, IProductElement target)
        {
            foreach (ProductElementViewModel model in nodesCollection)
            {
                if (model.Model == target)
                {
                    return model;
                }
                ProductElementViewModel model2 = SearchInNodes(model.Nodes, target);
                if (model2 != null)
                {
                    return model2;
                }
            }
            return null;
        }

        public static ProductElementViewModel FindNodeFor(this SolutionBuilderViewModel ViewModel, IProductElement target)
        {
            return SearchInNodes(ViewModel.Nodes, target);
        }
    }
}

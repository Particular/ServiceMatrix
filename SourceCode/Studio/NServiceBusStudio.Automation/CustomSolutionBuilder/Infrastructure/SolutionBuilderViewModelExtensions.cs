using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime.UI;
using NuPattern.Runtime;
using NuPattern.Runtime.UI.ViewModels;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Infrastructure
{
    public static class SolutionBuilderViewModelExtensions
    {
        public static IProductElementViewModel SearchInNodes(IEnumerable<IProductElementViewModel> nodesCollection, IProductElement target)
        {
            foreach (IProductElementViewModel model in nodesCollection)
            {
                if (model.Model == target)
                {
                    return model;
                }
                IProductElementViewModel model2 = SearchInNodes(model.NodesViewModel, target);
                if (model2 != null)
                {
                    return model2;
                }
            }
            return null;
        }

        public static IProductElementViewModel FindNodeFor(this ISolutionBuilderViewModel ViewModel, IProductElement target)
        {
            return SearchInNodes(ViewModel.NodesViewModel, target);
        }
    }
}

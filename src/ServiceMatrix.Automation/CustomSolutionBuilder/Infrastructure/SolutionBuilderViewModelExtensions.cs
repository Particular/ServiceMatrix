namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Infrastructure
{
    using System.Collections.Generic;
    using NuPattern.Runtime;
    using NuPattern.Runtime.UI.ViewModels;

    public static class SolutionBuilderViewModelExtensions
    {
        public static IProductElementViewModel SearchInNodes(IEnumerable<IProductElementViewModel> nodesCollection, IProductElement target)
        {
            foreach (var model in nodesCollection)
            {
                if (model.Data == target)
                {
                    return model;
                }
                var model2 = SearchInNodes(model.ChildNodes, target);
                if (model2 != null)
                {
                    return model2;
                }
            }
            return null;
        }

        public static IProductElementViewModel FindNodeFor(this ISolutionBuilderViewModel ViewModel, IProductElement target)
        {
            return SearchInNodes(ViewModel.TopLevelNodes, target);
        }
    }
}

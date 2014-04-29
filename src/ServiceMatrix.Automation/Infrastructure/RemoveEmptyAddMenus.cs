namespace NServiceBusStudio.Automation.Infrastructure
{
    using Microsoft.VisualStudio.Shell;
    using NuPattern.Runtime;
    using NuPattern.Runtime.UI.ViewModels;
    using NuPattern.VisualStudio.Solution;
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel.Composition;
    using System.Linq;
    using NuPattern;

    [Export]
    public class RemoveEmptyAddMenus
    {
        public ISolution Solution { get; set; }

        public IPatternWindows PatternWindows { get; set; }

        public ISolutionBuilderViewModel SolutionBuilderViewModel { get; set; }

        [ImportingConstructor]
        public RemoveEmptyAddMenus([Import] ISolution solution,
                                   [Import] IPatternWindows patternWindows,
                                   [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            Solution = solution;
            PatternWindows = patternWindows;

            StartListening(serviceProvider);
        }

        private void StartListening(IServiceProvider serviceProvider)
        {
            var events = serviceProvider.TryGetService<ISolutionEvents>();

            events.SolutionOpened += (s, e) =>
            {
                WireSolution(serviceProvider);
            };

            events.SolutionClosed += (s, e) =>
            {
                UnhandleChanges(SolutionBuilderViewModel.TopLevelNodes);
                SolutionBuilderViewModel = null;
            };

            if (Solution.IsOpen)
            {
                WireSolution(serviceProvider);
            }
        }

        public void WireSolution(IServiceProvider serviceProvider)
        {
            if (SolutionBuilderViewModel == null)
            {
                SolutionBuilderViewModel = PatternWindows.GetSolutionBuilderViewModel(serviceProvider);
                HandleChanges(SolutionBuilderViewModel.TopLevelNodes);
                RemoveEmptyAddMenuItems(SolutionBuilderViewModel.TopLevelNodes);
            }
        }

        private void HandleChanges(ObservableCollection<IProductElementViewModel> observableCollection)
        {
            observableCollection.CollectionChanged += ProductElementViewModel_CollectionChanged;

            foreach (var item in observableCollection)
            {
                HandleChanges(item.ChildNodes);
            }
        }

        private void UnhandleChanges(ObservableCollection<IProductElementViewModel> observableCollection)
        {
            observableCollection.CollectionChanged -= ProductElementViewModel_CollectionChanged;

            foreach (var item in observableCollection)
            {
                HandleChanges(item.ChildNodes);
            }
        }

        private void ProductElementViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IProductElementViewModel newElement in e.NewItems)
                    {
                        HandleChanges(new ObservableCollection<IProductElementViewModel>(new[] { newElement }));
                        RemoveEmptyAddMenuItems(new ObservableCollection<IProductElementViewModel>(new[] { newElement }));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IProductElementViewModel removedElement in e.OldItems)
                    {
                        UnhandleChanges(new ObservableCollection<IProductElementViewModel>(new[] { removedElement }));
                    }
                    break;
            }
        }

        private void RemoveEmptyAddMenuItems(ObservableCollection<IProductElementViewModel> observableCollection)
        {
            foreach (var item in observableCollection)
            {
                var addMenuItem = item.MenuOptions.FirstOrDefault(x => x.Caption == "Add");
                if (addMenuItem != null &&
                    addMenuItem.MenuOptions.All(x => x.IsEnabled == false ||
                                                     x.Data is ICollectionSchema))
                {
                    item.MenuOptions.Remove(addMenuItem);
                }

                RemoveEmptyAddMenuItems(item.ChildNodes);
            }
        }

    }
}

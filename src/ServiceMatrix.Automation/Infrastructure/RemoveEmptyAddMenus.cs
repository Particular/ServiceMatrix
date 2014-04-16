using Microsoft.VisualStudio.Shell;
using NuPattern.Runtime;
using NuPattern.Runtime.UI.ViewModels;
using NuPattern.VisualStudio.Solution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuPattern;

namespace NServiceBusStudio.Automation.Infrastructure
{
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
            this.Solution = solution;
            this.PatternWindows = patternWindows;

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
                UnhandleChanges(this.SolutionBuilderViewModel.TopLevelNodes);
                this.SolutionBuilderViewModel = null;
            };

            if (this.Solution.IsOpen)
            {
                WireSolution(serviceProvider);
            }
        }

        public void WireSolution(IServiceProvider serviceProvider)
        {
            if (this.SolutionBuilderViewModel == null)
            {
                this.SolutionBuilderViewModel = this.PatternWindows.GetSolutionBuilderViewModel(serviceProvider);
                HandleChanges(this.SolutionBuilderViewModel.TopLevelNodes);
                RemoveEmptyAddMenuItems(this.SolutionBuilderViewModel.TopLevelNodes);
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

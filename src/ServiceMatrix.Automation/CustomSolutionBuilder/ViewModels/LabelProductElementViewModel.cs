using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using NuPattern.Runtime.UI;
using NuPattern.Runtime;
using NuPattern.Runtime.UI.ViewModels;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels
{
    using System.ComponentModel;
    using System.Windows.Input;

    public class LabelElementViewModel : IProductElementViewModel
    {
        public ObservableCollection<IMenuOptionViewModel> MenuOptions { get; set; }

        // Methods
        public LabelElementViewModel(IAbstractElement element, ISolutionBuilderContext ctx)
        {
            Data = element;
            Context = ctx;
            MenuOptions = new ObservableCollection<IMenuOptionViewModel>();
        }

        // Properties
        public string Label { get; set; }

        public void AddMenuOption(IMenuOptionViewModel menuOption)
        {
            MenuOptions.Add(menuOption);
        }

        public string IconPath { get; set;  }

        public bool IsSelected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsEditing
        {
            get { return false; }
        }

        public void AddChildNodes(IEnumerable<IProductElement> elements)
        {
        }

        public string AddNewElement(IPatternElementInfo info)
        {
            return null;
        }

        public ObservableCollection<IProductElementViewModel> ChildNodes
        {
            get { return new ObservableCollection<IProductElementViewModel>(); }
        }

        public ISolutionBuilderContext Context { get; private set; }

        public IProductElement Data { get; private set; }

        public ICommand DeleteCommand
        {
            get { return null; }
        }

        public IElementContainer ElementContainerData
        {
            get { return null; }
        }

        public void EndEdit()
        {
        }

        public bool IsExpanded { get; set; }

        public IProductElementViewModel ParentNode
        {
            get { return null; }
        }

        public void Reorder()
        {
        }

        public void Select(IProductElement element)
        {
        }
    }
}

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
    public class LabelElementViewModel : IProductElementViewModel
    {
        public ObservableCollection<IMenuOptionViewModel> MenuOptions { get; set; }

        // Methods
        public LabelElementViewModel(IAbstractElement element, ISolutionBuilderContext ctx)
        {
            this.MenuOptions = new ObservableCollection<IMenuOptionViewModel>();
        }

        // Properties
        public string Label { get; set; }

        public void AddMenuOption(IMenuOptionViewModel menuOption)
        {
            this.MenuOptions.Add(menuOption);
        }

        public ISolutionBuilderContext ContextViewModel
        {
            get { throw new NotImplementedException(); }
        }

        public string IconPath
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSelected
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IProductElement Model
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.ObjectModel.ObservableCollection<IProductElementViewModel> NodesViewModel
        {
            get { throw new NotImplementedException(); }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        public bool IsEditing
        {
            get { throw new NotImplementedException(); }
        }

        public void AddChildNodes(IEnumerable<IProductElement> elements)
        {
            throw new NotImplementedException();
        }

        public string AddNewElement(IPatternElementInfo info)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<IProductElementViewModel> ChildNodes
        {
            get { throw new NotImplementedException(); }
        }

        public ISolutionBuilderContext Context
        {
            get { throw new NotImplementedException(); }
        }

        public IProductElement Data
        {
            get { throw new NotImplementedException(); }
        }

        public System.Windows.Input.ICommand DeleteCommand
        {
            get { throw new NotImplementedException(); }
        }

        public IElementContainer ElementContainerData
        {
            get { throw new NotImplementedException(); }
        }

        public void EndEdit()
        {
            throw new NotImplementedException();
        }

        public bool IsExpanded
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IProductElementViewModel ParentNode
        {
            get { throw new NotImplementedException(); }
        }

        public void Reorder()
        {
            throw new NotImplementedException();
        }

        public void Select(IProductElement element)
        {
            throw new NotImplementedException();
        }
    }

    public class LabelProductElementViewModel : IProductElementViewModel
    {
        public ObservableCollection<IMenuOptionViewModel> MenuOptions { get; set; }

        // Methods
        public LabelProductElementViewModel(IProduct element, ISolutionBuilderContext ctx)
        {
            this.MenuOptions = new ObservableCollection<IMenuOptionViewModel>();
        }

        // Properties
        public string Label { get; set; }

        public void AddMenuOption(IMenuOptionViewModel menuOption)
        {
            this.MenuOptions.Add(menuOption);
        }

        public ISolutionBuilderContext ContextViewModel
        {
            get { throw new NotImplementedException(); }
        }

        public string IconPath
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSelected
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IProductElement Model
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.ObjectModel.ObservableCollection<IProductElementViewModel> NodesViewModel
        {
            get { throw new NotImplementedException(); }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        public bool IsEditing
        {
            get { throw new NotImplementedException(); }
        }

        public void AddChildNodes(IEnumerable<IProductElement> elements)
        {
            throw new NotImplementedException();
        }

        public string AddNewElement(IPatternElementInfo info)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<IProductElementViewModel> ChildNodes
        {
            get { throw new NotImplementedException(); }
        }

        public ISolutionBuilderContext Context
        {
            get { throw new NotImplementedException(); }
        }

        public IProductElement Data
        {
            get { throw new NotImplementedException(); }
        }

        public System.Windows.Input.ICommand DeleteCommand
        {
            get { throw new NotImplementedException(); }
        }

        public IElementContainer ElementContainerData
        {
            get { throw new NotImplementedException(); }
        }

        public void EndEdit()
        {
            throw new NotImplementedException();
        }

        public bool IsExpanded
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IProductElementViewModel ParentNode
        {
            get { throw new NotImplementedException(); }
        }

        public void Reorder()
        {
            throw new NotImplementedException();
        }

        public void Select(IProductElement element)
        {
            throw new NotImplementedException();
        }
    }

}

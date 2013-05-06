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
        public ObservableCollection<MenuOptionViewModel> MenuOptions { get; set; }

        // Methods
        public LabelElementViewModel(IAbstractElement element, ISolutionBuilderContext ctx)
        {
            this.MenuOptions = new ObservableCollection<MenuOptionViewModel>();
        }

        // Properties
        public string Label { get; set; }

        public void AddMenuOption(MenuOptionViewModel menuOption)
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
    }

    public class LabelProductElementViewModel : IProductElementViewModel
    {
        public ObservableCollection<MenuOptionViewModel> MenuOptions { get; set; }

        // Methods
        public LabelProductElementViewModel(IProduct element, ISolutionBuilderContext ctx)
        {
            this.MenuOptions = new ObservableCollection<MenuOptionViewModel>();
        }

        // Properties
        public string Label { get; set; }

        public void AddMenuOption(MenuOptionViewModel menuOption)
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
    }

}

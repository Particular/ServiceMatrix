using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime.UI.ViewModels;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels
{
    public class TempMenuOptionViewModel: IMenuOptionViewModel
    {
        public TempMenuOptionViewModel(string caption, IEnumerable<IMenuOptionViewModel> items)
        {
            
        }

        public string Caption
        {
            get { throw new NotImplementedException(); }
        }

        public object Data
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

        public int GroupIndex
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

        public bool IsEnabled
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

        public bool IsVisible
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

        public System.Collections.ObjectModel.ObservableCollection<IMenuOptionViewModel> MenuOptions
        {
            get { throw new NotImplementedException(); }
        }

        public long SortOrder
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
    }
}

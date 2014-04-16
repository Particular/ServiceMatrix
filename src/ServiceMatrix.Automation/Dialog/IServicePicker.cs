using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace NServiceBusStudio.Automation.Dialog
{
    public interface IServicePicker
    {
        bool? ShowDialog();

        ObservableCollection<string> Elements { get; set; }
        ICollection<string> SelectedItems { get; set;  }
        string Title { get; set; }
    }
}

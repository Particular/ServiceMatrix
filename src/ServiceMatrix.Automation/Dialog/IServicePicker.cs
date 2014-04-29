namespace NServiceBusStudio.Automation.Dialog
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IServicePicker
    {
        bool? ShowDialog();

        ObservableCollection<string> Elements { get; set; }
        ICollection<string> SelectedItems { get; set;  }
        string Title { get; set; }
    }
}

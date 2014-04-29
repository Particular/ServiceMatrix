namespace NServiceBusStudio.Automation.Dialog
{
    using System.Collections.Generic;

    interface IElementPicker
    {
        bool? ShowDialog();

        ICollection<string> Elements { get; set; }
        string SelectedItem { get; }
        string Title { get; set; }
        string MasterName { get; set; }
    }
}

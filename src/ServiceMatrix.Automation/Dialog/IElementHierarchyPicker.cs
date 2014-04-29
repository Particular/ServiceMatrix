namespace NServiceBusStudio.Automation.Dialog
{
    using System.Collections.Generic;

    interface IElementHierarchyPicker
    {
        string SlaveName { get; set; }
        bool? ShowDialog();

        IDictionary<string, ICollection<string>> Elements { get; set; }
        string SelectedMasterItem { get; }
        string SelectedSlaveItem { get; }
        string Title { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Dialog
{
    interface IElementHierarchyPicker
    {
        string SlaveName { get; set; }
        bool? ShowDialog();

        IDictionary<string, ICollection<string>> Elements { get; set; }
        string SelectedMasterItem { get; }
        string SelectedSlaveItem { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Dialog
{
    interface IElementPicker
    {
        bool? ShowDialog();

        ICollection<string> Elements { get; set; }
        string SelectedItem { get; }
        string Title { get; set; }
        string MasterName { get; set; }
    }
}

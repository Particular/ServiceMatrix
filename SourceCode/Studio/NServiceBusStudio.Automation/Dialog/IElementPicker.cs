using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace NServiceBusStudio.Automation.Dialog
{
    interface IElementPicker
    {
        bool? ShowDialog();

        ICollection<string> Elements { get; set; }
        string SelectedItem { get; }
    }
}

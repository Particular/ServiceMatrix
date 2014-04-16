using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Dialog
{
    public interface IEndpointPicker : IServicePicker
    {
        string ComponentName { get; set; }
    }
}

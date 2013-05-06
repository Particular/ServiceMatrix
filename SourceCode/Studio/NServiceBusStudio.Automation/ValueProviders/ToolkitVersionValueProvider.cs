using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.ValueProviders
{
    public class ToolkitVersionValueProvider : ValueProvider
    {
        public override object Evaluate()
        {
            return ToolkitConstants.Version;
        }
    }
}

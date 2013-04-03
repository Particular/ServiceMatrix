using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

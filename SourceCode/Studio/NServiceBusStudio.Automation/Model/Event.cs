using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;
using NServiceBusStudio.Automation.Infrastructure;
using NServiceBusStudio.Automation.Extensions;

namespace NServiceBusStudio
{
    partial class Event : IRenameRefactoring
    {
        public string Namespace
        {
            get { return this.Parent.Namespace; }
        }
    }
}

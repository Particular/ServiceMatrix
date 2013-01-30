using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using NServiceBusStudio.Automation.Extensions;
using NServiceBusStudio.Automation.Infrastructure;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;
using System.ComponentModel.Composition;

namespace NServiceBusStudio
{
    partial interface IEvents : IProjectReferenced
    {
    }

    partial class Events
    {
        public IProject Project
        {
            get { return this.AsCollection().GetProject(); }
        }

    }
}

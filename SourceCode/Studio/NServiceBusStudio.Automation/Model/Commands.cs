using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using NServiceBusStudio.Automation.Extensions;

namespace NServiceBusStudio
{
    partial interface ICommands : IProjectReferenced
    {
    }

    partial class Commands
    {
        public IProject Project
        {
            get { return this.AsCollection().GetProject(); }
        }
    }
}

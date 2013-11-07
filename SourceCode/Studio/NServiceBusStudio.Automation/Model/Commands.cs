using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Automation.Extensions;
using NuPattern.VisualStudio.Solution;

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

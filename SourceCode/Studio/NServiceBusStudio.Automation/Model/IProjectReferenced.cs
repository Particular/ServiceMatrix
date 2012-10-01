using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;

namespace NServiceBusStudio
{
    public interface IProjectReferenced
    {
        IProject Project { get; }
        string Namespace { get; set; }
    }
}

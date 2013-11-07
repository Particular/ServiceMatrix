using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio
{
    public interface IProjectReferenced
    {
        IProject Project { get; }
        string Namespace { get; set; }
    }
}

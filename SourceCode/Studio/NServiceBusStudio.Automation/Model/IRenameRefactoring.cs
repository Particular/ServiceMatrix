using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBusStudio
{
    public interface IRenameRefactoring
    {
        string Namespace { get; }
        string OriginalInstanceName { get; }
        string InstanceName { get; }
    }
}

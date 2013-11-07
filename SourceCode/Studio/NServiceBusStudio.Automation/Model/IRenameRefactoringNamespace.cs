using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBusStudio
{
    public interface IRenameRefactoringNamespace
    {
        string OriginalInstanceName { get; }
        string InstanceName { get; }
    }
}

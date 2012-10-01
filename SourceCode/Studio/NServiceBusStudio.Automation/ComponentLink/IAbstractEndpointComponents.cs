using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace AbstractEndpoint
{
    public interface IAbstractEndpointComponents : IToolkitInterface
    {
        IAbstractComponentLink CreateComponentLink(string name, Action<IAbstractComponentLink> initializer = null, bool raiseInstantiateEvents = true);
        IEnumerable<IAbstractComponentLink> AbstractComponentLinks { get; }
    }
}

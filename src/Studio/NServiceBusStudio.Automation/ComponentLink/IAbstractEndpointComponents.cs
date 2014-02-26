using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime.ToolkitInterface;

namespace AbstractEndpoint
{
    public interface IAbstractEndpointComponents : IToolkitInterface
    {
        IAbstractEndpoint ParentEndpoint { get; }
        IAbstractComponentLink CreateComponentLink(string name, Action<IAbstractComponentLink> initializer = null, bool raiseInstantiateEvents = true);
        IEnumerable<IAbstractComponentLink> AbstractComponentLinks { get; }
    }
}

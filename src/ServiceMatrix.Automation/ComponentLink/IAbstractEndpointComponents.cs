namespace AbstractEndpoint
{
    using System;
    using System.Collections.Generic;
    using NuPattern.Runtime.ToolkitInterface;

    public interface IAbstractEndpointComponents : IToolkitInterface
    {
        IAbstractEndpoint ParentEndpoint { get; }
        IAbstractComponentLink CreateComponentLink(string name, Action<IAbstractComponentLink> initializer = null, bool raiseInstantiateEvents = true);
        IEnumerable<IAbstractComponentLink> AbstractComponentLinks { get; }
    }
}

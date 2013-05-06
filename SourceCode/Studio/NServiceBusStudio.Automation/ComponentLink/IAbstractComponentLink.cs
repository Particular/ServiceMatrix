using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Core;
using NServiceBusStudio;
using NuPattern.Runtime.ToolkitInterface;

namespace AbstractEndpoint
{
    public interface IAbstractComponentLink : IToolkitInterface
    {
        IElementReference<IComponent> ComponentReference { get; }
        IEnumerable<IAbstractComponentLink> Siblings { get; }
        Int64 Order { get; set; }
        void SetNextOrderNumber();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractEndpoint;

namespace NServiceBusStudio
{
    partial class NServiceBusWebComponents : IAbstractEndpointComponents
    {
        public IAbstractComponentLink CreateComponentLink(string name, Action<IAbstractComponentLink> initializer = null, bool raiseInstantiateEvents = true)
        {
            var result = CreateNServiceBusWebComponentLink(name, new Action<INServiceBusWebComponentLink>(initializer), raiseInstantiateEvents);
            result.SetNextOrderNumber();
            return result;
        }

        public IEnumerable<IAbstractComponentLink> AbstractComponentLinks
        {
            get
            {
                return this.NServiceBusWebComponentLinks.Cast<IAbstractComponentLink>();
            }
        }
    }
}

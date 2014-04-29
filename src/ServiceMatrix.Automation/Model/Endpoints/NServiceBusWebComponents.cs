namespace NServiceBusStudio
{
    using System;
    using System.Collections.Generic;
    using AbstractEndpoint;

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
                return NServiceBusWebComponentLinks;
            }
        }

        public IAbstractEndpoint ParentEndpoint
        {
            get { return Parent; }
        }
    }
}

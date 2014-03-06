using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractEndpoint;

namespace NServiceBusStudio
{
    partial class NServiceBusHostComponents : IAbstractEndpointComponents
    {
        public IAbstractComponentLink CreateComponentLink(string name, Action<IAbstractComponentLink> initializer = null, bool raiseInstantiateEvents = true)
        {
            var result = CreateNServiceBusHostComponentLink(name, new Action<IAbstractComponentLink>(initializer), raiseInstantiateEvents);
            result.SetNextOrderNumber();
            return result;
        }

        public IEnumerable<IAbstractComponentLink> AbstractComponentLinks
        {
            get
            {
                return this.NServiceBusHostComponentLinks.Cast<IAbstractComponentLink>();
            }
        }

        public IAbstractEndpoint ParentEndpoint
        {
            get { return this.Parent as IAbstractEndpoint; }
        }
    }
}

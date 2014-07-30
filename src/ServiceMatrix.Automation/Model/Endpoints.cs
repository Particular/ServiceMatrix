using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractEndpoint;

namespace NServiceBusStudio
{
    partial interface IEndpoints
    {
        IEnumerable<IAbstractEndpoint> GetAll();
    }

    partial class Endpoints
    {
        public IEnumerable<IAbstractEndpoint> GetAll()
        {
            var endpoints = new List<IAbstractEndpoint>();

            endpoints.AddRange(this.NServiceBusHosts);
            endpoints.AddRange(this.NServiceBusMVCs);

            return endpoints;
        }
    }
}

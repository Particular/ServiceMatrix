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
        IEnumerable<INServiceBusMVC> GetMvcEndpoints();

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

        public IEnumerable<INServiceBusMVC> GetMvcEndpoints()
        {
            var endpoints = new List<INServiceBusMVC>();
            endpoints.AddRange(this.NServiceBusMVCs);
            return endpoints;
        }
    }
}

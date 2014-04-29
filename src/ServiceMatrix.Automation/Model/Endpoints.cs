namespace NServiceBusStudio
{
    using System.Collections.Generic;
    using AbstractEndpoint;

    partial interface IEndpoints
    {
        IEnumerable<IAbstractEndpoint> GetAll();
    }

    partial class Endpoints
    {
        public IEnumerable<IAbstractEndpoint> GetAll()
        {
            var endpoints = new List<IAbstractEndpoint>();

            endpoints.AddRange(NServiceBusHosts);
            endpoints.AddRange(NServiceBusWebs);
            endpoints.AddRange(NServiceBusMVCs);

            return endpoints;
        }
    }
}

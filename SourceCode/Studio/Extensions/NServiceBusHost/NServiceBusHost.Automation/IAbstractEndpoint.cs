using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio;
using abs = AbstractEndpoint;

namespace NServiceBusHost
{
    partial interface IAbstractEndpoint : abs.IAbstractEndpoint
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractEndpoint;

namespace NServiceBusStudio
{
    partial interface IUseCaseLink
    {
        IEnumerable<IAbstractEndpoint> Endpoints { get; set; }
        IEnumerable<IComponent> Components { get; set; }
        IEnumerable<ICommand> Commands { get; set; }
        IEnumerable<IEvent> Events { get; set; }
    }

    partial class UseCaseLink
    {
        public IEnumerable<IAbstractEndpoint> Endpoints { get; set; }
        public IEnumerable<IComponent> Components { get; set; }
        public IEnumerable<ICommand> Commands { get; set; }
        public IEnumerable<IEvent> Events { get; set; }
    }
}

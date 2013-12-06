using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio
{
    partial interface ISubscribes
    {
        IProcessedCommandLink CreateLink(ICommand command);
        ISubscribedEventLink CreateLink(IEvent @event);
    }

    partial class Subscribes
    {
        public IProcessedCommandLink CreateLink(ICommand command)
        {
            if (!this.ProcessedCommandLinks.Any(x => x.CommandReference.Value == command))
            {
                return this.CreateProcessedCommandLink(command.InstanceName, p => p.CommandReference.Value = command);
            }

            return null;
        }

        public ISubscribedEventLink CreateLink(IEvent @event)
        {
            if (!this.SubscribedEventLinks.Any(x => x.EventReference.Value == @event))
            {
                return this.CreateSubscribedEventLink(@event.InstanceName, p => p.EventReference.Value = @event);
            }

            return null;
        }
    }
}

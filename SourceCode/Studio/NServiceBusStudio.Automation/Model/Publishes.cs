using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio
{
    partial interface IPublishes
    {
        ICommandLink CreateLink(ICommand command);
        IEventLink CreateLink(IEvent @event);
    }

    partial class Publishes
    {
        public ICommandLink CreateLink(ICommand command)
        {
            if (!this.CommandLinks.Any(x => x.CommandReference.Value == command))
            {
                return this.CreateCommandLink(command.InstanceName, p => p.CommandReference.Value = command);
            }

            return null;
        }

        public IEventLink CreateLink(IEvent @event)
        {
            if (!this.EventLinks.Any(x => x.EventReference.Value == @event))
            {
                return this.CreateEventLink(@event.InstanceName, p => p.EventReference.Value = @event);
            }

            return null;
        }
    }
}

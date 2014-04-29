namespace NServiceBusStudio
{
    using System.Linq;

    partial interface IPublishes
    {
        ICommandLink CreateLink(ICommand command);
        IEventLink CreateLink(IEvent @event);
    }

    partial class Publishes
    {
        public ICommandLink CreateLink(ICommand command)
        {
            if (CommandLinks.All(x => x.CommandReference.Value != command))
            {
                return CreateCommandLink(command.InstanceName, p => p.CommandReference.Value = command);
            }

            return null;
        }

        public IEventLink CreateLink(IEvent @event)
        {
            if (EventLinks.All(x => x.EventReference.Value != @event))
            {
                return CreateEventLink(@event.InstanceName, p => p.EventReference.Value = @event);
            }

            return null;
        }
    }
}

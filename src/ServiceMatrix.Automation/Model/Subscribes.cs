namespace NServiceBusStudio
{
    using System.Linq;

    partial interface ISubscribes
    {
        IProcessedCommandLink CreateLink(ICommand command);
        ISubscribedEventLink CreateLink(IEvent @event);
    }

    partial class Subscribes
    {
        public IProcessedCommandLink CreateLink(ICommand command)
        {
            if (ProcessedCommandLinks.All(x => x.CommandReference.Value != command))
            {
                return CreateProcessedCommandLink(command.InstanceName, p => p.CommandReference.Value = command);
            }

            return null;
        }

        public ISubscribedEventLink CreateLink(IEvent @event)
        {
            if (SubscribedEventLinks.All(x => x.EventReference.Value != @event))
            {
                return CreateSubscribedEventLink(@event.InstanceName, p => p.EventReference.Value = @event);
            }

            return null;
        }
    }
}

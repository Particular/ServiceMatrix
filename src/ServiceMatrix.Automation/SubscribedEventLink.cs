namespace NServiceBusStudio
{
    using System.Linq;
    using NServiceBusStudio.Core;
    using NServiceBusStudio.Automation;

    partial interface ISubscribedEventLink
    {
        IElementReference<IEvent> EventReference { get; }
    }

    partial class SubscribedEventLink
    {
        private ElementReference<IEvent> eventReference;

        public IElementReference<IEvent> EventReference
        {
            get
            {
                return eventReference ??
                    (eventReference = new ElementReference<IEvent>(
                        () => Parent.Parent.Parent.Parent.Parent.Service.SelectMany(s => s.Contract.Events.Event),
                        new PropertyReference<string>(() => EventId, value => EventId = value),
                        new PropertyReference<string>(() => EventName, value => EventName = value)));
            }
        }

        partial void Initialize()
        {
            EventIdChanged += (sender, args) => InstanceName = EventReference.Value == null ? AnyMessageSupport.TextForUI : EventReference.Value.InstanceName;
            if (EventReference.Value == null)
                InstanceName = AnyMessageSupport.TextForUI;
            else
                EventReference.Value.InstanceNameChanged += (sender, args) => EventIdChanged(sender, args);
        }
    }
}

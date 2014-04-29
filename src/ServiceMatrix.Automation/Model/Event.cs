namespace NServiceBusStudio
{
    using System.Linq;

    partial class Event : IRenameRefactoring
    {
        public string Namespace
        {
            get { return Parent.Namespace; }
        }

        partial void Initialize()
        {
            AsElement().Deleting += (sender, eventargs) =>
            {
                // Find Event Links to the deleted Event
                var root = AsElement().Root.As<IApplication>();

                var eventLinks = root.Design.Services.Service.SelectMany(s => s.Components.Component.SelectMany(c => c.Publishes.EventLinks.Where(el => el.EventReference.Value == this))).ToList();
                eventLinks.ForEach(el => el.Delete());

                var subscribedEventLinks = root.Design.Services.Service.SelectMany(s => s.Components.Component.SelectMany(c => c.Subscribes.SubscribedEventLinks.Where(el => el.EventReference.Value == this))).ToList();
                subscribedEventLinks.ForEach(el => el.Delete());
            };
        }
    }
}

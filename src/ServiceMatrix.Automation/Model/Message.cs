namespace NServiceBusStudio
{
    using System.Linq;
    partial class Message : IRenameRefactoring
    {
        public string Namespace
        {
            get { return Parent.Namespace; }
        }

        partial void Initialize()
        {
            AsElement().Deleting += (sender, eventargs) =>
            {
                // Find Message Links to the deleted Message
                var root = AsElement().Root.As<IApplication>();

                var handledMessageLinks = root.Design.Services.Service.SelectMany(s => s.Components.Component.SelectMany(c => c.Subscribes.HandledMessageLinks.Where(hl => hl.MessageReference.Value == this))).ToList();
                handledMessageLinks.ForEach(el => el.Delete());

                var processedCommandLinkReplies = root.Design.Services.Service.SelectMany(s => s.Components.Component.SelectMany(c => c.Subscribes.ProcessedCommandLinks).Where(pcl => pcl.ProcessedCommandLinkReply != null && pcl.ProcessedCommandLinkReply.MessageReference.Value == this).Select(pcl => pcl.ProcessedCommandLinkReply)).ToList();
                processedCommandLinkReplies.ForEach(el => el.Delete());
            };
        }
    }
}

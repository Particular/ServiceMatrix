using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;
using NServiceBusStudio.Automation.Infrastructure;
using NServiceBusStudio.Automation.Extensions;

namespace NServiceBusStudio
{
    partial class Message : IRenameRefactoring
    {
        public string Namespace
        {
            get { return this.Parent.Namespace; }
        }

        partial void Initialize()
        {
            this.AsElement().Deleting += (sender, eventargs) =>
            {
                // Find Message Links to the deleted Message
                var root = this.AsElement().Root.As<IApplication>();

                var handledMessageLinks = root.Design.Services.Service.SelectMany(s => s.Components.Component.SelectMany(c => c.Subscribes.HandledMessageLinks.Where(hl => hl.MessageReference.Value == this))).ToList();
                handledMessageLinks.ForEach(el => el.Delete());

                var processedCommandLinkReplies = root.Design.Services.Service.SelectMany(s => s.Components.Component.SelectMany(c => c.Subscribes.ProcessedCommandLinks).Where(pcl => pcl.ProcessedCommandLinkReply != null && pcl.ProcessedCommandLinkReply.MessageReference.Value == this).Select(pcl => pcl.ProcessedCommandLinkReply)).ToList();
                processedCommandLinkReplies.ForEach(el => el.Delete());
            };
        }
    }
}

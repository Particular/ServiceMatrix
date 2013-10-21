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
    partial class Event : IRenameRefactoring
    {
        public string Namespace
        {
            get { return this.Parent.Namespace; }
        }

        partial void Initialize()
        {
            this.AsElement().Deleting += (sender, eventargs) =>
            {
                // Find Event Links to the deleted Event
                var root = this.AsElement().Root.As<IApplication>();

                var eventLinks = root.Design.Services.Service.SelectMany(s => s.Components.Component.SelectMany(c => c.Publishes.EventLinks.Where(el => el.EventReference.Value == this))).ToList();
                eventLinks.ForEach(el => el.Delete());

                var subscribedEventLinks = root.Design.Services.Service.SelectMany(s => s.Components.Component.SelectMany(c => c.Subscribes.SubscribedEventLinks.Where(el => el.EventReference.Value == this))).ToList();
                subscribedEventLinks.ForEach(el => el.Delete());
            };
        }
    }
}

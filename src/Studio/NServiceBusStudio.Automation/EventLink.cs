using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Core;
using NServiceBusStudio.Automation;

namespace NServiceBusStudio
{
	partial interface IEventLink
	{
		IElementReference<IEvent> EventReference { get; }		
	}

	partial class EventLink
	{
		private ElementReference<IEvent> eventReference;

		public IElementReference<IEvent> EventReference
		{
			get
			{
				return this.eventReference ??
					(this.eventReference = new ElementReference<IEvent>(
						() => this.Parent.Parent.Parent.Parent.Contract.Events.Event,
						new PropertyReference<string>(() => this.EventId, value => this.EventId = value),
						new PropertyReference<string>(() => this.EventName, value => this.EventName = value)));
			}
		}

		partial void Initialize()
		{
			this.EventIdChanged += (sender, args) => this.InstanceName = this.EventReference.Value == null ? AnyMessageSupport.TextForUI : this.EventReference.Value.InstanceName;
			if (this.EventReference.Value == null)
                this.InstanceName = AnyMessageSupport.TextForUI;
            else
                this.EventReference.Value.InstanceNameChanged += (sender, args) => this.EventIdChanged(sender, args);
		}
	}
}

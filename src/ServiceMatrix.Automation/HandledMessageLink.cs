using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Core;
using NuPattern.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio
{
    partial interface IHandledMessageLink
    {
        IElementReference<IMessage> MessageReference { get; }
    }

    partial class HandledMessageLink
    {
        private ElementReference<IMessage> messageReference;

        public IElementReference<IMessage> MessageReference
        {
            get
            {
                return messageReference ??
                    (messageReference = new ElementReference<IMessage>(
                        () => Parent.Parent.Parent.Parent.Contract.Messages.Message,
                        new PropertyReference<string>(() => MessageId, value => MessageId = value),
                        new PropertyReference<string>(() => MessageName, value => MessageName = value)));
            }
        }

        partial void Initialize()
        {
            MessageIdChanged += (sender, args) => InstanceName = MessageReference.Value == null ? "(None)" : MessageReference.Value.InstanceName;
            if (MessageReference.Value == null)
                InstanceName = "(None)";
            else
                MessageReference.Value.InstanceNameChanged += (sender, args) => MessageIdChanged(sender, args);
        }

        public Boolean StartsSaga
        {
            get { return false; }
            set { }
        }
    }
}

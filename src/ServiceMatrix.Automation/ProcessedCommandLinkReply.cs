using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Core;
using NuPattern.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio
{
    partial interface IProcessedCommandLinkReply
    {
        IElementReference<IMessage> MessageReference { get; }
    }

    partial class ProcessedCommandLinkReply
    {
        private ElementReference<IMessage> messageReference;

        public IElementReference<IMessage> MessageReference
        {
            get
            {
                return this.messageReference ??
                    (this.messageReference = new ElementReference<IMessage>(
                        () => this.Parent.Parent.Parent.Parent.Parent.Contract.Messages.Message,
                        new PropertyReference<string>(() => this.MessageId, value => this.MessageId = value),
                        new PropertyReference<string>(() => this.MessageName, value => this.MessageName = value)));
            }
        }

        partial void Initialize()
        {
            this.MessageIdChanged += (sender, args) => this.InstanceName = this.MessageReference.Value == null ? "(None)" : this.MessageReference.Value.InstanceName;
            if (this.MessageReference.Value == null)
                this.InstanceName = "(None)";
            else
                this.MessageReference.Value.InstanceNameChanged += (sender, args) => this.MessageIdChanged(sender, args);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Core;
using Microsoft.VisualStudio.Patterning.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio
{
    partial interface IProcessedCommandLink
    {
        IElementReference<ICommand> CommandReference { get; }
    }

    partial class ProcessedCommandLink
    {
        private ElementReference<ICommand> commandReference;

        public IElementReference<ICommand> CommandReference
        {
            get
            {
                return this.commandReference ??
                    (this.commandReference = new ElementReference<ICommand>(
                        () => this.Parent.Parent.Parent.Parent.Contract.Commands.Command,
                        new PropertyReference<string>(() => this.CommandId, value => this.CommandId = value),
                        new PropertyReference<string>(() => this.CommandName, value => this.CommandName = value)));
            }
        }

        partial void Initialize()
        {
            this.CommandIdChanged += (sender, args) => this.InstanceName = this.CommandReference.Value == null ? "(None)" : this.CommandReference.Value.InstanceName;
            if (this.CommandReference.Value == null)
                this.InstanceName = "(None)";
            else
                this.CommandReference.Value.InstanceNameChanged += (sender, args) => this.CommandIdChanged(sender, args);
        }
    }
}

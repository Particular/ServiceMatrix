namespace NServiceBusStudio
{
    using NServiceBusStudio.Core;

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
                return commandReference ??
                    (commandReference = new ElementReference<ICommand>(
                        () => Parent.Parent.Parent.Parent.Contract.Commands.Command,
                        new PropertyReference<string>(() => CommandId, value => CommandId = value),
                        new PropertyReference<string>(() => CommandName, value => CommandName = value)));
            }
        }

        partial void Initialize()
        {
            CommandIdChanged += (sender, args) => InstanceName = CommandReference.Value == null ? "(None)" : CommandReference.Value.InstanceName;
            if (CommandReference.Value == null)
                InstanceName = "(None)";
            else
                CommandReference.Value.InstanceNameChanged += (sender, args) => CommandIdChanged(sender, args);
        }
    }
}

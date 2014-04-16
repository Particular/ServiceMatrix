using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBusStudio
{
    // INServiceBusMessage
    public interface INServiceBusMessage
    {
        string CodeIdentifier { get; }
    }
    partial interface ICommand : INServiceBusMessage {}
    partial interface IEvent : INServiceBusMessage {}


    // IMessageLink
    public interface IMessageLink 
    { 
        Boolean StartsSaga { get; } 
    }
    partial interface IProcessedCommandLink : IMessageLink { }
    partial interface ISubscribedEventLink : IMessageLink { }
    partial interface IHandledMessageLink : IMessageLink { }

    // Extension Methods
    public static class INServiceBusMessageExtensions
    {
        public static IService GetParentService(this INServiceBusMessage message)
        {
            if (message is ICommand)
            {
                return (message as ICommand).Parent.Parent.Parent;
            }
            else
            {
                return (message as IEvent).Parent.Parent.Parent;
            }
        }

        public static string GetMessageTypeName(this IMessageLink messagelink)
        {
            if (messagelink is IProcessedCommandLink)
            {
                return (messagelink as IProcessedCommandLink).CommandReference.Value.CodeIdentifier;
            }
            else if (messagelink is ISubscribedEventLink)
            {
                var el = messagelink as ISubscribedEventLink;
                return (el == null || el.EventReference == null || el.EventReference.Value == null) ? "object" : el.EventReference.Value.CodeIdentifier;
            }
            else if (messagelink is IHandledMessageLink)
            {
                return (messagelink as IHandledMessageLink).MessageReference.Value.CodeIdentifier;
            }
            else return null;
        }

        public static string GetMessageTypeFullName(this IEventLink eventlink)
        {
             return eventlink.EventReference.Value.Parent.Namespace + "." + eventlink.EventReference.Value.CodeIdentifier;
        }

        public static string GetMessageTypeFullName(this ICommandLink commandlink)
        {
            return commandlink.CommandReference.Value.Parent.Namespace + "." + commandlink.CommandReference.Value.CodeIdentifier;
        }

        public static string GetMessageTypeFullName(this IProcessedCommandLinkReply processedCommandLinkReply)
        {
            return processedCommandLinkReply.MessageReference.Value.Parent.Namespace + "." + processedCommandLinkReply.MessageReference.Value.CodeIdentifier;
        }

        public static string GetMessageTypeFullName(this IHandledMessageLink handledMessageLinkReply)
        {
            return handledMessageLinkReply.MessageReference.Value.Parent.Namespace + "." + handledMessageLinkReply.MessageReference.Value.CodeIdentifier;
        }
    }


}

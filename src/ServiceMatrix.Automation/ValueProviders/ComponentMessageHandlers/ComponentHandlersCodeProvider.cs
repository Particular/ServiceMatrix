﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace NServiceBusStudio.Automation.ValueProviders.ComponentMessageHandlers
{
    using NServiceBusStudio.Automation.Extensions;

    [CLSCompliant(false)]
    [Category("General")]
    [Description("Generate CustomClassBody value and Set Code properties.")]
    [DisplayName("Returns CustomClassBody value and sets AdditionalUsings, Inherits and ClassBody")]
    public class ComponentHandlersCodeProvider : ComponentFromLinkBasedValueProvider
    {
        public override object Evaluate()
        {
            Component.AdditionalUsings = GenerateAddtionalUsings();
            Component.Inherits = GenerateInherits();
            Component.ClassBody = GenerateClassBody();
            Component.InterfaceBody = GenerateInterfaceBody();           
            return GenerateCustomClassBody();
        }

      

        private string GenerateAddtionalUsings()
        {
            // Using should include all the message types namespaces without duplicates
            var sb = new StringBuilder();
            var commandsNamespaces = Component.Subscribes.ProcessedCommandLinks.Select(cl => cl.CommandReference.Value.Parent.Namespace)
                .Union(Component.Publishes.CommandLinks.Select(cl => cl.CommandReference.Value.Parent.Namespace));
            var eventsNamespaces = Component.Subscribes.SubscribedEventLinks
                .Where(l => l.EventReference.Value != null) // Ignore generic message handlers
                .Select(el => el.EventReference.Value.Parent.Namespace);
            var messagesNamespaces = Component.Subscribes.HandledMessageLinks.Select(ml => ml.MessageReference.Value.Parent.Namespace);

            foreach (var ns in commandsNamespaces.Union(eventsNamespaces).Union(messagesNamespaces).Distinct())
            {
                sb.AppendLine("using " + ns + ";");
            }

            if (Component.IsSaga)
            {
                sb.AppendLine("using NServiceBus.Saga;");
            }

            return sb.ToString();
        }

        private IEnumerable<string> typeNames;

        private IEnumerable<string> TypeNames
        {
            get
            {
                if (typeNames == null)
                {
                    var commandsTypes = Component.Subscribes.ProcessedCommandLinks.Select(cl => cl.CommandReference.Value.CodeIdentifier);
                    var eventsTypes = Component.Subscribes.SubscribedEventLinks.Select(el => el.EventReference.Value == null ? "object" : el.EventReference.Value.CodeIdentifier);

                    typeNames = commandsTypes.Union(eventsTypes).Distinct();
                }
                return typeNames;
            }
        }

        private IEnumerable<IMessageLink> messages;

        private IEnumerable<IMessageLink> Messages
        {
            get
            {
                if (messages == null)
                {
                    var commandsTypes = Component.Subscribes.ProcessedCommandLinks.Select(cl => cl as IMessageLink);
                    var eventsTypes = Component.Subscribes.SubscribedEventLinks.Select(el => el as IMessageLink);
                    var messagesTypes = Component.Subscribes.HandledMessageLinks.Select(ml => ml as IMessageLink);
                    messages = commandsTypes.Union(eventsTypes).Union(messagesTypes).Distinct();
                }
                return messages;
            }
        }

        private string GenerateInherits()
        {
            // Iherits from 
            var sb = new StringBuilder();
            var inheritance = new List<string>();

            if (Component.IsSaga)
            {
                inheritance.Add (String.Format("Saga<{0}SagaData>", Component.CodeIdentifier));
            }

            if (Component.Publishes.CommandLinks.Any())
            {
                inheritance.Add(String.Format("I{0}", Component.CodeIdentifier));
                inheritance.Add(String.Format("ServiceMatrix.Shared.INServiceBusComponent"));
            }

            foreach (var message in Messages)
            {
                var definition = message.StartsSaga && Component.IsSaga ? "IAmStartedByMessages" : "IHandleMessages";
                definition += "<" + message.GetMessageTypeName() + ">";
                inheritance.Add(definition);
            }

            if (inheritance.Any())
            {
                sb.AppendFormat (": " + String.Join(", ", inheritance));
            }

            return sb.ToString();
        }

        private string GenerateClassBody()
        {
            var sb = new StringBuilder();
            
            foreach (var typename in TypeNames)
            {
                sb.AppendLine();
                sb.AppendLine("		public void Handle(" + typename + " message)");
                sb.AppendLine("		{");

                if (Component.IsSaga)
                {
                    sb.AppendLine("			// Store message in Saga Data for later use");
                    sb.AppendLine("			this.Data." + typename + " = message;");
                }

                sb.AppendLine("			// Handle message on partial class");
                sb.AppendLine("			this.HandleImplementation(message);");

                if (Component.IsSaga)
                {
                    sb.AppendLine();
                    sb.AppendLine("			// Check if Saga is Completed ");
                    sb.AppendLine("			CheckIfAllMessagesReceived();");
                }

                sb.AppendLine("		}");
            }

            foreach (var typename in TypeNames)
            {
                sb.AppendLine();
                sb.AppendLine("		partial void HandleImplementation(" + typename + " message);");
            }

            foreach (var typename in Component.Publishes.CommandLinks.Select(c => c.CommandReference.Value.CodeIdentifier))
            {
                sb.AppendLine();
                sb.AppendLine("        public void Send(" + typename + " message)");
                sb.AppendLine("        {");
                sb.AppendLine("            Configure" + typename + "(message);");
                sb.AppendLine("            Bus.Send(message);");
                sb.AppendLine("        }");
            }


            foreach (var typename in Component.Publishes.CommandLinks.Select(c => c.CommandReference.Value.CodeIdentifier))
            {
                sb.AppendLine();
                sb.AppendLine("        partial void Configure" + typename + "(" + typename + " message);");
            }


            foreach (var typename in Component.Subscribes.HandledMessageLinks.Select(ml => ml.MessageReference.Value.CodeIdentifier))
            {
                sb.AppendLine();
                sb.AppendLine("        public void Handle(" + typename + " message)");
                sb.AppendLine("        {");

                if (Component.IsSaga)
                {
                    sb.AppendLine("            // Store message in Saga Data for later use");
                    sb.AppendLine("            this.Data." + typename + " = message;");
                }

                sb.AppendLine();
                sb.AppendLine("            // Handle message on partial class");
                sb.AppendLine("            this.HandleImplementation(message);");
                

                if (Component.IsSaga)
                {
                    sb.AppendLine();
                    sb.AppendLine("            // Check if Saga is Completed ");
                    sb.AppendLine("            CheckIfAllMessagesReceived();");
                }

                sb.AppendLine("        }");
            }

            foreach (var typename in Component.Subscribes.HandledMessageLinks.Select(ml => ml.MessageReference.Value.CodeIdentifier))
            {
                sb.AppendLine();
                sb.AppendLine("        partial void HandleImplementation(" + typename + " message);");
            }

            // Check to avoid collision with Saga Bus
            if (!Component.IsSaga)
            {
                sb.AppendLine();
                sb.AppendLine("        public IBus Bus { get; set; }");
            }

            if (Component.IsSaga)
            {
                var allMessages = TypeNames.Union(Component.Subscribes.HandledMessageLinks.Select(ml => ml.MessageReference.Value.CodeIdentifier));

                sb.AppendLine();
                sb.AppendLine("        public void CheckIfAllMessagesReceived()");
                sb.AppendLine("        {");
                sb.AppendLine("            if (" + String.Join(" && ", allMessages.Select(x => "this.Data." + x + " != null")) + ")");
                sb.AppendLine("            {");
                sb.AppendLine("                AllMessagesReceived();");
                sb.AppendLine("            }");
                sb.AppendLine("        }");

                sb.AppendLine();
                sb.AppendLine("        partial void AllMessagesReceived();");
            }

            if (Component.IsSaga)
            {
                sb.AppendLine("     }");
                sb.AppendLine();
                sb.AppendLine("     public partial class " + Component.CodeIdentifier + "SagaData : IContainSagaData");
                sb.AppendLine("     {");
                sb.AppendLine("           public virtual Guid Id { get; set; }");
                sb.AppendLine("           public virtual string Originator { get; set; }");
                sb.AppendLine("           public virtual string OriginalMessageId { get; set; }");
                sb.AppendLine();

                foreach (var typename in TypeNames)
                {
                    sb.AppendLine("           public virtual " + typename + " " + typename + " { get; set; }");
                }
                foreach (var typename in Component.Subscribes.HandledMessageLinks.Select(ml => ml.MessageReference.Value.CodeIdentifier))
                {
                    sb.AppendLine("           public virtual " + typename + " " + typename + " { get; set; }");
                }
            }
            return sb.ToString();
        }

        private string GenerateCustomClassBody()
        {
            var sb = new StringBuilder();
            
            foreach (var typename in TypeNames)
            {
                sb.AppendLine();
                sb.AppendLine("        partial void HandleImplementation(" + typename + " message)");
                sb.AppendLine("        {");
                sb.AppendLine("            // TODO: " + Component.CodeIdentifier + ": Add code to handle the " + typename + " message.");
                sb.AppendLine("            Console.WriteLine(\"" + Component.Parent.Parent.InstanceName + " received \" + message.GetType().Name);");

                foreach (var publishedCommand in Component.Publishes.CommandLinks)
                {
                    var variableName = publishedCommand.CodeIdentifier.LowerCaseFirstCharacter();
                    sb.AppendLine();
                    sb.AppendFormat("            var {0} = new {1}();", variableName, publishedCommand.GetMessageTypeFullName());
                    sb.AppendLine();
                    sb.AppendFormat("            Bus.Send({0});", variableName);
                    sb.AppendLine();
                }

                foreach (var publishedEvent in Component.Publishes.EventLinks)
                {
                    var variableName = publishedEvent.CodeIdentifier.LowerCaseFirstCharacter();
                    sb.AppendLine();
                    sb.AppendFormat("            var {0} = new {1}();", variableName, publishedEvent.GetMessageTypeFullName());
                    sb.AppendLine();
                    sb.AppendFormat("            Bus.Publish({0});", variableName);
                    sb.AppendLine();
                }

                foreach (var processedCommandLink in Component.Subscribes.ProcessedCommandLinks.Where(cl => cl.CommandReference.Value.CodeIdentifier == typename &&
                                                                                                                  cl.ProcessedCommandLinkReply != null))
                {
                    sb.AppendLine();
                    sb.AppendLine("            var response = new " + processedCommandLink.ProcessedCommandLinkReply.GetMessageTypeFullName() + "();");
                    sb.AppendLine("            Bus.Reply(response);");
                }

                sb.AppendLine("        }");
            }

            return sb.ToString();
        }

        private string GenerateInterfaceBody()
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            
            foreach (var typename in Component.Publishes.CommandLinks.Select(c => c.CommandReference.Value.CodeIdentifier))
            {
                sb.AppendLine("        void Send(" + typename + " message);");
            }

            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace NServiceBusStudio.Automation.ValueProviders.ComponentMessageHandlers
{
    [CLSCompliant(false)]
    [Category("General")]
    [Description("Generate CustomClassBody value and Set Code properties.")]
    [DisplayName("Returns CustomClassBody value and sets AdditionalUsings, Inherits and ClassBody")]
    public class ComponentHandlersCodeProvider : ComponentFromLinkBasedValueProvider
    {
        public override object Evaluate()
        {
            this.Component.AdditionalUsings = this.GenerateAddtionalUsings();
            this.Component.Inherits = this.GenerateInherits();
            this.Component.ClassBody = this.GenerateClassBody();
            this.Component.InterfaceBody = this.GenerateInterfaceBody();           
            return this.GenerateCustomClassBody();
        }

      

        private string GenerateAddtionalUsings()
        {
            // Using should include all the message types namespaces without duplicates
            var sb = new StringBuilder();
            var commandsNamespaces = this.Component.Subscribes.ProcessedCommandLinks.Select(cl => cl.CommandReference.Value.Parent.Namespace)
                .Union(this.Component.Publishes.CommandLinks.Select(cl => cl.CommandReference.Value.Parent.Namespace));
            var eventsNamespaces = this.Component.Subscribes.SubscribedEventLinks
                .Where(l => l.EventReference.Value != null) // Ignore generic message handlers
                .Select(el => el.EventReference.Value.Parent.Namespace);
            var messagesNamespaces = this.Component.Subscribes.HandledMessageLinks.Select(ml => ml.MessageReference.Value.Parent.Namespace);

            foreach (var ns in commandsNamespaces.Union(eventsNamespaces).Union(messagesNamespaces).Distinct())
            {
                sb.AppendLine("using " + ns + ";");
            }

            if (this.Component.IsSaga)
            {
                sb.AppendLine("using NServiceBus.Saga;");
            }

            return sb.ToString();
        }

        private IEnumerable<string> typeNames = null;

        private IEnumerable<string> TypeNames
        {
            get
            {
                if (typeNames == null)
                {
                    var commandsTypes = this.Component.Subscribes.ProcessedCommandLinks.Select(cl => cl.CommandReference.Value.CodeIdentifier);
                    var eventsTypes = this.Component.Subscribes.SubscribedEventLinks.Select(el => el.EventReference.Value == null ? "object" : el.EventReference.Value.CodeIdentifier);

                    typeNames = commandsTypes.Union(eventsTypes).Distinct();
                }
                return typeNames;
            }
        }

        private IEnumerable<IMessageLink> messages = null;

        private IEnumerable<IMessageLink> Messages
        {
            get
            {
                if (messages == null)
                {
                    var commandsTypes = this.Component.Subscribes.ProcessedCommandLinks.Select(cl => cl as IMessageLink);
                    var eventsTypes = this.Component.Subscribes.SubscribedEventLinks.Select(el => el as IMessageLink);
                    var messagesTypes = this.Component.Subscribes.HandledMessageLinks.Select(ml => ml as IMessageLink);
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

            if (this.Component.IsSaga)
            {
                inheritance.Add (String.Format("Saga<{0}SagaData>", this.Component.CodeIdentifier));
            }

            if (this.Component.Publishes.CommandLinks.Any())
            {
                inheritance.Add(String.Format("I{0}", this.Component.CodeIdentifier));
                inheritance.Add(String.Format("ServiceMatrix.Shared.INServiceBusComponent"));
            }

            foreach (var message in this.Messages)
            {
                var definition = message.StartsSaga && this.Component.IsSaga ? "IAmStartedByMessages" : "IHandleMessages";
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
            
            foreach (var typename in this.TypeNames)
            {
                sb.AppendLine();
                sb.AppendLine("		public void Handle(" + typename + " message)");
                sb.AppendLine("		{");

                if (this.Component.IsSaga)
                {
                    sb.AppendLine("			// Store message in Saga Data for later use");
                    sb.AppendLine("			this.Data." + typename + " = message;");
                }

                sb.AppendLine("			// Handle message on partial class");
                sb.AppendLine("			this.HandleImplementation(message);");

                if (this.Component.IsSaga)
                {
                    sb.AppendLine();
                    sb.AppendLine("			// Check if Saga is Completed ");
                    sb.AppendLine("			CheckIfAllMessagesReceived();");
                }

                sb.AppendLine("		}");
            }

            foreach (var typename in this.TypeNames)
            {
                sb.AppendLine();
                sb.AppendLine("		partial void HandleImplementation(" + typename + " message);");
            }

            foreach (var typename in this.Component.Publishes.CommandLinks.Select(c => c.CommandReference.Value.CodeIdentifier))
            {
                sb.AppendLine();
                sb.AppendLine("        public void Send(" + typename + " message)");
                sb.AppendLine("        {");
                sb.AppendLine("            Configure" + typename + "(message);");
                sb.AppendLine("            Bus.Send(message);");
                sb.AppendLine("        }");
            }


            foreach (var typename in this.Component.Publishes.CommandLinks.Select(c => c.CommandReference.Value.CodeIdentifier))
            {
                sb.AppendLine();
                sb.AppendLine("        partial void Configure" + typename + "(" + typename + " message);");
            }


            foreach (var typename in this.Component.Subscribes.HandledMessageLinks.Select(ml => ml.MessageReference.Value.CodeIdentifier))
            {
                sb.AppendLine();
                sb.AppendLine("        public void Handle(" + typename + " message)");
                sb.AppendLine("        {");

                if (this.Component.IsSaga)
                {
                    sb.AppendLine("            // Store message in Saga Data for later use");
                    sb.AppendLine("            this.Data." + typename + " = message;");
                }

                sb.AppendLine();
                sb.AppendLine("            // Handle message on partial class");
                sb.AppendLine("            this.HandleImplementation(message);");
                

                if (this.Component.IsSaga)
                {
                    sb.AppendLine();
                    sb.AppendLine("            // Check if Saga is Completed ");
                    sb.AppendLine("            CheckIfAllMessagesReceived();");
                }

                sb.AppendLine("        }");
            }

            foreach (var typename in this.Component.Subscribes.HandledMessageLinks.Select(ml => ml.MessageReference.Value.CodeIdentifier))
            {
                sb.AppendLine();
                sb.AppendLine("        partial void HandleImplementation(" + typename + " message);");
            }

            // Check to avoid collision with Saga Bus
            if (!this.Component.IsSaga)
            {
                sb.AppendLine();
                sb.AppendLine("        public IBus Bus { get; set; }");
            }

            if (this.Component.IsSaga)
            {
                var allMessages = this.TypeNames.Union(this.Component.Subscribes.HandledMessageLinks.Select(ml => ml.MessageReference.Value.CodeIdentifier));

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

            if (this.Component.IsSaga)
            {
                sb.AppendLine("     }");
                sb.AppendLine();
                sb.AppendLine("     public partial class " + this.Component.CodeIdentifier + "SagaData : IContainSagaData");
                sb.AppendLine("     {");
                sb.AppendLine("           public virtual Guid Id { get; set; }");
                sb.AppendLine("           public virtual string Originator { get; set; }");
                sb.AppendLine("           public virtual string OriginalMessageId { get; set; }");
                sb.AppendLine();

                foreach (var typename in this.TypeNames)
                {
                    sb.AppendLine("           public virtual " + typename + " " + typename + " { get; set; }");
                }
                foreach (var typename in this.Component.Subscribes.HandledMessageLinks.Select(ml => ml.MessageReference.Value.CodeIdentifier))
                {
                    sb.AppendLine("           public virtual " + typename + " " + typename + " { get; set; }");
                }
            }
            return sb.ToString();
        }

        private string GenerateCustomClassBody()
        {
            var sb = new StringBuilder();
            
            foreach (var typename in this.TypeNames)
            {
                sb.AppendLine();
                sb.AppendLine("        partial void HandleImplementation(" + typename + " message)");
                sb.AppendLine("        {");
                sb.AppendLine("            // TODO: " + this.Component.CodeIdentifier + ": Add code to handle the " + typename + " message.");
                sb.AppendLine("            Console.WriteLine(\"" + this.Component.Parent.Parent.InstanceName + " received \" + message.GetType().Name);");

                foreach (var publishedCommand in this.Component.Publishes.CommandLinks)
                {
                     sb.AppendLine();
                     sb.AppendLine("            // TODO: Add code inside the lambda to fill in the message properties.");
                     sb.AppendFormat("            Bus.Send<{0}>(m => {{ }});", publishedCommand.GetMessageTypeFullName());
                     sb.AppendLine();
                }

                foreach (var publishedEvent in this.Component.Publishes.EventLinks)
                {
                    sb.AppendLine();
                    sb.AppendLine("            // TODO: Add code inside the lambda to fill in the message properties.");
                    sb.AppendFormat("            Bus.Publish<{0}>(m => {{ }});", publishedEvent.GetMessageTypeFullName());
                    sb.AppendLine();
                }

                foreach (var processedCommandLink in this.Component.Subscribes.ProcessedCommandLinks.Where(cl => cl.CommandReference.Value.CodeIdentifier == typename &&
                                                                                                                  cl.ProcessedCommandLinkReply != null))
                {
                    sb.AppendLine();
                    sb.AppendLine("            // TODO: Add code inside the lambda to fill in the message properties.");
                    sb.AppendFormat("            Bus.Reply<{0}>(m => {{ }});", processedCommandLink.ProcessedCommandLinkReply.GetMessageTypeFullName());
                    sb.AppendLine();
                }

                sb.AppendLine("        }");
            }

            return sb.ToString();
        }

        private string GenerateInterfaceBody()
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            
            foreach (var typename in this.Component.Publishes.CommandLinks.Select(c => c.CommandReference.Value.CodeIdentifier))
            {
                sb.AppendLine("        void Send(" + typename + " message);");
            }

            return sb.ToString();
        }
    }
}

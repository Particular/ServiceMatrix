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

            foreach (var ns in commandsNamespaces.Union(eventsNamespaces).Distinct())
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
                    messages = commandsTypes.Union(eventsTypes).Distinct();
                }
                return messages;
            }
        }

        private string GenerateInherits()
        {
            // Iherits from 
            var sb = new StringBuilder();
            bool isFirst = true;

            if (this.Component.IsSaga)
            {
                sb.AppendFormat(": Saga<{0}SagaData>", this.Component.CodeIdentifier);
                isFirst = false;
            }

            foreach (var message in this.Messages)
            {
                var definition = message.StartsSaga && this.Component.IsSaga ? "IAmStartedByMessages" : "IHandleMessages";
                definition += "<" + message.GetMessageTypeName() + ">";
                if (isFirst)
                {
                    sb.AppendFormat(": {0}", definition);
                    isFirst = false;
                }
                else
                {
                    sb.AppendFormat(", {0}", definition);
                }
            }

            return sb.ToString();
        }

        private string GenerateClassBody()
        {
            var sb = new StringBuilder();
            sb.AppendLine();

            foreach (var typename in this.TypeNames)
            {
                sb.AppendLine("		public void Handle(" + typename + " message)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.HandleImplementation(message);");

                if (this.Component.AutoPublishEvents && this.Component.Publishes.EventLinks.Any())
                {
                    sb.AppendLine();
                    foreach (var publishedEvent in this.Component.Publishes.EventLinks)
                    {
                        if (publishedEvent.EventReference.Value != null)
                        {
                            sb.AppendLine("			this.Bus.Publish<" + publishedEvent.GetMessageTypeFullName() + ">(e => {");
                            sb.AppendLine("			    /* If set properties is required: set \"Auto Publish Event\" to False, and move this line inside HandleImplementation method. */ });");
                            
                        }
                    }
                }

                sb.AppendLine("		}");
                sb.AppendLine();
            }

            foreach (var typename in this.TypeNames)
            {
                sb.AppendLine("		partial void HandleImplementation(" + typename + " message);");
            }

            // Check to avoid collision with Saga Bus
            if (!this.Component.IsSaga)
            {
                sb.AppendLine();
                sb.AppendLine("		public IBus Bus { get; set; }");
            }

            foreach (var typename in this.Component.Publishes.CommandLinks.Select(c => c.CommandReference.Value.CodeIdentifier))
            {
                sb.AppendLine("		public void Send(" + typename + " message)");
                sb.AppendLine("		{");
                sb.AppendLine("			Bus.Send(message);");
                sb.AppendLine("		}");
                sb.AppendLine();
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

                sb.AppendLine("        }");
            }

            foreach (var publishedCommand in this.Component.Publishes.CommandLinks)
            {
                sb.AppendLine();
                if (publishedCommand.CommandReference.Value != null)
                {
                    sb.AppendLine("            // call Send(new " + publishedCommand.GetMessageTypeFullName() + "()); to send a message");
                }

            }

            foreach (var publishedEvent in this.Component.Publishes.EventLinks)
            {
                sb.AppendLine();
                if (publishedEvent.EventReference.Value != null)
                {
                    sb.AppendLine("            // call Bus.Publish<" + publishedEvent.GetMessageTypeFullName() + ">(m => { /* set properties on m in here */ });");
                }
            }

            return sb.ToString();
        }

    }
}

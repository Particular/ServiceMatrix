using System;
using System.Text;
using AbstractEndpoint;
using NServiceBusStudio.Automation.Model;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Extensions
{
    public static class GenerateMessageConventions
    {
        public static string GetMessageConventions(this IProductElement endpoint)
        {
            var generatedConventions = string.Empty;
            try
            {
                var app = endpoint.Root.As<IApplication>();

                var rootNameSpace = string.Empty;
                var applicationName = app.CodeIdentifier;
                var projectNameForInternal = app.ProjectNameInternalMessages;
                var projectNameForContracts = app.ProjectNameContracts;

                var project = endpoint.As<IAbstractEndpoint>().As<IProductElement>().GetProject();
                if (project != null)
                {
                    rootNameSpace = project.Data.RootNamespace;
                }

                generatedConventions =
                    app.TargetNsbVersion == TargetNsbVersion.Version4
                    ? GetMessageConventionsV4(rootNameSpace, applicationName, projectNameForInternal, projectNameForContracts)
                    : GetMessageConventionsV5(rootNameSpace, applicationName, projectNameForInternal, projectNameForContracts);
            }
            catch (Exception ex)
            {
                //TODO: Why are we catching the exception here??
            }

            return generatedConventions;
        }

        public static string GetMessageConventionsV4(string rootNamespace, string applicationName, string projectNameForInternal, string projectNameForContracts)
        {
            var sb = new StringBuilder();
            if (!String.IsNullOrEmpty(rootNamespace))
            {
                sb.AppendLine("namespace " + rootNamespace);
            }

            sb.Append(@"{
    public class MessageConventions : IWantToRunBeforeConfiguration
    {
        public void Init()
        {
            Configure.Instance");
            sb.AppendLine();
            sb.AppendLine("            .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith(\"" +
                applicationName + "." + projectNameForInternal + ".Commands\"))");
            sb.AppendLine("            .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith(\"" +
                applicationName + "." + projectNameForContracts + "\"))");
            sb.AppendLine("            .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith(\"" +
                applicationName + "." + projectNameForInternal + ".Messages\"));");
            sb.Append(@"        }
    }
}
");
            return sb.ToString();
        }

        public static string GetMessageConventionsV5(string rootNamespace, string applicationName, string projectNameForInternal, string projectNameForContracts)
        {
            var sb = new StringBuilder();
            if (!String.IsNullOrEmpty(rootNamespace))
            {
                sb.AppendLine("namespace " + rootNamespace);
            }

            sb.Append(@"{
    public class MessageConventions
    {
        public static void Apply(Configure.ConventionsBuilder conventionsBuilder)
        {
            conventionsBuilder");
            sb.AppendLine();
            sb.AppendLine("                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith(\"" +
                applicationName + "." + projectNameForInternal + ".Commands\"))");
            sb.AppendLine("                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith(\"" +
                applicationName + "." + projectNameForContracts + "\"))");
            sb.AppendLine("                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith(\"" +
                applicationName + "." + projectNameForInternal + ".Messages\"));");
            sb.Append(@"        }
    }
}
");
            return sb.ToString();
        }
    }
}

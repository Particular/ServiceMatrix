using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractEndpoint;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Extensions
{
    public static class GenerateMessageConventions
    {
        public static string GetMessageConventions(this IProductElement endpoint)
        {
            var sb = new StringBuilder();
            try
            {
                var app = endpoint.Root.As<NServiceBusStudio.IApplication>();

                /*
                 namespace <#= namespace #>
                {
                    public class MessageConventions : IWantToRunBeforeConfiguration
                    {
                        public void Init()
                        {
                            Configure.Instance
                                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("<#= app.InternalMessagesProjectNamespace #>"))
                                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("<#= app.ContractsProjectNamespace #>"));
                        }
                    }
                }

                 * */
                var project = endpoint.As<IAbstractEndpoint>().As<IProductElement>().GetProject();
                if (project != null)
                {
                    sb.AppendLine("namespace " + project.Data.RootNamespace);
                }
                sb.Append(@"{
    public class MessageConventions : IWantToRunBeforeConfiguration
    {
        public void Init()
        {
            Configure.Instance");
                sb.AppendLine();
                sb.AppendLine("            .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith(\"" +
                    app.Design.InternalMessagesProject.As<IProductElement>().GetProject().Data.RootNamespace + "\"))");
                sb.AppendLine("            .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith(\"" +
                    app.Design.ContractsProject.As<IProductElement>().GetProject().Data.RootNamespace + "\"));");
                sb.Append(@"        }
    }
}
");
            }
            catch (Exception ex)
            {
            }
            return sb.ToString();
        }

    }
}

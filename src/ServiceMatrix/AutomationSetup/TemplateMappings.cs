using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace NServiceBusStudio.AutomationSetup
{
    static partial class TemplateMappings
    {
        static IDictionary<string, List<Tuple<string, string, string, string>>> Mappings;

        static TemplateMappings()
        {
            SetupMappings();
        }

        public static Tuple<Guid, string, string>[] GetTuplesForBaseTemplate(string template)
        {
            template = template.StartsWith(templateUriPrefix) ? template.Remove(0, templateUriPrefix.Length) : template;

            List<Tuple<string, string, string, string>> entries;
            if (!Mappings.TryGetValue(template, out entries))
            {
                return new Tuple<Guid, string, string>[0];
            }

            return entries
                .Select(t => Tuple.Create(new Guid(t.Item2), t.Item4, t.Item3))
                .ToArray();
        }

        static partial void SetupMappings();

        private static Regex TemplateRegex = new Regex(@"^.*?(?:\.v(?<version>.+))?\.tt$");
        const string templateUriPrefix = @"t4://extension/" + ToolkitConstants.VsixIdentifier;
        const string templateUriTemplate = templateUriPrefix + "{0}";

        private static Tuple<string, string, string, string> CreateTemplateTuple(string template, string guid)
        {
            template = "/" + template.Replace('\\', '/');

            var match = TemplateRegex.Match(template);
            if (!match.Success)
            {
                return null;
            }

            var version = match.Groups["version"].Value;
            return Tuple.Create(
                template.Replace(".v" + version, ""),
                guid,
                string.Format(CultureInfo.InvariantCulture, templateUriTemplate, template),
                "^NSB " + version);
        }
    }
}

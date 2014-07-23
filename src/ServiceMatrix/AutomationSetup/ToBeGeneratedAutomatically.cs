using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace NServiceBusStudio.AutomationSetup
{
    static class TemplateMappings
    {
        static IDictionary<string, List<Tuple<string, string, string, string>>> Mappings;

        static TemplateMappings()
        {
            var templates =
                new[]
                {
                    CreateTemplateTuple(@"\T\T4\Endpoints\NSBH\TransportConfig.v4.tt", "49afd489-a5ff-4743-bfd0-2c4d33e0d6de"),
                    CreateTemplateTuple(@"\T\T4\Endpoints\NSBH\TransportConfig.v5.tt", "c6ba4012-3810-40e8-9169-a7b52bc4b692"),
                };

            Mappings =
                templates
                    .GroupBy(t => t.Item1)
                    .ToDictionary(
                        g => g.Key,
                        g =>
                            g.Select(t => new { T = t, VersionParts = t.Item4.Split('.').Length })
                                .OrderBy(v => v.VersionParts)
                                .Select(v => v.T)
                                .ToList());
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

        private static Regex TemplateRegex = new Regex(@"^.*?(?:\.v(?<version>.+))?\.tt$");
        const string templateUriPrefix = @"t4://extension/" + ToolkitConstants.VsixIdentifier;
        const string templateUriTemplate = templateUriPrefix + "{0}";

        private static Tuple<string, string, string, string> CreateTemplateTuple(string template, string guid)
        {
            template = template.Replace('\\', '/');

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

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Runtime;
using NuPattern.Runtime.UriProviders;
using NuPattern.VisualStudio.Extensions;

namespace NServiceBusStudio.AutomationSetup
{
    public interface ITemplateMappings
    {
        IEnumerable<VersionedTemplateReference> GetMappingsForBaseTemplate(string templateUri);
    }

    [Export(typeof(ITemplateMappings))]
    public class TemplateMappings : ITemplateMappings
    {
        private static readonly ITracer tracer = Tracer.Get<TemplateMappings>();

        IDictionary<string, List<VersionedTemplateReference>> mappings;

        [ImportingConstructor]
        public TemplateMappings(IUriReferenceService uriService)
        {
            var vsixUri = new Uri(VsixExtensionUriProvider.UriSchemeName + "://" + ToolkitConstants.VsixIdentifier);
            var extension = uriService.ResolveUri<IInstalledExtension>(vsixUri);
            if (extension == null)
            {
                tracer.Warn("Cannot find installed extension for toolkit {0}", ToolkitConstants.VsixIdentifier);
            }
            else
            {
                SetupMappings(extension.InstallPath);
            }
        }

        public IEnumerable<VersionedTemplateReference> GetMappingsForBaseTemplate(string templateUri)
        {
            List<VersionedTemplateReference> entries;
            return (mappings != null && mappings.TryGetValue(templateUri, out entries)) ? entries : null;
        }

        private void SetupMappings(string installPath)
        {
            var baseTemplatePath = Path.Combine(installPath, "T", "T4");
            var installPathLength = installPath.Length;
            mappings =
                Directory.EnumerateFiles(baseTemplatePath, "*.tt", SearchOption.AllDirectories)         // examine all .tt files in the extension install folder
                    .Select(f => CreateTemplateReference(f.Substring(installPathLength)))               // parse the relative path as a versioned template file name
                    .Where(t => t != null)                                                              // keep the versioned templates
                    .GroupBy(t => t.BaseTemplateUri)                                                    // group by base template uri
                    .ToDictionary(                                                                      // create a mapping
                        g => g.Key,                                                                     // with the base template uri as key
                        g => g.OrderByDescending(v => v.Version).ToList());                             // and a list of versioned templates ordered by version
        }

        private static Regex TemplateRegex = new Regex(@"^.*\.v(?<version>\d+(?:\.\d+){1,3}).*\.tt$");
        const string templateUriPrefix = TextTemplateUri.UriHostPrefix + ToolkitConstants.VsixIdentifier;
        const string templateUriTemplate = templateUriPrefix + "{0}";

        private static VersionedTemplateReference CreateTemplateReference(string template)
        {
            // Parse the relative template path to see if it contains version information
            // If it doesn't return null
            // If it does, return an object containing the base uri without version information, a version object with the extracted version information,
            // and the uri of the actual template file as a template uri.
            template = template.StartsWith("\\") ? template.Replace('\\', '/') : "/" + template.Replace('\\', '/');

            var match = TemplateRegex.Match(template);
            if (!match.Success)
            {
                return null;
            }

            var versionString = match.Groups["version"].Value;
            Version version;
            if (!Version.TryParse(versionString, out version))
            {
                return null;
            }

            return new VersionedTemplateReference(
                string.Format(CultureInfo.InvariantCulture, templateUriTemplate, template.Replace(".v" + versionString, "")),
                version,
                string.Format(CultureInfo.InvariantCulture, templateUriTemplate, template));
        }
    }

    public class VersionedTemplateReference
    {
        public VersionedTemplateReference(string baseTemplateUri, Version version, string templateUri)
        {
            BaseTemplateUri = baseTemplateUri;
            Version = version;
            TemplateUri = templateUri;
        }

        public string BaseTemplateUri { get; private set; }
        public Version Version { get; private set; }
        public string TemplateUri { get; private set; }
    }
}
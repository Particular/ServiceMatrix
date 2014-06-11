using System;
using System.IO;

namespace NServiceBusStudio.Automation.Util
{
    internal static class ServiceMatrixOverrides
    {
        const string feedSection = "Feed";
        const string nugetFeedUrlKey = "Url";
        const string includePreReleaseKey = "IncludePreRelease";
        const string packagesSection = "Packages";

        static readonly IniReader iniReader;

        static ServiceMatrixOverrides()
        {
            var file = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ServiceMatrix"), "servicematrix.ini");
            iniReader = new IniReader(file);
        }

        public static string GetNugetFeedServiceBaseAddress()
        {
            return ToNullIfEmpty(iniReader.GetValue(feedSection, nugetFeedUrlKey));
        }

        public static bool GetIncludePreReleaseVersions()
        {
            bool includePreRelease;
            return bool.TryParse(iniReader.GetValue(feedSection, includePreReleaseKey), out includePreRelease) && includePreRelease;
        }

        public static string GetNugetPackageVersion(string packageId)
        {
            return ToNullIfEmpty(iniReader.GetValue(packagesSection, packageId));
        }

        static string ToNullIfEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
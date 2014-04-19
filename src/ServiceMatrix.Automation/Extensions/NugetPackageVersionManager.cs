namespace NServiceBusStudio.Automation.Extensions
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using NuGet.VisualStudio;

    public static class NugetPackageVersionManager
    {
        static ConcurrentDictionary<string, string> packageVersionsDictionary = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Clears the list of all installed packages and their versions
        /// </summary>
        public static void ClearCache()
        {
            packageVersionsDictionary.Clear();
        }

        /// <summary>
        /// Returns the currently stored value for the given package. 
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public static string GetVersionFromCacheForPackage(string packageId)
        {
            string version;
            packageVersionsDictionary.TryGetValue(packageId, out version);
            return version;
        }

        /// <summary>
        /// Adds or updates the version in the cache with what's currently installed packages
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="installedPackages"></param>
        public static void UpdateCache(string packageId, IEnumerable<IVsPackageMetadata> installedPackages)
        {
            foreach (var package in installedPackages.Where(package => packageId.Equals(package.Id)))
            {
                packageVersionsDictionary.AddOrUpdate(packageId, package.VersionString, (key, existingValue) => package.VersionString);
                break;
            }
        }
    }
}

namespace NServiceBusStudio.Automation.Extensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using NuGet.VisualStudio;

    public static class NugetPackageVersionManager
    {
        static ConcurrentDictionary<Tuple<int?, string>, string> packageVersionsDictionary = new ConcurrentDictionary<Tuple<int?, string>, string>();

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
            var key = packageVersionsDictionary.Keys.Where(k => k.Item2 == packageId).OrderByDescending(k => k.Item1).FirstOrDefault();
            return key != null ? packageVersionsDictionary[key] : null;
        }

        /// <summary>
        /// Returns the currently stored value for the given package. 
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="majorVersion"></param>
        /// <returns></returns>
        public static string GetVersionFromCacheForPackage(string packageId, int? majorVersion)
        {
            string version;
            packageVersionsDictionary.TryGetValue(Tuple.Create(majorVersion, packageId), out version);
            return version;
        }

        /// <summary>
        /// Adds or updates the version in the cache with what's currently installed packages
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="majorVersion"></param>
        /// <param name="installedPackages"></param>
        public static void UpdateCache(string packageId, int? majorVersion, IEnumerable<IVsPackageMetadata> installedPackages)
        {
            foreach (var package in installedPackages.Where(package => packageId.Equals(package.Id)))
            {
                packageVersionsDictionary.AddOrUpdate(
                    Tuple.Create(majorVersion, packageId),
                    package.VersionString,
                    (key, existingValue) => package.VersionString);
                break;
            }
        }
    }
}

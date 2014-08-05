namespace NServiceBusStudio.Automation.Extensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using NuGet.VisualStudio;
    using NuGetExtensions;

    public static class NugetPackageVersionManager
    {
        static ConcurrentDictionary<Tuple<PackageTargetVersion, string>, string> packageVersionsDictionary =
            new ConcurrentDictionary<Tuple<PackageTargetVersion, string>, string>();

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
        /// <param name="targetVersion"></param>
        /// <returns></returns>
        public static string GetVersionFromCacheForPackage(string packageId, PackageTargetVersion targetVersion)
        {
            string version;
            packageVersionsDictionary.TryGetValue(Tuple.Create(targetVersion, packageId), out version);
            return version;
        }

        /// <summary>
        /// Adds or updates the version in the cache with what's currently installed packages
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="targetVersion"></param>
        /// <param name="installedPackages"></param>
        public static void UpdateCache(string packageId, PackageTargetVersion targetVersion, IEnumerable<IVsPackageMetadata> installedPackages)
        {
            foreach (var package in installedPackages.Where(package => packageId.Equals(package.Id)))
            {
                packageVersionsDictionary.AddOrUpdate(
                    Tuple.Create(targetVersion, packageId),
                    package.VersionString,
                    (key, existingValue) => package.VersionString);
                break;
            }
        }
    }
}

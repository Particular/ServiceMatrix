namespace NServiceBusStudio.Automation.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Xml;
    using System.Xml.Linq;
    using GitVersion;
    using Model;
    using NuGet.VisualStudio;
    using NuPattern;
    using NuPattern.Diagnostics;
    using NuPattern.VisualStudio;
    using NuPattern.VisualStudio.Solution;
    using Util;

    /// <summary>
    /// Extensions to <see cref="ISolution"/> APIs.
    /// </summary>
    public static class SolutionExtensions
    {
        const string officialNugetFeed = "http://packages.nuget.org/api/v2/";

        private static readonly ITracer tracer = Tracer.Get(typeof(SolutionExtensions));

        /// <summary>
        /// Adds the given <paramref name="reference"/> project as a 
        /// project reference to <paramref name="project"/>.
        /// </summary>
        public static void AddReference(this IProject project, IProject reference)
        {
            if (project == null || reference == null)
                return;

            var dteProject = project.As<EnvDTE.Project>();

            if (dteProject == null)
                return;

            var vsProject = dteProject.Object as VSLangProj.VSProject;
            var dteReference = reference.As<EnvDTE.Project>();

            try
            {
                if (vsProject != null && dteReference != null)
                    vsProject.References.AddProject(dteReference);
            }
            catch (COMException)
            {
                // It's already there. Ignore.
            }
        }

        /// <summary>
        /// Removes the given <paramref name="referenceToRemove"/> project as a 
        /// project reference to <paramref name="project"/>.
        /// </summary>
        public static void RemoveReference(this IProject project, IProject referenceToRemove)
        {
            Guard.NotNull(() => referenceToRemove, referenceToRemove);
            RemoveReference(project, referenceToRemove.Name);
        }

        /// <summary>
        /// Removes the given <paramref name="referenceToRemove"/> referent from <paramref name="project"/>.
        /// </summary>
        public static void RemoveReference(this IProject project, string referenceToRemove)
        {
            Guard.NotNull(() => project, project);
            Guard.NotNull(() => referenceToRemove, referenceToRemove);

            var dteProject = project.As<EnvDTE.Project>();

            if (dteProject == null)
                return;

            var vsProject = dteProject.Object as VSLangProj.VSProject;

            if (vsProject != null)
            {
                var reference = vsProject.References.Find(referenceToRemove);
                if (reference != null)
                    reference.Remove();
            }
        }

        /// <summary>
        /// Adds the given <paramref name="referenceLibraryPath"/> dll as a 
        /// project reference to <paramref name="project"/>.
        /// </summary>
        public static void AddReference(this IProject project, string referenceLibraryPath)
        {
            if (project == null || referenceLibraryPath == null)
                return;

            var dteProject = project.As<EnvDTE.Project>();

            if (dteProject == null)
                return;

            var vsProject = dteProject.Object as VSLangProj.VSProject;

            try
            {
                if (vsProject != null)
                    vsProject.References.Add(referenceLibraryPath);
            }
            catch (COMException)
            {
                // It's already there. Ignore.
            }
        }

        public static bool HasReference(this IProject project, string name)
        {
            var dteProject = project.As<EnvDTE.Project>();

            var vsProject = dteProject.Object as VSLangProj.VSProject;
            if (vsProject != null)
            {
                foreach (VSLangProj.Reference reference in vsProject.References)
                {
                    if (reference.Name == name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static void InstallNuGetPackage(this IProject project, IVsPackageInstallerServices vsPackageInstallerServices, IVsPackageInstaller vsPackageInstaller, IStatusBar StatusBar, string packageName, string targetNsbVersion)
        {
            string packageId;
            int? majorVersion;

            GetPackageIdAndMajorVersion(packageName, targetNsbVersion, out packageId, out majorVersion);

            try
            {
                var version = NugetPackageVersionManager.GetVersionFromCacheForPackage(packageId, majorVersion);

                if (!String.IsNullOrEmpty(version))
                {
                    StatusBar.DisplayMessage(String.Format("Installing Package: {0} {1}...", packageId, version));
                    try
                    {
                        InstallNugetPackageForSpecifiedVersion(project, vsPackageInstaller, packageId, version);
                    }
                    catch (Exception installException)
                    {
                        StatusBar.DisplayMessage(String.Format("When attempting to install version {0} of the package {1}, the following error occured: {2}.. Going to now try installing the latest version of Package ...", version, packageId, installException.Message));
                        // There was a problem installing the specified version of the package. Try the installing the latest available package from the source.
                        InstallLatestNugetPackage(project, vsPackageInstallerServices, vsPackageInstaller, packageId, majorVersion);
                    }
                }
                else
                {
                    StatusBar.DisplayMessage(String.Format("Installing the latest version of Package: {0}...", packageId));
                    InstallLatestNugetPackage(project, vsPackageInstallerServices, vsPackageInstaller, packageId, majorVersion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("NuGet Package {0} cannot be installed ({1}).", packageId, ex.Message), ex);
            }
            finally
            {
                StatusBar.DisplayMessage("");
            }
        }

        private static void InstallLatestNugetPackage(IProject project, IVsPackageInstallerServices vsPackageInstallerServices, IVsPackageInstaller vsPackageInstaller, string packageId, int? majorVersion)
        {
            // lookup latest version for the given major (or null), and install that
            var latestVersion = GetLatestVersionForMajor(packageId, majorVersion);

            vsPackageInstaller.InstallPackage(
                ServiceMatrixOverrides.GetNugetFeedServiceBaseAddress() ?? "All",
                project.As<EnvDTE.Project>(),
                packageId,
                latestVersion,
                false);

            // Call the installed packages to get the version that was just installed and cache the version.
            // Packages are needed in case latestVersion is null, 
            var installedPackages = vsPackageInstallerServices.GetInstalledPackages();
            NugetPackageVersionManager.UpdateCache(packageId, majorVersion, installedPackages);
        }

        private static void InstallNugetPackageForSpecifiedVersion(IProject project, IVsPackageInstaller vsPackageInstaller, string packageId, string version)
        {
            vsPackageInstaller.InstallPackage(
                ServiceMatrixOverrides.GetNugetFeedServiceBaseAddress() ?? "All",
                project.As<EnvDTE.Project>(),
                packageId,
                version,
                false);
        }

        static void GetPackageIdAndMajorVersion(string packageName, string targetNsbVersion, out string packageId, out int? majorVersion)
        {
            // default to latest version of package name
            packageId = packageName;
            majorVersion = null;

            switch (packageName)
            {
                case "NServiceBus":
                case "NServiceBus.Interfaces":
                case "NServiceBus.Host":
                case "NServiceBus.Autofac":
                    majorVersion = targetNsbVersion == TargetNsbVersion.Version4 ? 4 : (int?)null;
                    break;

                case "NServiceBus.RabbitMQ":
                case "NServiceBus.SqlServer":
                    majorVersion = targetNsbVersion == TargetNsbVersion.Version4 ? 1 : (int?)null;
                    break;

                case "NServiceBus.Azure.Transports.WindowsAzureStorageQueues":
                case "NServiceBus.Azure.Transports.WindowsAzureServiceBus":
                    majorVersion = targetNsbVersion == TargetNsbVersion.Version4 ? 5 : (int?)null;
                    break;

                case "ServiceControl.Plugin.DebugSession":
                case "ServiceControl.Plugin.Heartbeat":
                case "ServiceControl.Plugin.CustomChecks":
                case "ServiceControl.Plugin.SagaAudit":
                    // create the package id for the target version
                    packageId =
                        "ServiceControl.Plugin."
                        + (targetNsbVersion == TargetNsbVersion.Version4 ? "" : "Nsb5.")
                        + packageName.Substring("ServiceControl.Plugin.".Length, packageName.Length - "ServiceControl.Plugin.".Length);
                    majorVersion = targetNsbVersion == TargetNsbVersion.Version4 ? 1 : (int?)null;
                    break;
            }
        }

        static string GetLatestVersionForMajor(string packageId, int? majorVersion)
        {
            // first check for overrides
            var overridenPackageVersion = ServiceMatrixOverrides.GetNugetPackageVersion(packageId);
            if (!string.IsNullOrEmpty(overridenPackageVersion))
            {
                return overridenPackageVersion;
            }

            // if no major is given, use latest
            if (!majorVersion.HasValue)
            {
                return null;
            }

            using (var client = new WebClient())
            {
                client.BaseAddress = GetNugetFeedServiceBaseAddress();

                var query =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        @"Packages()?$filter=Id eq '{0}' and startswith(Version,'{1}.')&$orderby=Version desc",
                        packageId,
                        majorVersion.Value);

                string resultString;
                try
                {
                    resultString = client.DownloadString(query);
                }
                catch (WebException e)
                {
                    tracer.Trace(TraceEventType.Warning, e, "Could not retrieve result from feed for package {0}", packageId);
                    return null;
                }

                if (string.IsNullOrWhiteSpace(resultString))
                {
                    tracer.Trace(TraceEventType.Warning, null, "Retrieved empty result from feed for package {0}", packageId);
                    return null;
                }

                XElement xmlElement;
                try
                {
                    xmlElement = XElement.Parse(resultString);
                }
                catch (XmlException e)
                {
                    tracer.Trace(TraceEventType.Warning, e, "Retrieved invalid result from feed for package {0}", packageId);
                    return null;
                }

                var versionElementName = XName.Get("Version", "http://schemas.microsoft.com/ado/2007/08/dataservices");

                var versions =
                    xmlElement
                        .Descendants(versionElementName)
                        .Select(e => new { e.Value, Version = ParseVersion(e.Value) })
                        .OrderByDescending(v => v.Version);
                var version = versions.FirstOrDefault(v => v.Version != null);

                return version != null ? version.Value : null;
            }
        }

        static string GetNugetFeedServiceBaseAddress()
        {
            var overrideFeed = ServiceMatrixOverrides.GetNugetFeedServiceBaseAddress();
            return string.IsNullOrWhiteSpace(overrideFeed) ? officialNugetFeed : overrideFeed.Trim();
        }

        static SemanticVersion ParseVersion(string versionString)
        {
            SemanticVersion version;
            return SemanticVersion.TryParse(versionString, out version) ? version : null;
        }
    }
}

namespace NServiceBusStudio.Automation.Extensions
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using Model;
    using Properties;
    using NuGet.VisualStudio;
    using NuGetExtensions;
    using NuPattern;
    using NuPattern.Diagnostics;
    using NuPattern.VisualStudio;
    using NuPattern.VisualStudio.Solution;

    /// <summary>
    /// Extensions to <see cref="ISolution"/> APIs.
    /// </summary>
    public static class SolutionExtensions
    {
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

        public static void InstallNuGetPackage(
            this IProject project,
            IVsPackageInstallerServices vsPackageInstallerServices,
            IVsPackageInstaller vsPackageInstaller,
            IStatusBar StatusBar,
            INuGetVersionHelper nuGetVersionHelper,
            string packageId,
            string targetNsbVersion)
        {
            var targetVersion = GetPackageIdAndMajorVersion(packageId, targetNsbVersion);

            try
            {
                var version = NugetPackageVersionManager.GetVersionFromCacheForPackage(packageId, targetVersion);

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
                        InstallLatestNugetPackage(project, vsPackageInstallerServices, vsPackageInstaller, nuGetVersionHelper, packageId, targetVersion);
                    }
                }
                else
                {
                    StatusBar.DisplayMessage(String.Format("Installing the latest version of Package: {0}...", packageId));
                    InstallLatestNugetPackage(project, vsPackageInstallerServices, vsPackageInstaller, nuGetVersionHelper, packageId, targetVersion);
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

        static void InstallLatestNugetPackage(
            IProject project,
            IVsPackageInstallerServices vsPackageInstallerServices,
            IVsPackageInstaller vsPackageInstaller,
            INuGetVersionHelper nuGetVersionHelper,
            string packageId,
            PackageTargetVersion targetVersion)
        {
            // lookup latest version for the given major (or null), and install that
            var latestVersion = GetLatestVersionForMajor(nuGetVersionHelper, packageId, targetVersion);

            vsPackageInstaller.InstallPackage(
                "All",
                project.As<EnvDTE.Project>(),
                packageId,
                latestVersion,
                false);

            // Call the installed packages to get the version that was just installed and cache the version.
            // Packages are needed in case latestVersion is null, 
            var installedPackages = vsPackageInstallerServices.GetInstalledPackages();
            NugetPackageVersionManager.UpdateCache(packageId, targetVersion, installedPackages);
        }

        static void InstallNugetPackageForSpecifiedVersion(IProject project, IVsPackageInstaller vsPackageInstaller, string packageId, string version)
        {
            vsPackageInstaller.InstallPackage(
                "All",
                project.As<EnvDTE.Project>(),
                packageId,
                version,
                false);
        }

        static PackageTargetVersion GetPackageIdAndMajorVersion(string packageName, string targetNsbVersion)
        {
            switch (packageName)
            {
                case "NServiceBus":
                case "NServiceBus.Interfaces":
                case "NServiceBus.Host":
                case "NServiceBus.Autofac":
                    return targetNsbVersion == TargetNsbVersion.Version4
                        ? new PackageTargetVersion(4, false)
                        : new PackageTargetVersion(5, true);

                case "NServiceBus.RabbitMQ":
                case "NServiceBus.SqlServer":
                    return targetNsbVersion == TargetNsbVersion.Version4
                        ? new PackageTargetVersion(1, false)
                        : new PackageTargetVersion(null, true);

                case "NServiceBus.Azure.Transports.WindowsAzureStorageQueues":
                case "NServiceBus.Azure.Transports.WindowsAzureServiceBus":
                    return targetNsbVersion == TargetNsbVersion.Version4
                        ? new PackageTargetVersion(5, false)
                        : new PackageTargetVersion(null, true);
            }

            return new PackageTargetVersion(null, false);
        }

        static string GetLatestVersionForMajor(INuGetVersionHelper nuGetVersionHelper, string packageId, PackageTargetVersion targetVersion)
        {
            string version;

            try
            {
                version = nuGetVersionHelper.GetPackageVersion(packageId, targetVersion);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.NugetExceptionRetrievingPackageVersion, packageId, e.Message),
                    e);
            }

            if (string.IsNullOrEmpty(version))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.NugetNoPackageVersion, packageId, targetVersion));
            }

            return version;
        }
    }
}

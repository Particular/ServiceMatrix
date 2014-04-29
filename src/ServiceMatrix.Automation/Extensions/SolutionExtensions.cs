namespace NServiceBusStudio.Automation.Extensions
{
    using System;
    using System.Runtime.InteropServices;
    using EnvDTE;
    using NuPattern.VisualStudio.Solution;
    using NuPattern;
    using NuGet.VisualStudio;
    using NuPattern.VisualStudio;
    using System.Linq;
    using VSLangProj;

    /// <summary>
    /// Extensions to <see cref="ISolution"/> APIs.
    /// </summary>
    public static class SolutionExtensions
    {

        /// <summary>
        /// Adds the given <paramref name="reference"/> project as a 
        /// project reference to <paramref name="project"/>.
        /// </summary>
        public static void AddReference(this IProject project, IProject reference)
        {
            if (project == null || reference == null)
                return;

            var dteProject = project.As<Project>();

            if (dteProject == null)
                return;

            var vsProject = dteProject.Object as VSProject;
            var dteReference = reference.As<Project>();

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

            var dteProject = project.As<Project>();

            if (dteProject == null)
                return;

            var vsProject = dteProject.Object as VSProject;

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

            var dteProject = project.As<Project>();

            if (dteProject == null)
                return;

            var vsProject = dteProject.Object as VSProject;

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
            var dteProject = project.As<Project>();

            var vsProject = dteProject.Object as VSProject;

            foreach (var reference in vsProject.References)
            {
                if ((reference as Reference).Name == name)
                    return true;
            }
            return false;
        }

        public static void InstallNuGetPackage(this IProject project, IVsPackageInstallerServices vsPackageInstallerServices, IVsPackageInstaller vsPackageInstaller, IStatusBar StatusBar, string packageName)
        {
            try
            {
                var version = NugetPackageVersionManager.GetVersionFromCacheForPackage(packageName); 
                
                if (!String.IsNullOrEmpty(version))
                {
                    StatusBar.DisplayMessage(String.Format("Installing Package: {0} {1}...", packageName, version));
                    try
                    {
                        InstallNugetPackageForSpecifiedVersion(project, vsPackageInstaller, packageName, version);
                    }
                    catch (Exception installException)
                    {
                        StatusBar.DisplayMessage(String.Format("When attempting to install version {0} of the package {1}, the following error occured: {2}.. Going to now try installing the latest version of Package ...", version, packageName, installException.Message));
                        // There was a problem installing the specified version of the package. Try the installing the latest available package from the source.
                        InstallLatestNugetPackage(project, vsPackageInstallerServices, vsPackageInstaller, packageName);
                    }
                }
                else
                {
                    StatusBar.DisplayMessage(String.Format("Installing the latest version of Package: {0}...", packageName));
                    InstallLatestNugetPackage(project, vsPackageInstallerServices, vsPackageInstaller, packageName);
                }
                StatusBar.DisplayMessage("");
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("NuGet Package {0} cannot be installed ({1}).", packageName, ex.Message), ex);
            }
        }

        private static void InstallLatestNugetPackage(IProject project,IVsPackageInstallerServices vsPackageInstallerServices, IVsPackageInstaller vsPackageInstaller, string packageName)
        {
            vsPackageInstaller.InstallPackage("All",
                 project.As<Project>(),
                 packageName,
                 (Version)null,
                 false);

            // Call the installed packages to get the version that was just installed and cache the version.
            var installedPackages = vsPackageInstallerServices.GetInstalledPackages();
            NugetPackageVersionManager.UpdateCache(packageName, installedPackages);
           
        }

        private static void InstallNugetPackageForSpecifiedVersion(IProject project, IVsPackageInstaller vsPackageInstaller, string packageName, string version)
        {
            vsPackageInstaller.InstallPackage("All",
                 project.As<Project>(),
                 packageName,
                 version,
                 false);
        }


        
        private static object Log(string type, string log)
        {
            return "";
        }

    }
}

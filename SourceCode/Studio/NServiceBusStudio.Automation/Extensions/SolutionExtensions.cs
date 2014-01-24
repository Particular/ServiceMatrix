using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using NuPattern.VisualStudio.Solution;
using NuPattern;
using NuGet.VisualStudio;

namespace NServiceBusStudio.Automation.Extensions
{
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

            foreach (var reference in vsProject.References)
            {
                if ((reference as VSLangProj.Reference).Name == name)
                    return true;
            }
            return false;
        }

        

        public static void InstallNuGetPackage(this IProject project, IVsPackageInstaller VsPackageInstaller, string packageName)
        {
            try
            {
                VsPackageInstaller.InstallPackage("All",
                                                  project.As<EnvDTE.Project>(),
                                                  packageName,
                                                  default(Version),
                                                  false);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("NuGet Package {0} cannot be installed ({1}).", packageName, ex.Message), ex);
            }
        }

        
        private static object Log(string type, string log)
        {
            return "";
        }

    }
}

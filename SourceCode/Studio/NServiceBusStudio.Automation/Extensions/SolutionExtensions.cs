using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using System.Runtime.InteropServices;
using System.IO;

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
            Guard.NotNull(() => project, project);
            Guard.NotNull(() => referenceToRemove, referenceToRemove);

            var dteProject = project.As<EnvDTE.Project>();

            if (dteProject == null)
                return;

            var vsProject = dteProject.Object as VSLangProj.VSProject;

            if (vsProject != null)
            {
                var reference = vsProject.References.Find(referenceToRemove.Name);
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

        public static string DownloadNuGetPackages(this IProject infraproject)
        {
            var basePath = infraproject.PhysicalPath;
            var solutionPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(basePath));
            var packageConfig = Path.Combine(Path.GetDirectoryName(basePath), "packages.config");
            var packageSources = string.Empty;

            try
            {
                var pi = new System.Diagnostics.ProcessStartInfo(Path.Combine(solutionPath, @".nuget\nuget.exe"))
                {
                    Arguments = string.Format(" install \"{0}\" -source \"{1}\" -o \"{2}\""
                    , packageConfig
                    , packageSources
                    , Path.Combine(solutionPath, @"packages")),
                    CreateNoWindow = true,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                };
                var p = System.Diagnostics.Process.Start(pi);
                p.WaitForExit(120 * 1000);
            }
            catch { }
            return solutionPath;
        }

    }
}

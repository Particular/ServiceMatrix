using System.IO;
using System.Linq;
using NuGet;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio.Automation.NuGetExtensions
{
    public class NuGetVersionHelper : INuGetVersionHelper
    {
        IPackageRepository repository;

        private NuGetVersionHelper(IProject project)
        {
            var projectDirectory = Path.GetDirectoryName(project.PhysicalPath);
            var fileSystem = new PhysicalFileSystem(projectDirectory);
            var settings = Settings.LoadDefaultSettings(fileSystem, null, new MachineWideSettings());
            var packageSourceProvider = new PackageSourceProvider(settings);
            var repoFactory = new PackageRepositoryFactory();
            // This would get an aggregate if there's more than one active feed, but a single repo if there is a single feed
            // Lookups on single repos are optimized when possible
            // However you need to deal with exceptions yourself
            repository = AggregateRepository.Create(repoFactory, packageSourceProvider.GetEnabledPackageSources().ToArray(), true);
        }

        public static NuGetVersionHelper CreateHelperFor(IProject project)
        {
            return new NuGetVersionHelper(project);
        }

        public string GetPackageVersion(string packageId, PackageTargetVersion targetVersion)
        {
            IPackage package;

            var targetVersionSpec = targetVersion.GetVersionSpec();

            if (targetVersionSpec == null)
            {
                // This specific lookup can be resolved very efficiently with a service call
                // on specific repos
                package = repository.FindPackage(packageId, default(SemanticVersion), targetVersion.AllowPreRelease, false);
            }
            else
            {
                package = repository.FindPackage(packageId, targetVersionSpec, targetVersion.AllowPreRelease, false);
            }

            return package != null ? package.Version.ToString() : null;
        }
    }
}

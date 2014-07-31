using System.IO;
using NuGet;
using NuPattern.VisualStudio.Solution;
using System.Linq;

namespace NServiceBusStudio.Automation.NuGetExtensions
{
    public class NuGetVersionHelper : INuGetVersionHelper
    {
        IPackageRepository repository;

        public NuGetVersionHelper(IProject project)
        {
            var projectDirectory = Path.GetDirectoryName(project.PhysicalPath);
            var fileSystem = new PhysicalFileSystem(projectDirectory);
            var settings = Settings.LoadDefaultSettings(fileSystem, null, new MachineWideSettings());
            var packageSourceProvider = new PackageSourceProvider(settings);
            var repoFactory = new PackageRepositoryFactory();
            repository = packageSourceProvider.CreateAggregateRepository(repoFactory, true);
            //// TODO This would get an aggregate if there's more than one active feed, but a single repo if there is a single feed
            //// Lookups on single repos are optimized when possible
            //// However you need to deal with exceptions yourself
            // repository = AggregateRepository.Create(repoFactory, packageSourceProvider.GetEnabledPackageSources().ToArray(), true);
        }

        public string GetPackageVersion(string packageId, int? majorVersion)
        {
            return GetPackageVersion(packageId, majorVersion, true);
        }

        public string GetPackageVersion(string packageId, int? majorVersion, bool allowPreRelease)
        {
            IPackage package;

            if (!majorVersion.HasValue)
            {
                // This specific lookup can be resolved very efficiently with a service call
                // on specific repos
                package = repository.FindPackage(packageId, default(SemanticVersion), allowPreRelease, false);
            }
            else
            {
                // pre-release versions are compared as smaller than release versions, 
                // so using "*.0.0.0" as boundaries doesn't work as expected
                // i.e. using 5.0 as a max version if you request 4.* will allow 5.0.0-beta1
                var versionSpec =
                    new VersionSpec
                    {
                        MaxVersion = new SemanticVersion(majorVersion.Value, int.MaxValue, int.MaxValue, int.MaxValue),
                        IsMaxInclusive = true,
                        MinVersion = new SemanticVersion(majorVersion.Value - 1, int.MaxValue, int.MaxValue, int.MaxValue),
                        IsMinInclusive = false
                    };

                package = repository.FindPackage(packageId, versionSpec, allowPreRelease, false);
            }

            return package != null ? package.Version.ToString() : null;
        }
    }
}

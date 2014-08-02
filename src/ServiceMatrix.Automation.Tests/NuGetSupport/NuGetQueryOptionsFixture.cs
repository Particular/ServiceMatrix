using System;
using System.Collections.Generic;
using System.Linq;
using NuGet;
using NUnit.Framework;

namespace ServiceMatrix.Automation.Tests.NuGetSupport
{
    using System.IO;

    [TestFixture]
    public class NuGetQueryOptionsFixture
    {
        [Test]
        public void FeedCanBeQueriedById()
        {
            var fileSystem = new PhysicalFileSystem(Path.Combine(Environment.CurrentDirectory, "NuGetSupport"));
            var settings =
                Settings.LoadDefaultSettings(
                    fileSystem,
                    "NuGet.SingleSource.Config",
                    new MachineWideSettings());
            var packageSourceProvider = new PackageSourceProvider(settings);
            var repoFactory = new PackageRepositoryFactory();
            var repo = AggregateRepository.Create(repoFactory, packageSourceProvider.GetEnabledPackageSources().ToArray(), true);

            var package =
                repo.GetPackages()
                    .Where(p => p.Id == "NServiceBus")
                    .OrderByDescending(p => p.Version)
                    .FirstOrDefault();

            Assert.IsNotNull(package);
        }

        [Test]
        public void FeedCanBeQueriedByIdAndOtherProperties()
        {
            var fileSystem = new PhysicalFileSystem(Path.Combine(Environment.CurrentDirectory, "NuGetSupport"));
            var settings =
                Settings.LoadDefaultSettings(
                    fileSystem,
                    "NuGet.SingleSource.Config",
                    new MachineWideSettings());
            var packageSourceProvider = new PackageSourceProvider(settings);
            var repoFactory = new PackageRepositoryFactory();
            var repo = AggregateRepository.Create(repoFactory, packageSourceProvider.GetEnabledPackageSources().ToArray(), true);

            var package =
                repo.GetPackages()
                    .Where(p => p.Id == "NServiceBus" && p.Copyright.Contains("2012"))  // IQueryable, query resolved server side
                    .OrderByDescending(p => p.Version)
                    .FirstOrDefault();

            Assert.IsNotNull(package);
        }

        [Test]
        public void FeedCannotBeQueriedUsingVersion()
        {
            var fileSystem = new PhysicalFileSystem(Path.Combine(Environment.CurrentDirectory, "NuGetSupport"));
            var settings =
                Settings.LoadDefaultSettings(
                    fileSystem,
                    "NuGet.SingleSource.Config",
                    new MachineWideSettings());
            var packageSourceProvider = new PackageSourceProvider(settings);
            var repoFactory = new PackageRepositoryFactory();
            var repo = AggregateRepository.Create(repoFactory, packageSourceProvider.GetEnabledPackageSources().ToArray(), true);

            var package =
                repo.GetPackages()
                    .Where(p => p.Id == "NServiceBus" && p.Copyright.Contains("2012"))
                    .OrderByDescending(p => p.Version)
                    .FirstOrDefault();

            Assert.IsNotNull(package);

            var version = package.Version;

            package =
                repo.GetPackages()
                    .Where(p => p.Id == "NServiceBus")
                    .AsEnumerable()                         // can query by version in memory
                    .Where(p => p.Version.Equals(version))
                    .OrderByDescending(p => p.Version)
                    .FirstOrDefault();

            Assert.IsNotNull(package);
            Assert.AreEqual(version, package.Version);

            Assert.Throws<InvalidOperationException>(
                () =>
                    repo.GetPackages()
                        .Where(p => p.Id == "NServiceBus")
                        .Where(p => p.Version.Equals(version))  // but the odata query translation cannot deal with the version 
                        .OrderByDescending(p => p.Version)
                        .FirstOrDefault());

        }

        [Test]
        public void QueryingFeedWithVersionRangeCanResultInUnexpectedResults()
        {
            var fileSystem = new PhysicalFileSystem(Path.Combine(Environment.CurrentDirectory, "NuGetSupport"));
            var settings =
                Settings.LoadDefaultSettings(
                    fileSystem,
                    "NuGet.SingleSource.Config",
                    new MachineWideSettings());
            var packageSourceProvider = new PackageSourceProvider(settings);
            var repoFactory = new PackageRepositoryFactory();
            var repo = AggregateRepository.Create(repoFactory, packageSourceProvider.GetEnabledPackageSources().ToArray(), true);

            var package =
                repo.FindPackage(           // range query - extension method, with version comparison performed in memory as you'd do yourself
                    "NServiceBus",
                    new VersionSpec
                    {
                        MaxVersion = new SemanticVersion(5, 0, 0, 0),
                        IsMaxInclusive = false
                    },
                    true,                   // allow pre releae
                    false);

            // will succeed assumming 5.0 beta is in the nuget repo
            // pre-relase versions are considered smaller than release versions
            // so 5.0 beta is smaller than 5.0.0.0
            // Because of this we need to compare agains 4.max.max.max
            Assert.AreEqual(5, package.Version.Version.Major);
        }

        private class MachineWideSettings : IMachineWideSettings
        {
            Lazy<IEnumerable<Settings>> settings;

            public MachineWideSettings()
            {
                var baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                settings = new Lazy<IEnumerable<Settings>>(() => NuGet.Settings.LoadMachineWideSettings(new PhysicalFileSystem(baseDirectory)));
            }

            public IEnumerable<Settings> Settings
            {
                get { return settings.Value; }
            }
        }
    }
}

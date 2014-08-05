using System;
using System.Globalization;
using NuGet;

namespace NServiceBusStudio.Automation.NuGetExtensions
{
    public class PackageTargetVersion : IEquatable<PackageTargetVersion>
    {
        public PackageTargetVersion(int? majorVersion, bool allowPreRelease)
        {
            MajorVersion = majorVersion;
            AllowPreRelease = allowPreRelease;
        }

        public int? MajorVersion { get; set; }

        public bool AllowPreRelease { get; private set; }

        public bool Equals(PackageTargetVersion other)
        {
            return (AllowPreRelease == other.AllowPreRelease)
                   && (MajorVersion == other.MajorVersion);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PackageTargetVersion;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            return MajorVersion != null ? MajorVersion.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, AllowPreRelease ? "major version {0} (pre release)" : "major version {0}", MajorVersion);
        }

        internal IVersionSpec GetVersionSpec()
        {
            // pre-release versions are compared as smaller than release versions, 
            // so using "*.0.0.0" as boundaries doesn't work as expected
            // i.e. using 5.0 as a max version if you request 4.* will allow 5.0.0-beta1
            return MajorVersion.HasValue
                ? new VersionSpec
                    {
                        MinVersion = new SemanticVersion(MajorVersion.Value - 1, int.MaxValue, int.MaxValue, int.MaxValue),
                        IsMinInclusive = false,
                        MaxVersion = new SemanticVersion(MajorVersion.Value, int.MaxValue, int.MaxValue, int.MaxValue),
                        IsMaxInclusive = true,
                    }
                : null;

        }
    }
}

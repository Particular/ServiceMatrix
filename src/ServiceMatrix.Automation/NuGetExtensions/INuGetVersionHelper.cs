namespace NServiceBusStudio.Automation.NuGetExtensions
{
    public interface INuGetVersionHelper
    {
        string GetPackageVersion(string packageId, int? majorVersion);
        string GetPackageVersion(string packageId, int? majorVersion, bool allowPreRelease);
    }
}

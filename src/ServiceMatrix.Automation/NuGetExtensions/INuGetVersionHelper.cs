namespace NServiceBusStudio.Automation.NuGetExtensions
{
    public interface INuGetVersionHelper
    {
        string GetPackageVersion(string packageId, PackageTargetVersion targetVersion);
    }
}

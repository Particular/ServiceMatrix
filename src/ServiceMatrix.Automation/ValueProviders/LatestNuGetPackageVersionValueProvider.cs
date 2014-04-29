namespace NServiceBusStudio.Automation.ValueProviders
{
    using NuPattern.Runtime;
    using System;
    using System.ComponentModel;

    [CLSCompliant(false)]
    [Category("General")]
    [Description("Get Latest NuGet package version for a package")]
    // This is no longer needed!
    public class LatestNuGetPackageVersionValueProvider : ValueProvider
    {
        public string PackageName { get; set; }

        public override object Evaluate()
        {
            return null;
        }
    }
}

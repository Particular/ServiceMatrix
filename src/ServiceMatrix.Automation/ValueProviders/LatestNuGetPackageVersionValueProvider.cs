using Newtonsoft.Json;
using NuPattern.Runtime;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;

namespace NServiceBusStudio.Automation.ValueProviders
{
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

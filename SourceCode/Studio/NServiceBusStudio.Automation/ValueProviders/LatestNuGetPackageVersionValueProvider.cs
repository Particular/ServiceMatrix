using Newtonsoft.Json;
using NuPattern.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.ValueProviders
{
    [CLSCompliant(false)]
    [Category("General")]
    [Description("Get Latest NuGet package version for a package")]
    public class LatestNuGetPackageVersionValueProvider : ValueProvider
    {
        public string PackageName { get; set; }

        public override object Evaluate()
        {
            var uri = String.Format("https://nuget.org/api/v2/package-versions/{0}", PackageName);

            return GetHTTPGetLatestPackageVersion(uri);
        }

        private string GetHTTPGetLatestPackageVersion(string regeditUrl)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(regeditUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var stringResponse = response.Content.ReadAsStringAsync().Result;
                        var versions = JsonConvert.DeserializeObject<string[]>(stringResponse);

                        return versions.Last();
                    }
                }
            }
            catch { }

            return "1.0.0";
        }
    }
}

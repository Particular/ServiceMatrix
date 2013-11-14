using Microsoft.Win32;
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
    [Description("Get ServiceControlUri based on current configuration")]
    public class ServiceControlInstanceURIValueProvider : ValueProvider
    {
        public override object Evaluate()
        {
            var defaultUri = "http://localhost:33333/api";

            if (CheckHTTPGetUri(defaultUri))
            {
                return defaultUri;
            }

            var regeditPort = 0;
            using (var baseKey = RegistryKey.OpenBaseKey (RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var registryKey = baseKey.OpenSubKey(@"Software\ParticularSoftware\ServiceControl"))
                {
                    if (registryKey != null)
                    {
                        regeditPort = (int)registryKey.GetValue("Port", 0);
                    }
                }
            
            if (regeditPort != 0)
            {
                var regeditUrl = String.Format("http://localhost:{0}/api", regeditPort);
                if (CheckHTTPGetUri(regeditUrl))
                {
                    return regeditUrl;
                }
            }


            return "";

        }

        private bool CheckHTTPGetUri(string regeditUrl)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(regeditUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            catch { }

            return false;
        }
    }
}

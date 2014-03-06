using System;
using System.Configuration;
using System.IO;
using Microsoft.Win32;

namespace NServiceBusStudio.Automation.Licensing
{
    public class LicenseDescriptor
    {
        public static string OldLocalLicenseFile
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ParticularSoftware\Studio\License.xml");
            }
        }

        public static string LocalLicenseFile
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ParticularSoftware\Studio\License.xml");
            }
        }

        public static string HKCULicense
        {
            get
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey(String.Format(@"SOFTWARE\ParticularSoftware\Studio\{0}", LicenseManager.SoftwareVersion.ToString(2))))
                {
                    if (registryKey != null)
                    {
                        return (string)registryKey.GetValue("License", null);
                    }
                }

                return null;
            }
        }

        public static string HKLMLicense
        {
            get
            {
                try
                {
                    using (var registryKey = Registry.LocalMachine.OpenSubKey(String.Format(@"SOFTWARE\ParticularSoftware\Studio\{0}", LicenseManager.SoftwareVersion.ToString(2))))
                    {
                        if (registryKey != null)
                        {
                            return (string)registryKey.GetValue("License", null);
                        }
                    }
                }
                catch (Exception)
                {
                    //Swallow exception if we can't read HKLM
                }

                return null;
            }
        }

        public static string AppConfigLicenseFile
        {
            get { return ConfigurationManager.AppSettings["ParticularSoftware/Studio/LicensePath"]; }
        }

        public static string AppConfigLicenseString
        {
            get { return ConfigurationManager.AppSettings["ParticularSoftware/Studio/License"]; }
        }

        public static string PublicKey
        {
            get
            {
                return @"<RSAKeyValue><Modulus>qbQY2F8SX4Degq/lpSMzFwrQBS+YaK99TlEy52EMjKJybQ/ZwwnwqLUHZwwZ6GJ1NuQG6QZS4PGIWuejlFQ6aVT3Tzxk/VM7Jzl5CQAuzdehMcFWUgv0nyp6/Bsx9p9TWR/5gGSd0MLqrb2mR++fhxe1jJBTuS4Q3EmxbZCiFm0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            }
        }
    }
}
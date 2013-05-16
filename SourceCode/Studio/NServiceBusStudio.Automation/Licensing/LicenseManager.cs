using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

using Rhino.Licensing;
using NuPattern.Diagnostics;

namespace NServiceBusStudio.Automation.Licensing
{
    public class LicenseManager
    {
        private const string LicenseTypeKey = "LicenseType";
        private const string LicenseVersionKey = "LicenseVersion";

        private const int TRIAL_DAYS = 30;
        private const int TRIAL_NOTIFICATION_DAYS = 10;

        private static readonly ITracer tracer = Tracer.Get<LicenseManager>();
        public static readonly Version SoftwareVersion = GetStudioVersion();

        private License license;
        private bool trialPeriodHasExpired;
        private AbstractLicenseValidator validator;

        static LicenseManager instance;

        private LicenseManager()
        {
            validator = CreateValidator();

            Validate();
        }

        private LicenseManager(string licenseText)
        {
            validator = CreateValidator(licenseText);

            Validate();
        }

        /// <summary>
        /// Initializes the licensing system with the given license
        /// </summary>
        /// <param name="licenseText"></param>
        public static void Parse(string licenseText)
        {
            instance = new LicenseManager(licenseText);
        }

        /// <summary>
        ///     Get current NServiceBus licensing information
        /// </summary>
        public static License CurrentLicense
        {
            get
            {
                if (instance == null)
                    instance = new LicenseManager();

                return instance.license;
            }
        }

        /// <summary>
        /// Prompts the users if their trial license has expired
        /// </summary>
        public static void PromptUserForLicense()
        {
            if (instance == null)
                instance = new LicenseManager();

            instance.PromptUserForLicenseInternal();
        }

        private void PromptUserForLicenseInternal()
        {
            if (license.LicenseType == LicenseType.Standard ||
                (license.LicenseType == LicenseType.Trial &&
                (license.ExpirationDate - DateTime.Now).TotalDays >= TRIAL_NOTIFICATION_DAYS))
            {
                return;
            }

            //prompt user for license file
            using (var form = new TrialExpired())
            {
                form.CurrentLicenseExpireDate = license.ExpirationDate;

                form.ValidateLicenseFile = (f, s) =>
                {
                    StringLicenseValidator licenseValidator = null;

                    try
                    {
                        string selectedLicenseText = ReadAllTextWithoutLocking(s);
                        licenseValidator = new StringLicenseValidator(LicenseDescriptor.PublicKey,
                                                                        selectedLicenseText);
                        licenseValidator.AssertValidLicense();

                        using (var registryKey = Registry.CurrentUser.CreateSubKey(String.Format(@"SOFTWARE\ParticularSoftware\Studio\{0}", SoftwareVersion.ToString(2))))
                        {
                            if (registryKey == null)
                            {
                                return false;
                            }

                            registryKey.SetValue("License", selectedLicenseText, RegistryValueKind.String);
                        }

                        return true;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        tracer.Warn("Could not write to the registry.", ex);
                        f.DisplayError();
                    }
                    catch (LicenseExpiredException)
                    {
                        if (licenseValidator != null)
                        {
                            f.DisplayExpiredLicenseError(licenseValidator.ExpirationDate);
                        }
                        else
                        {
                            f.DisplayError();
                        }
                    }
                    catch (Exception)
                    {
                        f.DisplayError();
                    }

                    return false;
                };

                if (trialPeriodHasExpired)
                {
                    form.ExpiredTrialVersion();
                }
                else
                {
                    form.TrialVersion((license.ExpirationDate - DateTime.Now).Days);
                }

                //if user specifies a valid license file then run with that license
                validator = CreateValidator();
                Validate();


                if (license.LicenseType == LicenseType.Trial &&
                    trialPeriodHasExpired)
                {
                    throw new LicenseExpiredException();
                }
            }
        }

        internal static string ReadAllTextWithoutLocking(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var textReader = new StreamReader(fileStream))
            {
                return textReader.ReadToEnd();
            }
        }

        private void Validate()
        {
            if (validator != null)
            {
                try
                {
                    validator.AssertValidLicense();

                    tracer.Info((string)"Found a {0} license.", (object)validator.LicenseType);
                    tracer.Info((string)"Registered to {0}", (object)validator.Name);
                    tracer.Info((string)"Expires on {0}", (object)validator.ExpirationDate);
                    
                    CheckIfStudioVersionIsNewerThanLicenseVersion();

                    ConfigureStudioLicense();

                    return;
                }
                catch (LicenseExpiredException)
                {
                    trialPeriodHasExpired = true;
                    tracer.Error("License has expired.");
                }
                catch (LicenseNotFoundException)
                {
                    tracer.Error("License could not be loaded.");
                }
                catch (LicenseFileNotFoundException)
                {
                    tracer.Error("License could not be loaded.");
                }

                return;
            }

            tracer.Info("No valid license found.");
            ConfigureStudioToRunInTrialMode();
        }

        private void ConfigureStudioToRunInTrialMode()
        {
            var trialExpirationDate = DateTime.UtcNow.Date;

            var windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity != null && windowsIdentity.User != null &&
                !windowsIdentity.User.IsWellKnown(WellKnownSidType.LocalSystemSid))
            {
                //If first time run, configure expire date
                try
                {
                    string trialStartDateString;
                    DateTime trialStartDate = default(DateTime);

                    using (var registryKey = Registry.CurrentUser.CreateSubKey(String.Format(@"SOFTWARE\ParticularSoftware\Studio\{0}", SoftwareVersion.ToString(2))))
                    {
                        if (registryKey == null)
                        {
                            tracer.Warn("Falling back to run in Trial license mode.");

                            trialPeriodHasExpired = true;

                            //if trial expired, run in Basic1
                            RunInTrialMode(trialExpirationDate);
                        }

                        if ((trialStartDateString = (string)registryKey.GetValue("TrialStart", null)) == null)
                        {
                            trialStartDate = DateTime.UtcNow;
                            trialStartDateString = trialStartDate.ToString("yyyy-MM-dd");
                            registryKey.SetValue("TrialStart", trialStartDateString, RegistryValueKind.String);

                            tracer.Info("First time running NServiceBusStudio v{0}, setting trial license start.",
                                               SoftwareVersion.ToString(2));
                        }
                        else if (!DateTime.TryParseExact(trialStartDateString, "yyyy-MM-dd",
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.AssumeUniversal,
                                                   out trialStartDate))
                        {
                            trialStartDate = DateTime.UtcNow;
                            trialStartDateString = trialStartDate.ToString("yyyy-MM-dd");
                            registryKey.SetValue("TrialStart", trialStartDateString, RegistryValueKind.String);

                            tracer.Info("Wrong Trial Start date for NServiceBusStudio v{0}, setting trial license start to today.",
                                               SoftwareVersion.ToString(2));
                        }
                    }

                    trialExpirationDate = trialStartDate.Date.AddDays(TRIAL_DAYS);
                }
                catch (UnauthorizedAccessException ex)
                {
                    tracer.Info("Could not write to the registry. Because we didn't find a license file we assume the trial has expired.", ex);
                }
            }

            //Check trial is still valid
            if (trialExpirationDate > DateTime.UtcNow.Date)
            {
                tracer.Info("Trial for NServiceBus v{0} is still active, trial expires on {1}.",
                                   SoftwareVersion.ToString(2), trialExpirationDate.ToLocalTime().ToShortDateString());
                tracer.Info("Configuring NServiceBus to run in trial mode.");

                //Run in trial period
                RunInTrialMode(trialExpirationDate);
            }
            else
            {
                tracer.Info("Trial for NServiceBus v{0} has expired.", SoftwareVersion.ToString(2));
                tracer.Warn("Falling back to run in Basic1 license mode.");

                // Not Run, trial period expired
                trialPeriodHasExpired = true;
                RunInTrialMode(trialExpirationDate);
            }
        }

        private void RunInTrialMode(DateTime trialExpirationDate)
        {
            license = new License
            {
                LicenseType = LicenseType.Trial,
                ExpirationDate = trialExpirationDate
            };
        }

        private void RunInStandardMode(DateTime expirationDate)
        {
            license = new License
            {
                LicenseType = LicenseType.Standard,
                ExpirationDate = expirationDate
            };
        }

        private static AbstractLicenseValidator CreateValidator(string licenseText = "")
        {
            if (!String.IsNullOrEmpty(licenseText))
            {
                tracer.Info(@"Using license supplied via fluent API.");
                return new StringLicenseValidator(LicenseDescriptor.PublicKey, licenseText);
            }

            if (!String.IsNullOrEmpty(LicenseDescriptor.AppConfigLicenseString))
            {
                tracer.Info(@"Using embedded license supplied via config file AppSettings/ParticularSoftware/Studio/License.");
                licenseText = LicenseDescriptor.AppConfigLicenseString;
            }
            else if (!String.IsNullOrEmpty(LicenseDescriptor.AppConfigLicenseFile))
            {
                if (File.Exists(LicenseDescriptor.AppConfigLicenseFile))
                {
                    tracer.Info(@"Using license supplied via config file AppSettings/ParticularSoftware/Studio/LicensePath ({0}).", LicenseDescriptor.AppConfigLicenseFile);
                    licenseText = ReadAllTextWithoutLocking(LicenseDescriptor.AppConfigLicenseFile);
                }
            }
            else if (File.Exists(LicenseDescriptor.LocalLicenseFile))
            {
                tracer.Info(@"Using license in current folder ({0}).", LicenseDescriptor.LocalLicenseFile);
                licenseText = ReadAllTextWithoutLocking(LicenseDescriptor.LocalLicenseFile);
            }
            else if (File.Exists(LicenseDescriptor.OldLocalLicenseFile))
            {
                tracer.Info(@"Using license in current folder ({0}).", LicenseDescriptor.OldLocalLicenseFile);
                licenseText = ReadAllTextWithoutLocking(LicenseDescriptor.OldLocalLicenseFile);
            }
            else if (!String.IsNullOrEmpty(LicenseDescriptor.HKCULicense))
            {
                tracer.Info(@"Using embedded license found in registry [HKEY_CURRENT_USER\SOFTWARE\ParticularSoftware\Studio\{0}\License].", SoftwareVersion.ToString(2));
                licenseText = LicenseDescriptor.HKCULicense;
            }
            else if (!String.IsNullOrEmpty(LicenseDescriptor.HKLMLicense))
            {
                tracer.Info(@"Using embedded license found in registry [HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\Studio\{0}\License].", SoftwareVersion.ToString(2));
                licenseText = LicenseDescriptor.HKLMLicense;
            }

            return String.IsNullOrEmpty(licenseText) ? null : new StringLicenseValidator(LicenseDescriptor.PublicKey, licenseText);
        }

        //if NServiceBus version > license version, throw an exception
        private void CheckIfStudioVersionIsNewerThanLicenseVersion()
        {
            if (validator.LicenseType == Rhino.Licensing.LicenseType.None)
                return;

            if (validator.LicenseAttributes.ContainsKey(LicenseVersionKey))
            {
                try
                {
                    Version semver = GetStudioVersion();
                    Version licenseVersion = Version.Parse(validator.LicenseAttributes[LicenseVersionKey]);
                    if (licenseVersion >= semver)
                        return;
                }
                catch (Exception exception)
                {
                    throw new ConfigurationErrorsException(
                        "Your license is valid for an older version of NServiceBus. If you are still within the 1 year upgrade protection period of your original license, you should have already received a new license and if you havent, please contact customer.care@nservicebus.com. If your upgrade protection has lapsed, you can renew it at http://www.nservicebus.com/PurchaseSupport.aspx.",
                        exception);
                }
            }

            throw new ConfigurationErrorsException(
                "Your license is valid for an older version of NServiceBus. If you are still within the 1 year upgrade protection period of your original license, you should have already received a new license and if you havent, please contact customer.care@nservicebus.com. If your upgrade protection has lapsed, you can renew it at http://www.nservicebus.com/PurchaseSupport.aspx.");
        }

        private static Version GetStudioVersion()
        {
            Version assembyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            return new Version(assembyVersion.Major, assembyVersion.Minor);
        }

        /// <summary>
        ///     Set NSeriviceBus license information.
        /// </summary>
        private void ConfigureStudioLicense()
        {
            license = new License();

            switch (validator.LicenseType)
            {
                case Rhino.Licensing.LicenseType.Standard:
                    SetLicenseType(LicenseType.Standard);
                    break;
                case Rhino.Licensing.LicenseType.Trial:
                    SetLicenseType(LicenseType.Trial);
                    break;
                default:
                    tracer.Error((string)"Got unexpected license type [{0}], setting trial license type.",
                                       (object)validator.LicenseType.ToString());
                    license.LicenseType = LicenseType.Trial;
                    break;
            }

            license.ExpirationDate = validator.ExpirationDate;
        }

        private void SetLicenseType(string defaultLicenseType)
        {
            if ((validator.LicenseAttributes == null) ||
                (!validator.LicenseAttributes.ContainsKey(LicenseTypeKey)) ||
                (string.IsNullOrEmpty(validator.LicenseAttributes[LicenseTypeKey])))
            {
                license.LicenseType = defaultLicenseType;
            }
            else
            {
                license.LicenseType = validator.LicenseAttributes[LicenseTypeKey];
            }
        }
    }
}

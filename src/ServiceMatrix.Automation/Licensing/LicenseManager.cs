namespace NServiceBusStudio.Automation.Licensing
{
    using NuPattern.Diagnostics;
    using Particular.Licensing;

    public class LicenseManager
    {
        static License license;
        static string licenseText;
        static readonly ITracer tracer = Tracer.Get<LicenseManager>();
        RegistryLicenseStore licenseStore = new RegistryLicenseStore();

        static readonly LicenseManager instance = new LicenseManager();

        private LicenseManager()
        {
            if (license == null)
            {
                InitializeLicense();
            }
        }

        public static LicenseManager Instance
        {
            get
            {
                return instance;
            }
        }

        public bool HasLicenseExpired()
        {
            return license == null || LicenseExpirationChecker.HasLicenseExpired(license);
        }

        public bool PromptUserForLicenseIfTrialHasExpired()
        {
            if (license == null || LicenseExpirationChecker.HasLicenseExpired(license))
            {
                var trialExpiredDlg = new TrialExpired
                {
                    CurrentLicense = license
                };
                trialExpiredDlg.ShowDialog();
                if (trialExpiredDlg.ResultingLicenseText == null)
                {
                    return false;
                }
                new RegistryLicenseStore()
                    .StoreLicense(trialExpiredDlg.ResultingLicenseText);
                license = LicenseDeserializer.Deserialize(trialExpiredDlg.ResultingLicenseText);
                return true;
            }
            return true;
        }

        License GetTrialLicense()
        {
            if (UserSidChecker.IsNotSystemSid())
            {
                var trialStartDate = TrialStartDateStore.GetTrialStartDate();
                var trialLicense = License.TrialLicense(trialStartDate);

                //Check trial is still valid
                if (LicenseExpirationChecker.HasLicenseExpired(trialLicense))
                {
                    tracer.Warn("Trial for the Particular Service Platform has expired");
                }
                else
                {
                    var message = string.Format("Trial for Particular Service Platform is still active, trial expires on {0}. Configuring NServiceBus to run in trial mode.", trialLicense.ExpirationDate.Value.ToShortDateString());
                    tracer.Info(message);
                }
                return trialLicense;
            }

            tracer.Error("Could not access registry for the current user sid. Please ensure that the license has been properly installed.");
            return null;
        }

        void InitializeLicense()
        {
            //only do this if not been configured by the fluent API
            if (licenseText == null)
            {
                //look in the new platform locations
                if (!(new RegistryLicenseStore().TryReadLicense(out licenseText)))
                {
                    //TODO: Check legacy locations for Service Matrix.
                    if (string.IsNullOrWhiteSpace(licenseText))
                    {
                        license = GetTrialLicense();
                        return;
                    }
                }
            }

            LicenseVerifier.Verify(licenseText);

            var foundLicense = LicenseDeserializer.Deserialize(licenseText);

            if (LicenseExpirationChecker.HasLicenseExpired(foundLicense))
            {
                tracer.Error("You can renew it at http://particular.net/licensing.");
                return;
            }

            if (foundLicense.UpgradeProtectionExpiration != null)
            {
                tracer.Info(string.Format("UpgradeProtectionExpiration: {0}", foundLicense.UpgradeProtectionExpiration));
            }
            else
            {
                tracer.Info(string.Format("Expires on {0}", foundLicense.ExpirationDate));
            }

            license = foundLicense;
        }
    }
}

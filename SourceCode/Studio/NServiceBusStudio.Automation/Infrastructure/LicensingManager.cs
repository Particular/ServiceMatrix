using NServiceBusStudio.Automation.Dialog;
using NServiceBusStudio.Automation.Infrastructure.Licensing;
using NuPattern.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace NServiceBusStudio.Automation.Infrastructure
{
    [Export]
    public class LicensingManager
    {
        [Required]
        [Import(AllowDefault = true)]
        private IDialogWindowFactory WindowFactory
        {
            get;
            set;
        }

        public LicensingManager()
        {
        }

        public void CheckLicense(bool isCreating)
        {
            var validLicense = false;
            var expiredTrialVersion = true;
            var trialVersionDaysLeft = 10;

            if (!validLicense)
            {
                var dialog = WindowFactory.CreateDialog<LicensingDialog>() as LicensingDialog;

                if (expiredTrialVersion)
                {
                    dialog.ExpiredTrialVersion();
                    dialog.ShowDialog();

                    if (isCreating)
                    {
                        throw new Exception("You cannot create new NServiceBus solutions using an expired trial version. Please purchase a license.");
                    }
                }
                else
                {
                    dialog.TrialVersion(trialVersionDaysLeft);
                    dialog.ShowDialog();
                }
            }
        }
    }
}

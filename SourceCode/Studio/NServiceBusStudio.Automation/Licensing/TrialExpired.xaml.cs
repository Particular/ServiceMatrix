using NuPattern.Presentation;
using NuPattern.Runtime;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace NServiceBusStudio.Automation.Licensing
{
    /// <summary>
    /// Interaction logic for ComponentPicker.xaml
    /// </summary>
    public partial class TrialExpired : CommonDialogWindow, IDialogWindow, IDisposable
    {
        public TrialExpired()
        {
            InitializeComponent();
        }

        public DateTime CurrentLicenseExpireDate { get; set; }

        public Func<TrialExpired, string, bool> ValidateLicenseFile { get; set; }

        public void TrialVersion(int daysleft)
        {
            this.Title = String.Format("ServiceMatrix - Trial version - {0} days left", daysleft.ToString());
            this.TrialVersionDaysLeft.Text = daysleft.ToString();

            this.TrialVersionPane.Visibility = System.Windows.Visibility.Visible;
            this.ExpiredTrialVersionPane.Visibility = System.Windows.Visibility.Collapsed;
            this.ShowDialog();
        }

        public void ExpiredTrialVersion()
        {
            this.Title = "ServiceMatrix - Expired Trial version";

            this.TrialVersionPane.Visibility = System.Windows.Visibility.Collapsed;
            this.ExpiredTrialVersionPane.Visibility = System.Windows.Visibility.Visible;
            this.ShowDialog();
        }

        public void DisplayError()
        {
            errorPanel.Visibility = System.Windows.Visibility.Visible;
            errorMessageLabel.Text = "The file you have selected is not valid.";
            selectedFileExpirationDateLabel.Text = String.Empty;
        }

        public void DisplayExpiredLicenseError(DateTime expirationDate)
        {
            errorPanel.Visibility = System.Windows.Visibility.Visible;
            errorMessageLabel.Text = "The license file you have selected is expired.";
            selectedFileExpirationDateLabel.Text = String.Format("Expiration Date: {0}", expirationDate.ToLocalTime().ToShortDateString());
        }

        private void PurchaseALicense_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://particular.net/licensing"));
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var close = false;

            using (var openDialog = new OpenFileDialog())
            {
                openDialog.InitializeLifetimeService();
                openDialog.Filter = "License files (*.xml)|*.xml|All files (*.*)|*.*";
                openDialog.Title = "Select License file";

                if (ShowDialogInSTA(openDialog) == System.Windows.Forms.DialogResult.OK)
                {
                    string licenseFileSelected = openDialog.FileName;

                    if (ValidateLicenseFile(this, licenseFileSelected))
                    {
                        close = true;
                    }
                }
            }

            if (close)
            {
                this.DialogResult = true;
                this.Close();

                var registered = new Registered();
                registered.ShowDialog();
            }
        }

        //When calling a OpenFileDialog it needs to be done from an STA thread model otherwise it throws:
        //"Current thread must be set to single thread apartment (STA) mode before OLE calls can be made. Ensure that your Main function has STAThreadAttribute marked on it. This exception is only raised if a debugger is attached to the process."
        private static DialogResult ShowDialogInSTA(FileDialog dialog)
        {
            var result = System.Windows.Forms.DialogResult.Cancel;

            var thread = new Thread(() =>
            {
                result = dialog.ShowDialog();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            return result;
        }

        public void Dispose()
        {
        }
    }
}

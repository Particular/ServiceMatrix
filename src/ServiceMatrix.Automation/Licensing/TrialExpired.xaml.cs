namespace NServiceBusStudio.Automation.Licensing
{
    using NuPattern.Presentation;
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Forms;
    using Particular.Licensing;
    using MessageBox = System.Windows.MessageBox;

    /// <summary>
    /// Interaction logic for ComponentPicker.xaml
    /// </summary>
    public partial class TrialExpired : CommonDialogWindow, IDialogWindow, IDisposable
    {
        public License CurrentLicense { get; set; }

        public string ResultingLicenseText;

        public TrialExpired()
        {
            InitializeComponent();
            Loaded += TrialExpired_Loaded;
        }

        void TrialExpired_Loaded(object sender, RoutedEventArgs e)
        {
            WarningText.Text = "The trial period is now over";

            if (CurrentLicense != null && CurrentLicense.IsTrialLicense)
            {
                Title = "ServiceMatrix - Initial Trial Expired";
                Instructions.Text = "To extend your free trial, click 'Extend trial' and register online. When you receive your license file, save it to disk and then click the 'Browse' button below to select it.";
                GetTrial.Content = "Extend Trial";
                ButtonPanel.Children.Remove((UIElement)FindName("Purchase"));
                Purchase.Visibility = Visibility.Hidden;
                GetTrial.Margin = Purchase.Margin;
              
            }
            else
            {
                Title = "ServiceMatrix - Extended Trial Expired";
                Instructions.Text = "Please click 'Contact Sales' to request an extension to your free trial, or click 'Buy Now' to purchase a license online. When you receive your license file, save it to disk and then click the 'Browse' button below to select it.";
                GetTrial.Content = "Contact Sales";
            }
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {

            using (var openDialog = new OpenFileDialog())
            {
                openDialog.InitializeLifetimeService();
                openDialog.Filter = "License files (*.xml)|*.xml|All files (*.*)|*.*";
                openDialog.Title = "Select License file";

                var dialogResult = StaThreadRunner.ShowDialogInSTA(() => { return openDialog.ShowDialog(); });
                if (dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    var licenseText = NonLockingFileReader.ReadAllTextWithoutLocking(openDialog.FileName);
                    try
                    {
                        LicenseVerifier.Verify(licenseText);
                        var license = LicenseDeserializer.Deserialize(licenseText);

                        if (LicenseExpirationChecker.HasLicenseExpired(license))
                        {
                            var message = string.Format("The license you provided has expired.\r\nEither extend your trial or contact sales to obtain a new license. Or try a different file.");
                            MessageBox.Show(this, message, "License expired", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return;
                        }
                        MessageBox.Show(this, "The new license has been verified. It will now be stored in the Registry for future use.", "License applied", MessageBoxButton.OK, MessageBoxImage.Information);
                        ResultingLicenseText = licenseText;
                        Close();
                    }
                    catch (Exception exception)
                    {
                        var message = string.Format("An error occurred when parsing the license.\r\nMessage: {0}\r\nThe exception details have been appended to your log.", exception.Message);
                        MessageBox.Show(this, message,"Error parsing license", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public void Dispose()
        {
        }

        void Purchase_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://particular.net/licensing");
        }

        void GetTrial_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrentLicense != null && CurrentLicense.IsTrialLicense)
            {
                Process.Start("http://particular.net/extend-your-trial-14");
            }
            else
            {
                Process.Start("http://particular.net/extend-your-trial-45");
            }
        }
    }
}

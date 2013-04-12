using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NuPattern.Common.Presentation;
using NuPattern.Runtime;
using NServiceBusStudio.Automation.Dialog;
using System.Windows.Markup;
using System.Collections.ObjectModel;

namespace NServiceBusStudio.Automation.Infrastructure.Licensing
{
    /// <summary>
    /// Interaction logic for ComponentPicker.xaml
    /// </summary>
    public partial class LicensingDialog : CommonDialogWindow, IDialogWindow
    {
        public LicensingDialog()
        {
            InitializeComponent();
        }

        public void TrialVersion(int daysleft)
        {
            this.Title = String.Format ("Trial version - {0} days left", daysleft.ToString());
            this.TrialVersionDaysLeft.Text = daysleft.ToString();

            this.TrialVersionPane.Visibility = System.Windows.Visibility.Visible;
            this.ExpiredTrialVersionPane.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void ExpiredTrialVersion()
        {
            this.Title = "Expired Trial version";

            this.TrialVersionPane.Visibility = System.Windows.Visibility.Collapsed;
            this.ExpiredTrialVersionPane.Visibility = System.Windows.Visibility.Visible;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }

}

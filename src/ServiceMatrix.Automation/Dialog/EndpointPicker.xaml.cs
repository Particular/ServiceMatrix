using System.Windows;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern.Presentation;

namespace AbstractEndpoint.Automation.Dialog
{
    /// <summary>
    /// Interaction logic for ComponentPicker.xaml
    /// </summary>
    public partial class EndpointPicker : IDialogWindow
    {
        public EndpointPicker()
        {
            InitializeComponent();
        }

        private void NewEndpoint_Click(object sender, RoutedEventArgs e)
        {
            ((EndpointPickerViewModel)DataContext).AddEndpoint();
            AddEndpointText.Focus();
        }

        private void AddEndpointCancel_Click(object sender, RoutedEventArgs e)
        {
            ((EndpointPickerViewModel)DataContext).CancelEndpoint();
        }
    }
}

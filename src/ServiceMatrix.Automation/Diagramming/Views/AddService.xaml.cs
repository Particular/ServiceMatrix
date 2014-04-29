namespace NServiceBusStudio.Automation.Diagramming.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for AddService.xaml
    /// </summary>
    public partial class AddService
    {
        public AddService()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}

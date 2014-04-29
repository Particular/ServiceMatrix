namespace NServiceBusStudio.Automation.Diagramming.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for AddEndpoint.xaml
    /// </summary>
    public partial class AddEndpoint
    {
        public AddEndpoint()
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

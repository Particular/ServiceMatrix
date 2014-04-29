namespace NServiceBusStudio.Automation.Licensing
{
    using NuPattern.Presentation;
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for Registered.xaml
    /// </summary>
    public partial class Registered : IDialogWindow, IDisposable
    {
        public Registered()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Dispose()
        {
        }
    }
}

using NuPattern.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NServiceBusStudio.Automation.Licensing
{
    /// <summary>
    /// Interaction logic for Registered.xaml
    /// </summary>
    public partial class Registered : CommonDialogWindow, IDialogWindow, IDisposable
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

using Mindscape.WpfDiagramming;
using Mindscape.WpfDiagramming.Foundation;
using NServiceBusStudio.Automation.Diagrams.ViewModels;
using NServiceBusStudio.Automation.Diagrams.ViewModels.Connections;
using NServiceBusStudio.Automation.Diagrams.ViewModels.Shapes;
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

namespace NServiceBusStudio.Automation.Diagrams.Views
{
    /// <summary>
    /// Interaction logic for Diagram.xaml
    /// </summary>
    public partial class Diagram : UserControl
    {
        public NServiceBusDiagramAdapter Adapter { get; set; }

        public Diagram(NServiceBusDiagramAdapter adapter)
        {
            InitializeComponent();

            this.Adapter = adapter;
            this.DataContext = this.Adapter.ViewModel;
        }

        private void OnAddEndpointClick(object sender, RoutedEventArgs e)
        {
            this.Adapter.AddEndpoint("New Endpoint", "HOST");
        }

        private void OnAddServiceClick(object sender, RoutedEventArgs e)
        {
            this.Adapter.AddService("New Service");
        }

        private void OnSaveAsImageClick(object sender, RoutedEventArgs e)
        {
            var filename = "Diagram.png";
            DiagramBitmapRenderer.Png.Render(ds, filename);
            System.Diagnostics.Process.Start(filename);
        }
    }
}

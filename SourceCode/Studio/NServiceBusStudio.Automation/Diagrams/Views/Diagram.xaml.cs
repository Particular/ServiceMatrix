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
        public NServiceBusDiagramViewModel ViewModel { get; set; }

        public Diagram(NServiceBusDiagramViewModel viewModel)
        {
            InitializeComponent();
            ds.IsSmartScrollingEnabled = true;
            this.ViewModel = viewModel;
            this.DataContext = this.ViewModel;
        }

        private void AddEndpoint_Click(object sender, RoutedEventArgs e)
        {
            // Left Endpoint
            //var ecommerceEndpoint = new EndpointNode("Ecommerce", "MVC", new Point(50, 20));
            //this.ViewModel.Nodes.Add(ecommerceEndpoint);

            //var purchaseService = new ServiceNode("Purchase", ecommerceEndpoint);
            //this.ViewModel.Nodes.Add(purchaseService);

            //var submitOrderProcessorSender = new ComponentNode("SubmitOrderSender", purchaseService);
            //this.ViewModel.Nodes.Add(submitOrderProcessorSender);


            //// Command
            //var submitOrderCommand = new CommandNode("SubmitOrder", new Point(450, 90));
            //this.ViewModel.Nodes.Add(submitOrderCommand);

            //this.ViewModel.Connections.Add(new CommandConnection(submitOrderProcessorSender.ConnectionPoints[1], submitOrderCommand.ConnectionPoints[0]));


            //// Right Endpoint
            //var emptyEndpoint = new EmptyEndpointNode(new Point (900, 30));
            //this.ViewModel.Nodes.Add(emptyEndpoint);

            //var purchaseService2 = new ServiceNode("Purchase", emptyEndpoint);
            //this.ViewModel.Nodes.Add(purchaseService2);

            //var submitOrderProcessorComponent = new ComponentNode("SubmitOrderProcessor", purchaseService2);
            //this.ViewModel.Nodes.Add(submitOrderProcessorComponent);

            //this.ViewModel.Connections.Add(new CommandConnection(submitOrderCommand.ConnectionPoints[1], submitOrderProcessorComponent.ConnectionPoints[0]));


            //// Event
            //var orderPlacedEvent = new EventNode("OrderPlaced", new Point(450, 200));
            //this.ViewModel.Nodes.Add(orderPlacedEvent);

            //this.ViewModel.Connections.Add(new EventConnection(submitOrderProcessorComponent.ConnectionPoints[0], orderPlacedEvent.ConnectionPoints[1]));


            //// Receive Event Endpoint
            //var ordersEndpoint = new EndpointNode("Orders", "Host", new Point(50, 250));
            //this.ViewModel.Nodes.Add(ordersEndpoint);

            //var orderProcessingService = new ServiceNode("OrderProcessing", ordersEndpoint);
            //this.ViewModel.Nodes.Add(orderProcessingService);

            //var orderPlacedProcessorComponent = new ComponentNode("OrderPlacedProcessor", orderProcessingService);
            //this.ViewModel.Nodes.Add(orderPlacedProcessorComponent);

            //this.ViewModel.Connections.Add(new EventConnection(orderPlacedEvent.ConnectionPoints[0], orderPlacedProcessorComponent.ConnectionPoints[1]));


        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            ds.BringIntoView(ds.DiagramBounds);

            //// Layout algorithms
            //var layout = new GridLayoutAlgorithm() { Info = new NServiceBusLayoutAlgorithmInfo(), HorizontalOffset = 200, VerticalOffset = 200 };
            //ds.ApplyLayoutAlgorithm(layout);
        }

        private void SaveAsImage_Click(object sender, RoutedEventArgs e)
        {
            var filename = "Diagram.png";
            DiagramBitmapRenderer.Png.Render(ds, filename);
            System.Diagnostics.Process.Start(filename);
        }
    }
}

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

namespace NServiceBusStudio.Automation.Dialog
{
    /// <summary>
    /// Interaction logic for NewDiagram.xaml
    /// </summary>
    public partial class NewDiagram : CommonDialogWindow, IDialogWindow
    {
        public NewDiagram()
        {
            InitializeComponent();
        }

        private void CommonDialogWindow_Initialized(object sender, EventArgs e)
        {
            var list = new List<NodeModel>();

            var node1 = new NodeModel() { Label = "Node1", Tooltip = "Tooltip 1" };
            
            var node2 = new NodeModel() { Label = "Node2", Tooltip = "Tooltip 2" };
            node2.Connections.Add (new NodeConnection() { Target = node1, Weight=1 });

            var node3 = new NodeModel() { Label = "Node3", Tooltip = "Tooltip 3" };
            node3.Connections.Add(new NodeConnection() { Target = node2, Weight = 0 });

            var node4 = new NodeModel() { Label = "Node4", Tooltip = "Tooltip 4" };
            node4.Connections.Add(new NodeConnection() { Target = node2, Weight = 10 });
            node4.Connections.Add(new NodeConnection() { Target = node3, Weight = 20 });

            list.Add(node1);
            list.Add(node2);
            list.Add(node3);
            list.Add(node4);

            this.xnn.ItemsSource = list;
        }


        public class NodeModel
        {
            public string Label { get; set; }
            public string Tooltip { get; set; }
            public IList<NodeConnection> Connections { get; set; }

            public NodeModel()
            {
                this.Connections = new List<NodeConnection>();
            }
        }

        public class NodeConnection
        {
            public NodeModel Target { get; set; }
            public int Weight { get; set; }
        }
    }
}

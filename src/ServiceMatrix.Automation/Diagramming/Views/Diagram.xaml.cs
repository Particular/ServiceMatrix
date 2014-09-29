using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Mindscape.WpfDiagramming.Foundation;
using NServiceBusStudio;
using ServiceMatrix.Diagramming.ViewModels;
using ServiceMatrix.Diagramming.ViewModels.BaseViewModels;
using ServiceMatrix.Diagramming.ViewModels.Connections;

namespace ServiceMatrix.Diagramming.Views
{
    /// <summary>
    /// Interaction logic for Diagram.xaml
    /// </summary>
    public partial class Diagram
    {
        public ServiceMatrixDiagramAdapter Adapter { get; set; }

        public bool ItemHasBeenAdded { get; set; }

        public Diagram(ServiceMatrixDiagramAdapter adapter, IDialogWindowFactory windowFactory)
        {
            InitializeComponent();

            Adapter = adapter;
            DataContext = new ServiceMatrixDiagramViewModel(Adapter, windowFactory);

            // Visible elements on DiagramSurface (only elements rendered by Virtualization)
            var diagramElementsCollection = (INotifyCollectionChanged)ds.DiagramElements;
            diagramElementsCollection.CollectionChanged += diagramElementsCollection_CollectionChanged;

            // All nodes on Diagram Nodes 
            var nodesCollection = adapter.ViewModel.Nodes as INotifyCollectionChanged;
            nodesCollection.CollectionChanged += nodesCollection_CollectionChanged;

            if (adapter.ViewModel.Nodes.Count > 0)
            {
                nodesCollection_CollectionChanged(null, null);
                ds.SizeToFit();
            }
        }

        void nodesCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ItemHasBeenAdded = true;
            EmptyStateButtons.Visibility = (Adapter.ViewModel.Nodes.Count > 0) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void diagramElementsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var diagramNodeElement = item as DiagramElement;
                        diagramNodeElement.MouseEnter += diagramNodeElement_MouseEnter;
                        diagramNodeElement.MouseLeave += diagramNodeElement_MouseLeave;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var diagramNodeElement = item as DiagramElement;
                        diagramNodeElement.MouseEnter -= diagramNodeElement_MouseEnter;
                        diagramNodeElement.MouseLeave -= diagramNodeElement_MouseLeave;
                    }
                    break;
            }

            try
            {
                if (ItemHasBeenAdded && !ds.GetViewport().Contains(ds.DiagramBounds))
                {
                    ds.SizeToFit();
                    ItemHasBeenAdded = false;
                }
            }
            catch { }
        }


        private void diagramNodeElement_MouseEnter(object sender, MouseEventArgs e)
        {
            var diagramNodeElement = sender as DiagramNodeElement;
            var diagramConnectionElement = sender as DiagramConnectionElement;

            if (diagramNodeElement != null)
            {
                var context = diagramNodeElement.Content as GroupableNode;
                ((ServiceMatrixDiagramViewModel)DataContext).Diagram.HighlightNode(context);
            }
            else if (diagramConnectionElement != null)
            {
                var context = diagramConnectionElement.Content as BaseConnection;
                ((ServiceMatrixDiagramViewModel)DataContext).Diagram.HighlightConnection(context);
            }
        }

        private void diagramNodeElement_MouseLeave(object sender, MouseEventArgs e)
        {
            var diagramNodeElement = sender as DiagramNodeElement;
            var diagramConnectionElement = sender as DiagramConnectionElement;

            if (diagramNodeElement != null)
            {
                var context = diagramNodeElement.Content as GroupableNode;
                ((ServiceMatrixDiagramViewModel)DataContext).Diagram.UnhighlightNode(context);
            }
            else if (diagramConnectionElement != null)
            {
                var context = diagramConnectionElement.Content as BaseConnection;
                ((ServiceMatrixDiagramViewModel)DataContext).Diagram.UnhighlightConnection(context);
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (ds.Zoom >= 2.75)
            {
                ds.Zoom = 10;
            }
            else
            {
                ds.Zoom += 0.25;
            }
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (ds.Zoom <= 0.50)
            {
                ds.Zoom = 0.25;
            }
            else
            {
                ds.Zoom -= 0.25;
            }
        }

        void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}

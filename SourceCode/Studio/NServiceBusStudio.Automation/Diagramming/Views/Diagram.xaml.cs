using Mindscape.WpfDiagramming;
using Mindscape.WpfDiagramming.Foundation;
using ServiceMatrix.Diagramming.ViewModels;
using ServiceMatrix.Diagramming.ViewModels.BaseViewModels;
using ServiceMatrix.Diagramming.ViewModels.Connections;
using ServiceMatrix.Diagramming.ViewModels.Shapes;
using NuPattern.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace ServiceMatrix.Diagramming.Views
{
    /// <summary>
    /// Interaction logic for Diagram.xaml
    /// </summary>
    public partial class Diagram : UserControl
    {
        public Diagram(ServiceMatrixDiagramAdapter adapter)
        {
            InitializeComponent();
            this.DataContext = new ServiceMatrixDiagramViewModel (adapter);

            var diagramElementsCollection = ds.DiagramElements as INotifyCollectionChanged;
            diagramElementsCollection.CollectionChanged += diagramElementsCollection_CollectionChanged;
        }

        private void diagramElementsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                foreach (var item in e.NewItems)
                {
                    var diagramNodeElement = item as DiagramNodeElement;
                    if (diagramNodeElement != null)
                    {
                        diagramNodeElement.MouseEnter += diagramNodeElement_MouseEnter;
                        diagramNodeElement.MouseLeave += diagramNodeElement_MouseLeave;
                    }
                }
                break;
                case NotifyCollectionChangedAction.Remove:
                foreach (var item in e.OldItems)
                {
                    var diagramNodeElement = item as DiagramNodeElement;
                    if (diagramNodeElement != null)
                    {
                        diagramNodeElement.MouseEnter -= diagramNodeElement_MouseEnter;
                        diagramNodeElement.MouseLeave -= diagramNodeElement_MouseLeave;
                    }
                }
                break;
            }
        }

        private void diagramNodeElement_MouseEnter(object sender, MouseEventArgs e)
        {
            var diagramNodeElement = sender as DiagramNodeElement;
            var context = diagramNodeElement.Content as GroupableNode;

            ((ServiceMatrixDiagramViewModel)this.DataContext).Diagram.HighlightElement(context);
        }

        private void diagramNodeElement_MouseLeave(object sender, MouseEventArgs e)
        {
            var diagramNodeElement = sender as DiagramNodeElement;
            var context = diagramNodeElement.Content as GroupableNode;

            ((ServiceMatrixDiagramViewModel)this.DataContext).Diagram.UnhighlightElement(context);
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
    }
}

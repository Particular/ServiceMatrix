using Mindscape.WpfDiagramming;
using NServiceBusStudio.Automation.Diagrams.ViewModels.BaseViewModels;
using NServiceBusStudio.Automation.Diagrams.ViewModels.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels
{
    public class NServiceBusDiagramLayoutAlgorithm
    {
        static Dictionary<Guid, Point> SavedPositions = new Dictionary<Guid, Point>();

        public NServiceBusDiagramMindscapeViewModel ViewModel { get; set; }

        public NServiceBusDiagramLayoutAlgorithm(NServiceBusDiagramMindscapeViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public void SetElementPosition(GroupableNode node)
        {
            var position = this.LoadElementPosition(node);

            if (!position.HasValue)
            {
                position = this.CalculateElementPosition(node);
                this.SaveElementPosition(node, position.Value);
            }

            node.BoundsChanged += (s, e) => this.SaveElementPosition(node, node.Bounds.Location);
            node.Bounds = new Rect (position.Value, node.Bounds.Size);
        }

        private Point CalculateElementPosition(GroupableNode node)
        {
            const int ShapeWidth = 350;
            var x = 50.0;
            var y = 100.0;

            if (node is EndpointNode)
            {
                // Align endpoints algorithm
                var endpointsCount = this.ViewModel.Nodes.Count(n => n is EndpointNode);
                // Snap to 2 columns
                if (endpointsCount % 2 == 0)
                {
                    x = 50;
                }
                else
                {
                    x = 950;
                }
            }
            else if (node is CommandNode || node is EventNode)
            {
                x = 500;
            }

            var shapesOnSimilarXPosition = this.ViewModel.Nodes.Cast<GroupableNode>().Where(n => n.ParentNode == null &&
                                                                                                 ((n.Bounds.X >= x && n.Bounds.X <= x + ShapeWidth) ||
                                                                                                  (n.Bounds.X + n.Bounds.Width >= x && n.Bounds.X + n.Bounds.Width <= x + ShapeWidth)));
            if (shapesOnSimilarXPosition.Any())
            {
                y = shapesOnSimilarXPosition.Max(n => n.Bounds.Y + n.Bounds.Height);
                y = y + 100;
            }

            return new Point(x, y);
        }

        public Point? LoadElementPosition(GroupableNode node)
        {
            if (SavedPositions.Any(x => x.Key == node.Id))
            {
                return SavedPositions.First(x => x.Key == node.Id).Value;
            }

            return null;
        }

        public void SaveElementPosition(GroupableNode node, Point point)
        {
            if (SavedPositions.Any(x => x.Key == node.Id))
            {
                SavedPositions.Remove(node.Id);
            }

            SavedPositions.Add(node.Id, point);
        }
    }

}

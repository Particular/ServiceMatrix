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
        public NServiceBusDiagramViewModel ViewModel { get; set; }

        public NServiceBusDiagramLayoutAlgorithm(NServiceBusDiagramViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }


        public Point GetElementPosition(Type node)
        {
            const int ShapeWidth = 350;
            var x = 0.0;
            var y = 0.0;

            if (node == typeof(EndpointNode))
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
            else if (node == typeof(CommandNode) || node == typeof(EventNode))
            {
                x = 500;
            }

            var shapesOnSimilarXPosition = this.ViewModel.Nodes.Where(n => n is GroupableNode && ((GroupableNode)n).ParentNode == null &&
                                                                           ((n.Bounds.X >= x && n.Bounds.X <= x + ShapeWidth) ||
                                                                           (n.Bounds.X + n.Bounds.Width >= x && n.Bounds.X + n.Bounds.Width <= x + ShapeWidth)));
            if (shapesOnSimilarXPosition.Any())
            {
                y = shapesOnSimilarXPosition.Max(n => n.Bounds.Y + n.Bounds.Height);
                y = y + 100;
            }

            return new Point(x, y);
        }

    }

}

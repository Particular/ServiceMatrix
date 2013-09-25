using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mindscape.WpfDiagramming.Foundation;
using System.Windows;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels.BaseViewModels
{
    // A special position calculator that uses the bounds of the collapsed group rather than the bounds of the node hosting the connection point.
    // This is important for the expand/collapse group feature.
    // Note that although the ElbowPathfinderAdapter takes care of all the connection re-routing, using this custom position calculator
    // makes it easier to evenly distribute multiple connection along the same edge of a collapsed group if neccessary.
    public class CollapsedPositionCalculator : IPositionCalculator
    {
        public CollapsedPositionCalculator(GroupNode node, double xFactor, double yFactor, IPositionCalculator originalCalculator)
        {
            GroupNode = node;
            XFactor = xFactor;
            YFactor = yFactor;
            OriginalCalculator = originalCalculator;
        }

        public GroupNode GroupNode { get; set; }

        public double XFactor { get; set; }

        public double YFactor { get; set; }

        // Store the original position calculator that this custom calculator is temporarily replacing.
        public IPositionCalculator OriginalCalculator { get; set; }

        public Point GetPosition(Rect bounds)
        {
            double x = GroupNode.Bounds.Left + (GroupNode.Bounds.Width * XFactor);
            double y = GroupNode.Bounds.Top + (GroupNode.Bounds.Height * YFactor);
            return new Point(x, y);
        }
    }
}

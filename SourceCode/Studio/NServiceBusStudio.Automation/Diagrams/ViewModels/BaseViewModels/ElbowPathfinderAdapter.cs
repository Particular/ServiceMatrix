using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mindscape.WpfDiagramming.Foundation;
using System.Collections.ObjectModel;
using Mindscape.WpfDiagramming;
using System.Windows;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels.BaseViewModels
{
    // This is a custom pathfinder which hijacks the ElbowPathfinder for re-routing a connection when a group node is collapsed.
    public class ElbowPathfinderAdapter : IPathfinder
    {
        private static ElbowPathfinderAdapter _instance = new ElbowPathfinderAdapter();

        public static ElbowPathfinderAdapter Instance { get { return _instance; } }

        public ReadOnlyCollection<Point> FindPath(IDiagramConnection connection, IDiagramModel diagram)
        {
            ElbowPathfinder pathfinder = ElbowPathfinder.Instance;

            CollapsableConnection collapsable = connection as CollapsableConnection;
            if (collapsable != null && collapsable.IsVisible)
            {
                GroupNode toCollapsedParent = null;
                GroupNode fromCollapsedParent = null;

                DiagramConnectionPoint fromPoint = connection.FromConnectionPoint as DiagramConnectionPoint;
                if (fromPoint != null)
                {
                    GroupableNode fromGroupable = fromPoint.Connectable as GroupableNode;
                    if (fromGroupable != null)
                    {
                        fromCollapsedParent = fromGroupable.GetCollapsedParent();
                        if (fromCollapsedParent != null)
                        {
                            // If we have come this far, then the source of this connection is inside a collapsed group, and so the connection path needs to be modified.
                            // First we call UpdateCollapsedPositionCalculators to replace the position calculator of the source connection point.
                            // This is to evenly distribute multiple connections along the same edge of the collapsed group if neccessary.
                            fromCollapsedParent.UpdateCollapsedPositionCalculators(collapsable.FromConnectionPoint);
                            fromCollapsedParent.CanAcceptConnections = true;
                            // Then we create a dummy source connection point to trick the ElbowPathfinder into creating the re-routed path.
                            fromPoint = new DiagramConnectionPoint(fromCollapsedParent, fromPoint.Edge) { PositionCalculator = fromPoint.PositionCalculator };
                        }
                    }
                }

                DiagramConnectionPoint toPoint = connection.ToConnectionPoint as DiagramConnectionPoint;
                if (toPoint != null)
                {
                    GroupableNode toGroupable = toPoint.Connectable as GroupableNode;
                    if (toGroupable != null)
                    {
                        toCollapsedParent = toGroupable.GetCollapsedParent();
                        if (toCollapsedParent != null)
                        {
                            // If we have come this far, then the destination of this connection is inside a collapsed group, and so the connection path needs to be modified.
                            // First we call UpdateCollapsedPositionCalculators to replace the position calculator of the destination connection point.
                            // This is to evenly distribute multiple connections along the same edge of the collapsed group if neccessary.
                            toCollapsedParent.UpdateCollapsedPositionCalculators(collapsable.ToConnectionPoint);
                            toCollapsedParent.CanAcceptConnections = true;
                            // Then we create a dummy destination connection point to trick the ElbowPathfinder into creating the re-routed path.
                            toPoint = new DiagramConnectionPoint(toCollapsedParent, toPoint.Edge) { PositionCalculator = toPoint.PositionCalculator };
                        }
                    }
                }
                if (fromPoint != connection.FromConnectionPoint || toPoint != connection.ToConnectionPoint)
                {
                    // If a dummy connection point was created, then created a dummy connection for creating the modified connection path.
                    DiagramConnection dummyConnection = new DiagramConnection(fromPoint, toPoint);

                    // Set the CanAcceptConnections properties back to false for better UI experience.
                    if (toCollapsedParent != null)
                    {
                        toCollapsedParent.CanAcceptConnections = false;
                    }
                    if (fromCollapsedParent != null)
                    {
                        fromCollapsedParent.CanAcceptConnections = false;
                    }

                    // Use the dummy connection to create the modified path for the real connection.
                    ReadOnlyCollection<Point> path = pathfinder.FindPath(dummyConnection, diagram);
                    // Set the connection points on the dummy connection to null to remove event handlers.
                    dummyConnection.FromConnectionPoint = null;
                    dummyConnection.ToConnectionPoint = null;
                    return path;
                }
            }

            // Normal unmodified connection path creation:
            return pathfinder.FindPath(connection, diagram);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mindscape.WpfDiagramming;
using Mindscape.WpfDiagramming.Foundation;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels.BaseViewModels
{
    // A diagram connection that can be made invisible using the IsVisible property.
    public class CollapsableConnection : DiagramConnection
    {
        private bool _isVisible = true;

        public CollapsableConnection(DiagramConnectionPoint fromConnectionPoint, DiagramConnectionPoint toConnectionPoint)
            : base(fromConnectionPoint, toConnectionPoint)
        {
        }

        public CollapsableConnection(DiagramConnectionPoint fromConnectionPoint, DiagramConnectionPoint toConnectionPoint, IList<DiagramConnectionSegment> segments)
            : base(fromConnectionPoint, toConnectionPoint, segments)
        {
        }

        // Gets the custom connection pathfinder. This is important for the expand/collapse group feature.
        public override IPathfinder Pathfinder
        {
            get { return ElbowPathfinderAdapter.Instance; }
        }

        public override bool CanReceiveNewConnections
        {
            get { return true; }
        }

        public override bool CanOriginateNewConnections
        {
            get { return true; }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { Set<bool>(ref _isVisible, value, "IsVisible"); }
        }
    }
}

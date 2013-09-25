using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mindscape.WpfDiagramming;
using System.ComponentModel;
using Mindscape.WpfDiagramming.Foundation;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;
using NuPattern.Runtime.UI.ViewModels;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels.BaseViewModels
{
    // A node that can be the parent of groupable nodes.
    public abstract class GroupNode : GroupableNode
    {
        private const double CollapsedWidth = 180;
        private const double CollapsedHeight = 100;

        private bool _isChildDragOver;
        private bool _isExpanded = true;
        private bool _isChildLeaving = false;
        private Rect _expandedBounds;

        // This dictionary is for that logic that evenly distributes multiple connection along the same edge of the collapsed GroupNode if neccessary.
        private Dictionary<Edge, IList<DiagramConnectionPoint>> _points = new Dictionary<Edge, IList<DiagramConnectionPoint>>();

        public GroupNode(IProductElementViewModel innerViewModel)
            : base(innerViewModel)
        {
            RotationChanged += new EventHandler(GroupNode_RotationChanged);
            this.IsResizable = false;

            this.ChildNodes = new List<GroupableNode>();
        }

        private void GroupNode_RotationChanged(object sender, EventArgs e)
        {
            // Force all the connection point positions to update if this GroupNode is rotated.
            Bounds = new Rect(Bounds.X + 1, Bounds.Y + 1, Bounds.Width, Bounds.Height);
            Bounds = new Rect(Bounds.X - 1, Bounds.Y - 1, Bounds.Width, Bounds.Height);
        }

        // This is so the UI can indicate to the user that it can accept the node being dragged over this GroupNode.
        public bool IsChildDragOver
        {
            get { return _isChildDragOver; }
            set { Set<bool>(ref _isChildDragOver, value, "IsChildDragOver"); }
        }

        // This is so the UI can indicate to the user that moving a child node away from it's group node will remove it from the group node.
        public bool IsChildLeaving
        {
            get { return _isChildLeaving; }
            set { Set<bool>(ref _isChildLeaving, value, "IsChildLeaving"); }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value)
                {
                    RevertCollapsedPositionCalculators();
                    Set<bool>(ref _isExpanded, value, "IsExpanded");
                    if (!IsExpanded)
                    {
                        // When collapsed, change the size of this node to the collapsed size, and disable resizing (optional).
                        _expandedBounds = Bounds;
                        Bounds = new Rect(Bounds.X + (Bounds.Width - CollapsedWidth), Bounds.Y, CollapsedWidth, CollapsedHeight);
                        IsResizable = false;
                    }
                    else
                    {
                        // When expanded, restore the size of the node, but keep the top right corner of the node in the same place.
                        Bounds = new Rect(Bounds.X - (_expandedBounds.Width - Bounds.Width), Bounds.Y, _expandedBounds.Width, _expandedBounds.Height);
                        IsResizable = true;
                    }
                    OnIsExpandedChanged();
                }
            }
        }

        public event EventHandler IsExpandedChanged;

        private void OnIsExpandedChanged()
        {
            EventHandler handler = IsExpandedChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        internal void UpdateCollapsedPositionCalculators(DiagramConnectionPoint point)
        {
            if (point.Connections.Count == 0)
            {
                return;
            }
            // Obtain the current collection of connection points along the same edge as the given connection point.
            IList<DiagramConnectionPoint> connectionPoints;
            _points.TryGetValue(point.Edge, out connectionPoints);
            // If this is the first connection point along this edge, then create the collection and add it to the dictionary.
            if (connectionPoints == null)
            {
                connectionPoints = new List<DiagramConnectionPoint>();
                _points[point.Edge] = connectionPoints;
            }
            // If the given connection point is already in the collection, then nothing needs to be done.
            if (!connectionPoints.Contains(point))
            {
                connectionPoints.Add(point);
                double count = connectionPoints.Count;
                double spacing = 1 / (count + 1.0); // This is for evenly spacing multiple connection along the same edge of the collapsed group node.
                double x = spacing;
                foreach (DiagramConnectionPoint p in connectionPoints)
                {
                    // Maintain the original position calculator that will temporarily be replaced while this node is collapsed.
                    IPositionCalculator originalCalc = p.PositionCalculator;
                    CollapsedPositionCalculator calc = p.PositionCalculator as CollapsedPositionCalculator;
                    if (calc != null)
                    {
                        originalCalc = calc.OriginalCalculator;
                    }
                    // Depending on the edge, replace the position calculator of the given point with a custom position calculator that manages the collapsed group scenario.
                    switch (point.Edge)
                    {
                        case Edge.Left:
                            p.PositionCalculator = new CollapsedPositionCalculator(this, 0, x, originalCalc);
                            break;
                        case Edge.Top:
                            p.PositionCalculator = new CollapsedPositionCalculator(this, x, 0, originalCalc);
                            break;
                        case Edge.Right:
                            p.PositionCalculator = new CollapsedPositionCalculator(this, 1, x, originalCalc);
                            break;
                        case Edge.Bottom:
                            p.PositionCalculator = new CollapsedPositionCalculator(this, x, 1, originalCalc);
                            break;
                    }
                    x += spacing;
                }
            }
        }

        // Restore the position calculator of all the connection points to their original values, then clear the dictionary.
        // This is done when the visibility or expanded state of the group node changes.
        private void RevertCollapsedPositionCalculators()
        {
            foreach (IList<DiagramConnectionPoint> points in _points.Values)
            {
                foreach (DiagramConnectionPoint point in points)
                {
                    CollapsedPositionCalculator calc = point.PositionCalculator as CollapsedPositionCalculator;
                    if (calc != null)
                    {
                        point.PositionCalculator = calc.OriginalCalculator;
                    }
                }
            }
            _points = new Dictionary<Edge, IList<DiagramConnectionPoint>>();
        }

        protected override void OnIsVisibleChanged()
        {
            base.OnIsVisibleChanged();
            RevertCollapsedPositionCalculators();
        }

        // In the UI, group nodes can not recieve or originate connections.
        // But when creating dummy connections in the ElbowPathfinderAdapter, we need to allow this to prevent an exception being thrown.
        // So the CanAcceptConnnections property lets us control this.

        public override bool CanReceiveNewConnections
        {
            get { return CanAcceptConnections; }
        }

        public override bool CanOriginateNewConnections
        {
            get { return CanAcceptConnections; }
        }

        internal bool CanAcceptConnections { get; set; }



        #region Shapes Group Position & Resizing

        public IList<GroupableNode> ChildNodes { get; set; }
        public double SHAPE_HEADER_HEIGHT = 40;
        public double SHAPE_VERTICAL_MARGIN = 10;
        public double SHAPE_MIN_HEIGHT = 190;

        public void AddChild(GroupableNode child)
        {
            this.ChildNodes.Add(child);
            this.RecalculateHeight();
        }

        public void RemoveChild(GroupableNode child)
        {
            this.ChildNodes.Remove(child);
            RecalculateHeight();
        }

        public void RecalculateHeight()
        {
            // Set Shape Height based on Child's Height
            var childNodesHeight = this.ChildNodes.Sum(x => x.Bounds.Height + SHAPE_VERTICAL_MARGIN);
            childNodesHeight += SHAPE_HEADER_HEIGHT;
            if (childNodesHeight < SHAPE_MIN_HEIGHT)
            {
                childNodesHeight = SHAPE_MIN_HEIGHT;
            }
            this.Bounds = new Rect(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, childNodesHeight);

            // Define new Child position
            var childPosition = SHAPE_HEADER_HEIGHT;
            foreach (var item in this.ChildNodes)
            {
                item.SetPosition(childPosition);
                childPosition += item.Bounds.Height + SHAPE_VERTICAL_MARGIN;
            }

            // Recalculate Parent Height based on new Node Height
            if (this.Parent != null &&
                this.Parent is GroupNode)
            {
                ((GroupNode)this.Parent).RecalculateHeight();
            }
        }

        #endregion


        
    }
}

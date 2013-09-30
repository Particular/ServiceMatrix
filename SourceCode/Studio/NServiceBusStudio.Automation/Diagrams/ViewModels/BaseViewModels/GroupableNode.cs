using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mindscape.WpfDiagramming;
using Mindscape.WpfDiagramming.Foundation;
using System.Windows;
using NuPattern.Runtime.UI.ViewModels;
using System.Collections.ObjectModel;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels.BaseViewModels
{
    // A node that can be added to a group node. Both GroupNode and ChildNode extends this.
    public abstract class GroupableNode : DiagramNode
    {
        public static int ZOrderCounter = 0;

        private bool _isVisible = true;
        public IProductElementViewModel InnerViewModel { get; set; }

        public virtual Guid Id 
        { 
            get { return this.InnerViewModel.Data.Id; } 
        }

        public virtual string Name 
        {
            get { return this.InnerViewModel.Data.InstanceName; } 
        }

        public virtual ObservableCollection<IMenuOptionViewModel> MenuOptions
        {
            get { return this.InnerViewModel.MenuOptions; }
        }

        public GroupableNode(IProductElementViewModel innerViewModel)
        {
            this.InnerViewModel = innerViewModel;
            this.ZOrder = ++ZOrderCounter;
        }

        // This is for hiding the node if an ancestor group node is collapsed.
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    Set<bool>(ref _isVisible, value, "IsVisible");
                    // Loop through each of the connections attached to this node to set their visibility.
                    foreach (IDiagramConnectionPoint point in ConnectionPoints)
                    {
                        foreach (IDiagramConnection connection in point.Connections)
                        {
                            CollapsableConnection collapsable = connection as CollapsableConnection;
                            if (collapsable != null)
                            {
                                if (collapsable.FromConnectionPoint != null && collapsable.ToConnectionPoint != null)
                                {
                                    GroupableNode fromNode = GetFromNode(collapsable);
                                    GroupableNode toNode = GetToNode(collapsable);
                                    if (fromNode != null && toNode != null)
                                    {
                                        // if both ends of the connection have a common collapsed group node ancestor, then hide the connection, otherwise let it be visible.
                                        GroupNode fromCollapsedParent = fromNode.GetCollapsedParent();
                                        GroupNode toCollapsedParent = toNode.GetCollapsedParent();
                                        collapsable.IsVisible = fromCollapsedParent == null || fromCollapsedParent != toCollapsedParent;
                                    }
                                    else
                                    {
                                        collapsable.IsVisible = true;
                                    }
                                }
                                else
                                {
                                    collapsable.IsVisible = true;
                                }
                            }
                        }
                    }
                    OnIsVisibleChanged();
                }
            }
        }

        private GroupableNode GetToNode(CollapsableConnection connection)
        {
            GroupableNode node = connection.ToConnectionPoint.Connectable as GroupableNode;
            if (node == null)
            {
                CollapsableConnection toConnection = connection.ToConnectionPoint.Connectable as CollapsableConnection;
                if (toConnection != null)
                {
                    node = GetToNode(toConnection);
                }
            }
            return node;
        }

        private GroupableNode GetFromNode(CollapsableConnection connection)
        {
            GroupableNode node = connection.FromConnectionPoint.Connectable as GroupableNode;
            if (node == null)
            {
                CollapsableConnection fromConnection = connection.FromConnectionPoint.Connectable as CollapsableConnection;
                if (fromConnection != null)
                {
                    node = GetFromNode(fromConnection);
                }
            }
            return node;
        }

        // Returns the collapsed group node ancestor that is highest up the heirarchy structure.
        internal GroupNode GetCollapsedParent()
        {
            GroupNode collapsedParent = null;
            GroupNode parent = Parent as GroupNode;
            while (parent != null)
            {
                if (!parent.IsExpanded)
                {
                    collapsedParent = parent;
                }
                parent = parent.Parent as GroupNode;
            }
            return collapsedParent;
        }

        public event EventHandler IsVisibleChanged;

        protected virtual void OnIsVisibleChanged()
        {
            EventHandler handler = IsVisibleChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public override IDiagramPositionable Parent
        {
            get { return this.ParentNode; }
        }

        public GroupNode ParentNode { get; set; }

        internal void SetParent(GroupNode parent)
        {
            // Make sure we don't create a loop in the heirarchy structure.
            IDiagramPositionable node = parent;
            while (node != null)
            {
                if (node == this)
                {
                    return;
                }
                node = node.Parent;
            }
            if (this.ParentNode != parent)
            {
                // Detach event handlers.
                if (this.ParentNode != null)
                {
                    this.ParentNode.BoundsChanged -= new EventHandler(Parent_BoundsChanged);

                    GroupNode group = this.ParentNode as GroupNode;
                    if (group != null)
                    {
                        group.IsExpandedChanged -= new EventHandler(Group_IsExpandedChanged);
                        group.IsVisibleChanged -= new EventHandler(Group_IsVisibleChanged);
                    }
                }

                this.ParentNode = parent; // Set the parent.

                // Attach event handlers.
                if (this.ParentNode != null)
                {
                    this.ParentNode.BoundsChanged += new EventHandler(Parent_BoundsChanged);

                    GroupNode group = this.ParentNode as GroupNode;
                    if (group != null)
                    {
                        group.IsExpandedChanged += new EventHandler(Group_IsExpandedChanged);
                        group.IsVisibleChanged += new EventHandler(Group_IsVisibleChanged);
                    }
                }
            }

            this.ParentNode.AddChild(this);
        }

        // This node is only visibile if its parent is also visible and an expanded group node.

        private void Group_IsVisibleChanged(object sender, EventArgs e)
        {
            GroupNode group = sender as GroupNode;
            IsVisible = group.IsVisible && group.IsExpanded;
        }

        private void Group_IsExpandedChanged(object sender, EventArgs e)
        {
            GroupNode group = sender as GroupNode;
            IsVisible = group.IsVisible && group.IsExpanded;
        }

        // This updates the position of this node if the parent node is moved.
        private void Parent_BoundsChanged(object sender, EventArgs e)
        {
            OnBoundsChanged();
        }

        /*private bool _updateBoundsLock = false;

        protected override void OnBoundsChanged()
        {
          base.OnBoundsChanged();
          if (!_updateBoundsLock)
          {
            _updateBoundsLock = true;
            if (Parent != null)
            {
              Rect parentBounds = Parent.Bounds;
              if (Bounds.X + Bounds.Width + 10 > parentBounds.Width)
              {
                Parent.Bounds = new Rect(parentBounds.X, parentBounds.Y, Bounds.X + Bounds.Width + 10, parentBounds.Height);
              }
              if (Bounds.X - 10 < 0)
              {
                double diff = Bounds.X - 10;
                Parent.Bounds = new Rect(parentBounds.X + diff, parentBounds.Y, parentBounds.Width - diff, parentBounds.Height);
                Bounds = new Rect(Bounds.X - diff, Bounds.Y, Bounds.Width, Bounds.Height);
              }
            }
            _updateBoundsLock = false;
          }
        }*/

        #region Shapes Group Position & Resizing

        public void SetPosition(double Y)
        {
            this.Bounds = new Rect(this.Bounds.X, Y, this.Bounds.Width, this.Bounds.Height);
        }

        #endregion
    }
}

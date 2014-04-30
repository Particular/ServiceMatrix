namespace ServiceMatrix.Diagramming.ViewModels.BaseViewModels
{
    using System;
    using Mindscape.WpfDiagramming;
    using Mindscape.WpfDiagramming.Foundation;
    using System.Windows;
    using NuPattern.Runtime.UI.ViewModels;
    using System.Collections.ObjectModel;

    using System.ComponentModel;

    // A node that can be added to a group node. Both GroupNode and ChildNode extends this.
    public abstract class GroupableNode : DiagramNode
    {
        public static int ZOrderCounter = 0;

        private bool _isVisible = true;
        private bool _isHighlighted;
        public IProductElementViewModel InnerViewModel { get; set; }

        public delegate void ActivateElementHandler(object sender, EventArgs e);
        public event ActivateElementHandler ActivateElement;

        public virtual Guid Id 
        { 
            get { return InnerViewModel.Data.Id; } 
        }

        public virtual string Name 
        {
            get { return InnerViewModel.Data.InstanceName; } 
        }

        public virtual ObservableCollection<IMenuOptionViewModel> MenuOptions
        {
            get { return InnerViewModel.MenuOptions; }
        }

        public GroupableNode(IProductElementViewModel innerViewModel)
        {
            InnerViewModel = innerViewModel;
            if (InnerViewModel != null)
            {
                InnerViewModel.Data.PropertyChanged += InnerViewModelData_PropertyChanged;
            }

            ZOrder = ++ZOrderCounter;
        }

        void InnerViewModelData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                OnPropertyChanged("Id");
            } 
            else if (e.PropertyName == "InstanceName")
            {
                OnPropertyChanged("Name");
            }
            else if (e.PropertyName == "IsSaga")
            {
                OnPropertyChanged("IsSaga");
            }
        }

        public void Activate()
        {
            InnerViewModel.IsSelected = true;

            if (ActivateElement != null)
            {
                ActivateElement(this, new EventArgs());
            }
        }
    

        // This is for highlighting the node if mouse is over.
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                Set<bool>(ref _isHighlighted, value, "IsHighlighted");
            }
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
                    OnIsVisibleChanged();
                }
            }
        }

        // Returns the collapsed group node ancestor that is highest up the heirarchy structure.
        internal GroupNode GetCollapsedParent()
        {
            GroupNode collapsedParent = null;
            var parent = Parent as GroupNode;
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
            var handler = IsVisibleChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public override IDiagramPositionable Parent
        {
            get { return ParentNode; }
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
            if (ParentNode != parent)
            {
                // Detach event handlers.
                if (ParentNode != null)
                {
                    ParentNode.BoundsChanged -= new EventHandler(Parent_BoundsChanged);

                    var group = ParentNode;
                    if (group != null)
                    {
                        group.IsExpandedChanged -= new EventHandler(Group_IsExpandedChanged);
                        group.IsVisibleChanged -= new EventHandler(Group_IsVisibleChanged);
                    }
                }

                ParentNode = parent; // Set the parent.

                // Attach event handlers.
                if (ParentNode != null)
                {
                    ParentNode.BoundsChanged += new EventHandler(Parent_BoundsChanged);

                    var group = ParentNode;
                    if (group != null)
                    {
                        group.IsExpandedChanged += new EventHandler(Group_IsExpandedChanged);
                        group.IsVisibleChanged += new EventHandler(Group_IsVisibleChanged);
                    }
                }
            }

            ParentNode.AddChild(this);
        }

        // This node is only visibile if its parent is also visible and an expanded group node.

        private void Group_IsVisibleChanged(object sender, EventArgs e)
        {
            var group = sender as GroupNode;
            IsVisible = group.IsVisible && group.IsExpanded;
        }

        private void Group_IsExpandedChanged(object sender, EventArgs e)
        {
            var group = sender as GroupNode;
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
            Bounds = new Rect(Bounds.X, Y, Bounds.Width, Bounds.Height);
        }

        #endregion
    }
}

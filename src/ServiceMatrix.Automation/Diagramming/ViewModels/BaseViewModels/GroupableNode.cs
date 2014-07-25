using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Mindscape.WpfDiagramming;
using Mindscape.WpfDiagramming.Foundation;
using NuPattern.Runtime.UI.ViewModels;
using System.Linq;
using NuPattern.Library.Automation;

namespace ServiceMatrix.Diagramming.ViewModels.BaseViewModels
{
    using System.Web.UI.WebControls;

    // A node that can be added to a group node. Both GroupNode and ChildNode extends this.
    public abstract class GroupableNode : DiagramNode
    {
        public static int ZOrderCounter = 0;

        private bool _isVisible = true;
        private bool _isHighlighted = false;
        ObservableCollection<IMenuOptionViewModel> menuOptions;

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
            get { return menuOptions ?? InnerViewModel.MenuOptions; }
        }

        public GroupableNode(IProductElementViewModel innerViewModel)
        {
            InnerViewModel = innerViewModel;
            if (InnerViewModel != null)
            {
                InnerViewModel.Data.PropertyChanged += InnerViewModelData_PropertyChanged;
                SetupMenuOptions();
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
                ActivateElement(this, EventArgs.Empty);
            }
        }


        // This is for highlighting the node if mouse is over.
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                Set(ref _isHighlighted, value, "IsHighlighted");
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
                    Set(ref _isVisible, value, "IsVisible");
                    // Loop through each of the connections attached to this node to set their visibility.
                    foreach (IDiagramConnectionPoint point in ConnectionPoints)
                    {
                        foreach (var connection in point.Connections)
                        {
                            //CollapsableConnection collapsable = connection as CollapsableConnection;
                            //if (collapsable != null)
                            //{
                            //    if (collapsable.FromConnectionPoint != null && collapsable.ToConnectionPoint != null)
                            //    {
                            //        GroupableNode fromNode = GetFromNode(collapsable);
                            //        GroupableNode toNode = GetToNode(collapsable);
                            //        if (fromNode != null && toNode != null)
                            //        {
                            //            // if both ends of the connection have a common collapsed group node ancestor, then hide the connection, otherwise let it be visible.
                            //            GroupNode fromCollapsedParent = fromNode.GetCollapsedParent();
                            //            GroupNode toCollapsedParent = toNode.GetCollapsedParent();
                            //            collapsable.IsVisible = fromCollapsedParent == null || fromCollapsedParent != toCollapsedParent;
                            //        }
                            //        else
                            //        {
                            //            collapsable.IsVisible = true;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        collapsable.IsVisible = true;
                            //    }
                            //}
                        }
                    }
                    OnIsVisibleChanged();
                }
            }
        }

        //private GroupableNode GetToNode(CollapsableConnection connection)
        //{
        //    GroupableNode node = connection.ToConnectionPoint.Connectable as GroupableNode;
        //    if (node == null)
        //    {
        //        CollapsableConnection toConnection = connection.ToConnectionPoint.Connectable as CollapsableConnection;
        //        if (toConnection != null)
        //        {
        //            node = GetToNode(toConnection);
        //        }
        //    }
        //    return node;
        //}

        //private GroupableNode GetFromNode(CollapsableConnection connection)
        //{
        //    GroupableNode node = connection.FromConnectionPoint.Connectable as GroupableNode;
        //    if (node == null)
        //    {
        //        CollapsableConnection fromConnection = connection.FromConnectionPoint.Connectable as CollapsableConnection;
        //        if (fromConnection != null)
        //        {
        //            node = GetFromNode(fromConnection);
        //        }
        //    }
        //    return node;
        //}

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

                    var group = ParentNode as GroupNode;
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

                    var group = ParentNode as GroupNode;
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

        private void SetupMenuOptions()
        {
            var deleteAutomation = InnerViewModel.Data.AutomationExtensions.FirstOrDefault(ae => ae.Name == "Delete");
            if (deleteAutomation == null)
            {
                return;
            }

            var localMenuOptions = new ObservableCollection<IMenuOptionViewModel>(InnerViewModel.MenuOptions);
            for (var i = 0; i < localMenuOptions.Count; i++)
            {
                var menuOption = localMenuOptions[i] as MenuOptionViewModel;
                if (menuOption != null && menuOption.Caption == "Delete")
                {
                    localMenuOptions[i] =
                        new AutomationMenuOptionViewModel(deleteAutomation, menuOption.Caption, menuOption.ImagePath, menuOption.SortOrder)
                        {
                            GroupIndex = menuOption.GroupIndex
                        };
                    InnerViewModel.MenuOptions.CollectionChanged += MenuOptions_CollectionChanged;
                    menuOptions = localMenuOptions;
                    return;
                }
            }
        }

        void MenuOptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // TODO keep in sync - necessary?
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

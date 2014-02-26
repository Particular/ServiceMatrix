using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mindscape.WpfDiagramming;
using Mindscape.WpfDiagramming.Foundation;
using System.Windows;
using NuPattern.Runtime.UI.ViewModels;

namespace ServiceMatrix.Diagramming.ViewModels.BaseViewModels
{
    // A basic node that can be added to a group node. This node has one connection point on each edge side (left and right).
    public class ChildNode : GroupableNode
    {
        public ChildNode(IProductElementViewModel innerViewModel)
            : base(innerViewModel)
        {
            ConnectionPoints.Add(new DiagramConnectionPoint(this, Edge.Left));
            ConnectionPoints.Add(new DiagramConnectionPoint(this, Edge.Right));
        }
    }
}

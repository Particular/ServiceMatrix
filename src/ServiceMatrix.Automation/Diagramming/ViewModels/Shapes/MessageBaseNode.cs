namespace ServiceMatrix.Diagramming.ViewModels.Shapes
{
    using Mindscape.WpfDiagramming;
    using ServiceMatrix.Diagramming.ViewModels.BaseViewModels;
    using NuPattern.Runtime.UI.ViewModels;
    using System.Windows;

    public abstract class MessageBaseNode : GroupableNode
    {
        public MessageBaseNode(IProductElementViewModel innerViewModel)
            : base(innerViewModel)
        {
            ConnectionPoints.Add(new DiagramConnectionPoint(this, Edge.Left));
            ConnectionPoints.Add(new DiagramConnectionPoint(this, Edge.Right));

            IsResizable = false;
            Bounds = new Rect(0, 0, 320, 52);
        }
    }
}

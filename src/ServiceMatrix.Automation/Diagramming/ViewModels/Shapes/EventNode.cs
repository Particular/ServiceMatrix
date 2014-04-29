namespace ServiceMatrix.Diagramming.ViewModels.Shapes
{
    using NuPattern.Runtime.UI.ViewModels;

    public class EventNode : MessageBaseNode
    {
        public EventNode(IProductElementViewModel innerViewModel)
            : base(innerViewModel)
        {
        }
    }
}

namespace ServiceMatrix.Diagramming.ViewModels.Shapes
{
    using NuPattern.Runtime.UI.ViewModels;

    public class MessageNode : MessageBaseNode
    {
        public MessageNode(IProductElementViewModel innerViewModel)
            : base(innerViewModel)
        {
        }
    }
}

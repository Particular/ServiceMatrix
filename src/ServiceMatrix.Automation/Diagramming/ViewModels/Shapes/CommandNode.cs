namespace ServiceMatrix.Diagramming.ViewModels.Shapes
{
    using NuPattern.Runtime.UI.ViewModels;

    public class CommandNode : MessageBaseNode
    {
        public CommandNode(IProductElementViewModel innerViewModel)
            : base(innerViewModel)
        {
        }
    }
}

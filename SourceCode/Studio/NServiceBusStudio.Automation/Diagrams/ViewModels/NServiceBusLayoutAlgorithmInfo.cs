using Mindscape.WpfDiagramming.Foundation;
using NServiceBusStudio.Automation.Diagrams.ViewModels.Shapes;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels
{
    public class NServiceBusLayoutAlgorithmInfo : ILayoutAlgorithmInfo
    {
        public bool IsIncluded(IDiagramNode node)
        {
            return node is EndpointNode ||
                   node is CommandNode ||
                   node is EventNode;
        }
    }
}

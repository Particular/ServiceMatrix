namespace ServiceMatrix.Diagramming.ViewModels
{
    using Mindscape.WpfDiagramming.Foundation;
    using System;

    public class ServiceMatrixConnectionBuilder : IDiagramConnectionBuilder
    {
        public bool CanCreateConnection(IDiagramModel diagram, IDiagramConnectionPoint fromConnectionPoint, ConnectionDropTarget dropTarget)
        {
            return false;
        }

        public void CreateConnection(IDiagramModel diagram, IDiagramConnectionPoint fromConnectionPoint, IDiagramConnectionPoint toConnectionPoint)
        {
            throw new NotImplementedException();
        }
    }
}

using Mindscape.WpfDiagramming.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceMatrix.Diagramming.ViewModels
{
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

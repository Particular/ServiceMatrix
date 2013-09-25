using Mindscape.WpfDiagramming.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels
{
    public class NServiceBusConnectionBuilder : IDiagramConnectionBuilder
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

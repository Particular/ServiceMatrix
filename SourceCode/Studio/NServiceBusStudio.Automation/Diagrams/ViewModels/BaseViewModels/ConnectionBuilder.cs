using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mindscape.WpfDiagramming.Foundation;
using Mindscape.WpfDiagramming;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels.BaseViewModels
{
    public class ConnectionBuilder : IDiagramConnectionBuilder
    {
        public void CreateConnection(Diagram diagram, DiagramConnectionPoint fromConnectionPoint, DiagramConnectionPoint toConnectionPoint)
        {
            CollapsableConnection connection = new CollapsableConnection(fromConnectionPoint, toConnectionPoint);
            connection.ZOrder = 100; // To keep connections above all nodes when created.
            diagram.Connections.Add(connection);
        }

        void IDiagramConnectionBuilder.CreateConnection(IDiagramModel diagram, IDiagramConnectionPoint fromConnectionPoint, IDiagramConnectionPoint toConnectionPoint)
        {
            CreateConnection((Diagram)diagram, (DiagramConnectionPoint)fromConnectionPoint, (DiagramConnectionPoint)toConnectionPoint);
        }

        public virtual bool CanCreateConnection(IDiagramModel diagram, IDiagramConnectionPoint fromConnectionPoint, ConnectionDropTarget dropTarget)
        {
            return true;
        }
    }
}

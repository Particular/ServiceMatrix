using Mindscape.WpfDiagramming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels.Connections
{
    public class EventConnection : BaseConnection
    {
        public EventConnection(DiagramNode from, DiagramNode to)
            : base(from, to)
        {

        }
    }
}

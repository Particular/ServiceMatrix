using Mindscape.WpfDiagramming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceMatrix.Diagramming.ViewModels.Connections
{
    public class CommandConnection: BaseConnection
    {
        public CommandConnection(DiagramNode from, DiagramNode to)
            : base(from, to)
        {

        }
    }
}

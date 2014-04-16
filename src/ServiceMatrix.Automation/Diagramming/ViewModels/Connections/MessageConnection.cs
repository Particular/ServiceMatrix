using Mindscape.WpfDiagramming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMatrix.Diagramming.ViewModels.Connections
{
    public class MessageConnection : BaseConnection
    {
        public MessageConnection(DiagramNode from, DiagramNode to)
            : base(from, to)
        {

        }
    }
}

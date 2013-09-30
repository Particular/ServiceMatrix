using Mindscape.WpfDiagramming;
using NServiceBusStudio.Automation.Diagrams.ViewModels.BaseViewModels;
using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels.Shapes
{
    public abstract class MessageBaseNode : GroupableNode
    {
        public MessageBaseNode(IProductElementViewModel innerViewModel)
            : base(innerViewModel)
        {
            ConnectionPoints.Add(new DiagramConnectionPoint(this, Edge.Left));
            ConnectionPoints.Add(new DiagramConnectionPoint(this, Edge.Right));

            this.IsResizable = false;
            this.Bounds = new Rect(0, 0, 320, 52);
        }
    }
}

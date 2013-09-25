using Mindscape.WpfDiagramming;
using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels.Shapes
{
    public class EventNode : MessageBaseNode
    {
        public EventNode(IProductElementViewModel innerViewModel, Point position)
            : base(innerViewModel, position)
        {
        }
    }
}

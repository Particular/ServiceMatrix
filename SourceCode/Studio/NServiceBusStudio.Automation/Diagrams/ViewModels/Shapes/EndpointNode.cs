using Mindscape.WpfDiagramming;
using Mindscape.WpfDiagramming.Foundation;
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
    public class EndpointNode: GroupNode
    {
        public EndpointNode(IProductElementViewModel innerViewModel, Point position) : base (innerViewModel)
        {
            this.SHAPE_MIN_HEIGHT = 190;
            this.Bounds = new System.Windows.Rect(position.X, position.Y, 320, this.SHAPE_MIN_HEIGHT);
        }

        public string Type 
        {
            get { return "(" + this.InnerViewModel.Data.Info.Name + ")"; }
        }
    }
}

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
    public class ServiceNode: GroupNode
    {
        public ServiceNode(IProductElementViewModel innerViewModel, EndpointNode parent)
            : base(innerViewModel)
        {
            this.SHAPE_MIN_HEIGHT = 130;
            this.Bounds = new Rect(12, 40, 296, this.SHAPE_MIN_HEIGHT);

            this.SetParent(parent);
        }
    }
}

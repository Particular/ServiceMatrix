using ServiceMatrix.Diagramming.ViewModels.BaseViewModels;
using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiceMatrix.Diagramming.ViewModels.Shapes
{
    public class ServiceNode: GroupNode
    {
        public ServiceNode(IProductElementViewModel innerViewModel, EndpointNode parent)
            : base(innerViewModel)
        {
            SHAPE_MIN_HEIGHT = 130;
            Bounds = new Rect(12, 40, 296, SHAPE_MIN_HEIGHT);

            SetParent(parent);
        }
    }
}

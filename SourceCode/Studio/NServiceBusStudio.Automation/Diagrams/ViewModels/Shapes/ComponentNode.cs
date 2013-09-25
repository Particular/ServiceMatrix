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
    public class ComponentNode : ChildNode
    {
        public ComponentNode(IProductElementViewModel innerViewModel, ServiceNode parent)
            : base(innerViewModel)
        {
            this.IsResizable = false;
            
            this.Bounds = new Rect(12, 40, 276, 38);

            this.SetParent(parent);
        }
    }
}

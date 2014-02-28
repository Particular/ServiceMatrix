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
    public class ComponentNode : ChildNode
    {
        public IProductElementViewModel ComponentLinkViewModel { get; set; }

        public ComponentNode(IProductElementViewModel innerViewModel, ServiceNode parent, IProductElementViewModel componentLinkViewModel)
            : base(innerViewModel)
        {
            this.ComponentLinkViewModel = componentLinkViewModel;
            this.IsResizable = false;
            
            this.Bounds = new Rect(12, 40, 276, 38);

            this.SetParent(parent);
        }
    }
}

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
            ComponentLinkViewModel = componentLinkViewModel;
            IsResizable = false;
            
            Bounds = new Rect(12, 40, 276, 38);

            SetParent(parent);
        }

        public virtual bool IsSaga
        {
            get { return (bool)InnerViewModel.Data.Properties.First(x => x.DefinitionName == "IsSaga").Value; }
        }
    }
}

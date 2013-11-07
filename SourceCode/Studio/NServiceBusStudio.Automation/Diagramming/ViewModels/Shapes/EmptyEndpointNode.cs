using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiceMatrix.Diagramming.ViewModels.Shapes
{
    public class EmptyEndpointNode: EndpointNode
    {
        public static Guid NodeId = Guid.Empty;

        public EmptyEndpointNode(IMenuOptionViewModel deployUnhostedComponents): base(null)
        {
            _menuOptions = new ObservableCollection<IMenuOptionViewModel>();
            _menuOptions.Add(deployUnhostedComponents);
        }

        public override Guid Id
        {
            get { return NodeId; }
        }

        public override string Name
        {
            get { return "Empty Endpoint"; }
        }

        ObservableCollection<IMenuOptionViewModel> _menuOptions;
        public override ObservableCollection<IMenuOptionViewModel> MenuOptions 
        {
            get { return _menuOptions; }
        }
    }
}

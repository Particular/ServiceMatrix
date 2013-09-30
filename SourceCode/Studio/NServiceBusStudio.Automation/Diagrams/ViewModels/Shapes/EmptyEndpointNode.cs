using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels.Shapes
{
    public class EmptyEndpointNode: EndpointNode
    {
        public static Guid NodeId = Guid.Empty;

        public EmptyEndpointNode(): base(null)
        {
        }

        public override Guid Id
        {
            get { return NodeId; }
        }

        public override string Name
        {
            get { return "Empty Endpoint"; }
        }

        public override ObservableCollection<IMenuOptionViewModel> MenuOptions
        {
            get { return new ObservableCollection<IMenuOptionViewModel>(); }
        }
    }
}

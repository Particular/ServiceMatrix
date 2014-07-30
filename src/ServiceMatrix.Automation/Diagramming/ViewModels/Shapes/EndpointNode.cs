using System.Windows;
using NuPattern.Runtime.UI.ViewModels;
using ServiceMatrix.Diagramming.ViewModels.BaseViewModels;

namespace ServiceMatrix.Diagramming.ViewModels.Shapes
{
    public class EndpointNode : GroupNode
    {
        public EndpointNode(IProductElementViewModel innerViewModel)
            : base(innerViewModel)
        {
            SHAPE_MIN_HEIGHT = 190;
            Bounds = new Rect(0, 0, 320, SHAPE_MIN_HEIGHT);
        }

        public string Type
        {
            get
            {
                switch (InnerViewModel.Data.Info.Name)
                {
                    case "NServiceBusHost":
                        return "(NSB Host)";
                    case "NServiceBusMVC":
                        return "(ASP.NET MVC)";
                }

                return "";
            }
        }
    }
}

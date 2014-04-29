using Mindscape.WpfDiagramming;
using Mindscape.WpfDiagramming.Foundation;
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
    public class EndpointNode: GroupNode
    {
        public EndpointNode(IProductElementViewModel innerViewModel) : base (innerViewModel)
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
                        break;
                    case "NServiceBusMVC":
                        return "(ASP.NET MVC)";
                        break;
                    case "NServiceBusWeb":
                        return "(ASP.NET Web Forms)";
                        break;
                }

                return "";
            }
        }
    }
}

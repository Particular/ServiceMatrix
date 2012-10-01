using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;

namespace NServiceBus.Modeling.EndpointDesign
{
    public partial class EndpointDesignDiagram
    {
        public ShapeElement CreateShape(ModelElement element)
        {
            return this.CreateChildShape(element);
        }
    }
}

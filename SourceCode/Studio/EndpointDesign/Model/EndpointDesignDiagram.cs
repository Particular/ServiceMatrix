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

        public override void OnPaintShape(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPaintEventArgs e)
        {
            base.OnPaintShape(e);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawString("Powered by Particular NSB", new Font("Tahoma", 10, FontStyle.Bold), Brushes.Black, 7f, 0.1f);
            g.DrawString("Unlicense copy", new Font("Tahoma", 9, FontStyle.Bold), Brushes.Red, 7f, 0.25f);
        }
    }
}

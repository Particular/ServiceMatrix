using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using NServiceBusStudio;
using NuPattern.Runtime;

namespace NServiceBus.Modeling.EndpointDesign
{
    public partial class EndpointDesignDiagram
    {
        [Import(AllowDefault = true)]
        public IPatternManager ProductManager { get; set; }

        public IApplication GetNServiceBusStudioApplication()
        {
            if (this.ProductManager == null)
            {
                Microsoft.VisualStudio.Modeling.Shell.ModelingCompositionContainer.CompositionService.SatisfyImportsOnce(this);
            }

            if (this.ProductManager == null)
            {
                return null;
            }

            var app = this.ProductManager.Products.First().As<NServiceBusStudio.IApplication>();
            return app;
        }

        public override void OnPaintShape(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPaintEventArgs e)
        {
            base.OnPaintShape(e);

            var g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawString("Powered by Particular NSB", new Font("Tahoma", 10, FontStyle.Bold), Brushes.Black, 7f, 0.1f);
            g.DrawString("Unlicensed copy", new Font("Tahoma", 9, FontStyle.Bold), Brushes.Red, 7f, 0.25f);

            var app = GetNServiceBusStudioApplication();
            if (app != null)
            {
                g.DrawString(app.InstanceName, new Font("Tahoma", 10, FontStyle.Bold), Brushes.Gray, 0.1f, 0.1f);
                
                if (!String.IsNullOrEmpty(app.CompanyLogo) && System.IO.File.Exists(app.CompanyLogo))
                {
                    g.DrawImage(Bitmap.FromFile(app.CompanyLogo), 7f, 0.5f);
                }
            }
        }
    }
}

using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using ServiceMatrix.Diagramming.ViewModels;
using NuPattern;
using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;

namespace ServiceMatrix.Diagramming.Views
{
    [Guid("CB34A2CF-1CD7-46DA-B452-C4C9D75D6CE8")]
    public class ServiceMatrixDiagramToolWindow : ToolWindowPane
    {
        [Import]
        public ServiceMatrixDiagramAdapter NServiceBusDiagramAdapter { get; set; }

        public ServiceMatrixDiagramToolWindow() :
            base(null)
        {
            this.Caption = "ServiceMatrix - NServiceBus Diagram";
            this.BitmapResourceID = 301;
            this.BitmapIndex = 0;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var componentModel = this.GetService<SComponentModel, IComponentModel>();
            componentModel.DefaultCompositionService.SatisfyImportsOnce(this);

            var pane = new Diagram(this.NServiceBusDiagramAdapter);
            this.Content = pane;

            //pane.CaptionHasChanged += (s, e) =>
            //{
            //    this.Caption = pane.Caption;
            //};
        }
    }
}

namespace ServiceMatrix.Diagramming.Views
{
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Shell;
    using ServiceMatrix.Diagramming.ViewModels;
    using NuPattern;
    using System.ComponentModel.Composition;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell.Interop;

    [Guid("CB34A2CF-1CD7-46DA-B452-C4C9D75D6CE8")]
    public class ServiceMatrixDiagramToolWindow : ToolWindowPane
    {
        [Import]
        public ServiceMatrixDiagramAdapter NServiceBusDiagramAdapter { get; set; }

        public ServiceMatrixDiagramToolWindow() :
            base(null)
        {
            Caption = "ServiceMatrix - NServiceBus Canvas";
            BitmapResourceID = 301;
            BitmapIndex = 0;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var componentModel = this.GetService<SComponentModel, IComponentModel>();
            componentModel.DefaultCompositionService.SatisfyImportsOnce(this);

            NServiceBusDiagramAdapter.CloseWindow = () =>
            {
                var windowFrame = (IVsWindowFrame)Frame;
                windowFrame.Hide();
            };

            var pane = new Diagram(NServiceBusDiagramAdapter);
            Content = pane;
            
            //pane.CaptionHasChanged += (s, e) =>
            //{
            //    this.Caption = pane.Caption;
            //};
        }
    }
}

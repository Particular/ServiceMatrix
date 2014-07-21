using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NServiceBusStudio;
using NuPattern;
using ServiceMatrix.Diagramming.ViewModels;

namespace ServiceMatrix.Diagramming.Views
{
    [Guid("CB34A2CF-1CD7-46DA-B452-C4C9D75D6CE8")]
    public class ServiceMatrixDiagramToolWindow : ToolWindowPane
    {
        [Import]
        public ServiceMatrixDiagramAdapter NServiceBusDiagramAdapter { get; set; }

        [Required]
        [Import(AllowDefault = true)]
        private IDialogWindowFactory WindowFactory { get; set; }

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

            var pane = new Diagram(NServiceBusDiagramAdapter, WindowFactory);
            Content = pane;
        }
    }
}

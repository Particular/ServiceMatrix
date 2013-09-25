using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using NServiceBusStudio.Automation.Diagrams.ViewModels;
using NuPattern;
using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;

namespace NServiceBusStudio.Automation.Diagrams.Views
{
    [Guid("CB34A2CF-1CD7-46DA-B452-C4C9D75D6CE8")]
    public class NServiceBusDiagramsToolWindow : ToolWindowPane
    {
        [Import]
        public NServiceBusDiagramAdapter NServiceBusDiagramAdapter { get; set; }

        public NServiceBusDiagramsToolWindow() :
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

            var pane = new Diagram(this.NServiceBusDiagramAdapter.ViewModel);
            this.Content = pane;

            //pane.CaptionHasChanged += (s, e) =>
            //{
            //    this.Caption = pane.Caption;
            //};
        }
    }
}

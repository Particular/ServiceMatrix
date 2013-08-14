using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ComponentModelHost;

using NuPattern.Runtime.Shell.Properties;
using NuPattern.Runtime.UI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NServiceBusStudio.Automation.Diagrams.Views
{
    [Guid("CB34A2CF-1CD7-46DA-B452-C4C9D75D6CE8")]
    public class NServiceBusDiagramsToolWindow : ToolWindowPane
    {
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

            var pane = new NewDiagram();
            this.Content = pane;

            //pane.CaptionHasChanged += (s, e) =>
            //{
            //    this.Caption = pane.Caption;
            //};
        }
    }
}

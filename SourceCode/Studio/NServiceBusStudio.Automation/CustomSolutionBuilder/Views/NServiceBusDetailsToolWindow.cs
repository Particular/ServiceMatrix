using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ComponentModelHost;

using NuPattern.Runtime.Shell.Properties;
using NuPattern.Runtime.UI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;


namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Views
{
    [Guid("ED9FB3C8-D7B6-43FC-A596-132EF7B38B5D")]
    public class NServiceBusDetailsToolWindow : ToolWindowPane
    {
        public NServiceBusDetailsToolWindow() :
            base(null)
        {
            this.Caption = "ServiceMatrix Details";
            this.BitmapResourceID = 301;
            this.BitmapIndex = 0;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var pane = new DetailsPanel();
            this.Content = pane;

            pane.CaptionHasChanged += (s, e) =>
            {
                this.Caption = pane.Caption;
            };
        }
    }
}

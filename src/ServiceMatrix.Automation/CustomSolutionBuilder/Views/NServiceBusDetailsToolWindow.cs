namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Views
{
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    [Guid("ED9FB3C8-D7B6-43FC-A596-132EF7B38B5D")]
    public class NServiceBusDetailsToolWindow : ToolWindowPane
    {
        public NServiceBusDetailsToolWindow() :
            base(null)
        {
            Caption = "ServiceMatrix Details";
            BitmapResourceID = 301;
            BitmapIndex = 0;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var pane = new DetailsPanel();
            Content = pane;

            pane.CaptionHasChanged += (s, e) =>
            {
                Caption = pane.Caption;
            };
        }
    }
}

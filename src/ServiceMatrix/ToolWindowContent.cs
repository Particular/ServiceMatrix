using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows;

namespace NServiceBusStudio
{
    [Guid("1193C9AC-C7F7-4E33-BDE3-67632C74C7F8")]
    public class ToolWindowContent : ToolWindowPane
    {
        private DockPanel panel = new DockPanel { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };

        public ToolWindowContent()
        {
            BitmapResourceID = 300;
            BitmapIndex = 1;

            Content = panel;
        }

        public void SetContent(FrameworkElement content)
        {
            panel.Children.Clear();
            panel.Children.Add(content);
        }
    }
}

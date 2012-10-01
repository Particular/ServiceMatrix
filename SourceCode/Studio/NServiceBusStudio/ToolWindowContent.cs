using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            this.BitmapResourceID = 300;
            this.BitmapIndex = 1;

            this.Content = this.panel;
        }

        public void SetContent(FrameworkElement content)
        {
            this.panel.Children.Clear();
            this.panel.Children.Add(content);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace NServiceBusStudio.Automation.Diagrams.Styles
{
    public partial class DiagramNodeStyle
    {
        public DiagramNodeStyle()
        {
            InitializeComponent();
        }

        private void ShowMenu_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;

            button.ContextMenu.DataContext = button.DataContext;
            button.ContextMenu.IsOpen = true;
        }
    }
}

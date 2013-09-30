using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NServiceBusStudio.Automation.Diagrams.Styles
{
    public partial class DiagramSharedStyle
    {
        public DiagramSharedStyle()
        {
            InitializeComponent();
        }

        private void OnClickShowMenu(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;

            button.ContextMenu.DataContext = button.DataContext;
            button.ContextMenu.IsOpen = true;
            OnContextMenuOpening(sender, null);
        }

        private void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var item = sender as Button;

            if (item != null)
            {
                var type = Type.GetType("NuPattern.Runtime.UI.ViewModels.AutomationMenuOptionViewModel, NuPattern.Runtime.Core");
                var method = type.GetMethod("ReEvaluateCommand", BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null)
                {
                    foreach (var i in item.ContextMenu.Items)
                    {
                        if (i.GetType() == type)
                        {
                            method.Invoke(i, null);
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Views
{
    /// <summary>
    /// Interaction logic for InnerPanel.xaml
    /// </summary>
    public partial class InnerPanel : UserControl
    {
        public InnerPanel()
        {
            InitializeComponent();
        }

        public Action<IProductElement> NavigationAction { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationAction != null)
            {
                this.NavigationAction(((sender as FrameworkElement).DataContext as InnerPanelItem).Product);
            }
        }
    }
}

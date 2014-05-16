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
using System.Windows.Markup;
using NuPattern.Runtime;
using System.ComponentModel;
using NuPattern.Presentation;
using System.ComponentModel.DataAnnotations;

namespace NServiceBusStudio.Automation.Dialog
{
    /// <summary>
    /// Interaction logic for ElementPicker.xaml
    /// </summary>
    public partial class ElementHierarchyPicker : CommonDialogWindow, IDialogWindow, IComponentConnector
    {
        public ElementHierarchyPicker()
        {
            InitializeComponent();
        }
    }
}

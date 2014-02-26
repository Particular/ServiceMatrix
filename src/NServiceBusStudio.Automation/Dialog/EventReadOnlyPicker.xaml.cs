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
using NuPattern.Runtime;
using System.Windows.Markup;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.Dialog
{
    /// <summary>
    /// Interaction logic for EventReadOnlyPicker.xaml
    /// </summary>
    public partial class EventReadOnlyPicker : CommonDialogWindow, IDialogWindow, IElementPicker, IComponentConnector
    {
        public EventReadOnlyPicker()
        {
            InitializeComponent();
            this.SelectedItem = "";
        }

        protected override void OnActivated(EventArgs e)
        {
            this.DataContext = this;
            base.OnActivated(e);
        }

        public string MasterName
        {
            get;
            set;
        }

        public ICollection<string> Elements
        {
            get;
            set;
        }

        public string SelectedItem
        {
            get;
            set;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }

}

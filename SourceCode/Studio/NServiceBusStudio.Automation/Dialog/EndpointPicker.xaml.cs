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
using NuPattern.Common.Presentation;
using NuPattern.Runtime;
using NServiceBusStudio.Automation.Dialog;
using System.Windows.Markup;
using System.Collections.ObjectModel;

namespace AbstractEndpoint.Automation.Dialog
{
    /// <summary>
    /// Interaction logic for ComponentPicker.xaml
    /// </summary>
    public partial class EndpointPicker : CommonDialogWindow, IDialogWindow, IServicePicker, IComponentConnector
    {
        public EndpointPicker()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            this.DataContext = this;
            base.OnActivated(e);
        }

        public ObservableCollection<string> Elements
        {
            get;
            set;
        }

        public ICollection<string> SelectedItems
        {
            get;
            set;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedItems = new List<string> { this.EndpointSelector.SelectedValue.ToString() }; //selecteditems is not a dependencyproperty and there are known bugs with binding //may be fixed properly
            this.DialogResult = true;
            this.Close();
        }
    }

}

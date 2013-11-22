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
using NServiceBusStudio.Automation.Dialog;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using NuPattern.Presentation;

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
            private set;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.AddEndpointText.Text))
            {
                AddEndpointItem();
            }
            this.SelectedItems = this.EndpointsList.SelectedItems.OfType<string>().ToList(); //selecteditems is not a dependencyproperty and there are known bugs with binding //may be fixed properly
            this.DialogResult = true;
            this.Close();
        }

        private void AddEndpoint_Click(object sender, RoutedEventArgs e)
        {
            this.AddEndpointText.Text = this.AddEndpointText.Text.Trim();
            if (this.AddEndpointText.Text.Length > 0)
            {
                AddEndpointItem();
                AddEndpointCancel_Click(null, null);
            }
        }

        private void AddEndpointItem()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var endpoint = String.Format ("{0} [{1}]", this.AddEndpointText.Text, this.AddEndpointType.Text);
                Elements.Add(endpoint);
                this.EndpointsList.SelectedItems.Add(endpoint);
                this.AddEndpointText.Text = string.Empty;
            }));
        }

        private void NewEndpoint_Click(object sender, RoutedEventArgs e)
        {
            this.NewEndpoint.Visibility = System.Windows.Visibility.Visible;
            this.AddEndpointText.Text = "";
        }

        private void AddEndpointCancel_Click(object sender, RoutedEventArgs e)
        {
            this.NewEndpoint.Visibility = System.Windows.Visibility.Collapsed;
        }
        
    }

}

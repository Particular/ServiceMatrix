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
    public partial class EndpointPicker : CommonDialogWindow, IDialogWindow, IEndpointPicker, IComponentConnector
    {
        public EndpointPicker()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            DataContext = this;
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

        public string ComponentName
        {
            get;
            set;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AddEndpointText.Text))
            {
                AddEndpointItem();
            }
            SelectedItems = EndpointsList.SelectedItems.OfType<string>().ToList(); //selecteditems is not a dependencyproperty and there are known bugs with binding //may be fixed properly
            DialogResult = true;
            Close();
        }

        private void AddEndpoint_Click(object sender, RoutedEventArgs e)
        {
            AddEndpointText.Text = AddEndpointText.Text.Trim();
            if (AddEndpointText.Text.Length > 0)
            {
                AddEndpointItem();
                AddEndpointCancel_Click(null, null);
            }
        }

        private void AddEndpointItem()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var endpoint = String.Format ("{0} [{1}]", AddEndpointText.Text, AddEndpointType.Text);
                Elements.Add(endpoint);
                EndpointsList.SelectedItems.Add(endpoint);
                AddEndpointText.Text = string.Empty;
            }));
        }

        private void NewEndpoint_Click(object sender, RoutedEventArgs e)
        {
            NewEndpoint.Visibility = Visibility.Visible;
            AddEndpointText.Text = "";
            AddEndpointText.Focus();
        }

        private void AddEndpointCancel_Click(object sender, RoutedEventArgs e)
        {
            NewEndpoint.Visibility = Visibility.Collapsed;
        }
        
    }

}

namespace AbstractEndpoint.Automation.Dialog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using NServiceBusStudio.Automation.Dialog;
    using System.Collections.ObjectModel;
    using NuPattern.Presentation;

    /// <summary>
    /// Interaction logic for ComponentPicker.xaml
    /// </summary>
    public partial class EndpointPicker : IDialogWindow, IEndpointPicker
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

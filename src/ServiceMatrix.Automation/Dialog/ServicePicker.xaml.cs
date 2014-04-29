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
using System.Collections.ObjectModel;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.Dialog
{
    /// <summary>
    /// Interaction logic for ServicePicker.xaml
    /// </summary>
    public partial class ServicePicker : CommonDialogWindow, IDialogWindow, IServicePicker, IComponentConnector
    {
        public ServicePicker()
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

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AddServiceText.Text))
            {
                AddTextItem();
            }
            SelectedItems = ServicesList.SelectedItems.OfType<string>().ToList(); //selecteditems is not a dependencyproperty and there are known bugs with binding //may be fixed properly
            DialogResult = true;
            Close();
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            AddServiceText.Text = AddServiceText.Text.Trim();
            if (AddServiceText.Text.Length > 0)
            {
                AddTextItem();
            }
        }

        private void AddTextItem()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Elements.Add(AddServiceText.Text);
                ServicesList.SelectedItems.Add(AddServiceText.Text);
                AddServiceText.Text = string.Empty;
            }));
        }
    }
}

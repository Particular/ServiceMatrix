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
using System.Collections.ObjectModel;

namespace NServiceBusStudio.Automation.Dialog
{
    /// <summary>
    /// Interaction logic for EventReadOnlyPicker.xaml
    /// </summary>
    public partial class EventReadOnlyPicker : CommonDialogWindow, IDialogWindow, IEventPicker, IComponentConnector
    {
        public EventReadOnlyPicker()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            DataContext = this;
            base.OnActivated(e);
        }

        public string MasterName
        {
            get;
            set;
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
            SelectedItems = EventList.SelectedItems.OfType<string>().ToList(); //selecteditems is not a dependencyproperty and there are known bugs with binding //may be fixed properly
            DialogResult = true;
            Close();
        }
    }

}

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
    /// Interaction logic for SagaStarterPicker.xaml
    /// </summary>
    public partial class SagaStarterPicker : CommonDialogWindow, IDialogWindow, IEventPicker, IComponentConnector
    {
        public SagaStarterPicker()
        {
            InitializeComponent();
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

        public ObservableCollection<SagaStarter> InternalElements
        {
            get
            {
                var list = new List<SagaStarter>();
                this.Elements.ToList().ForEach(x => list.Add(new SagaStarter() { Name = x, IsSelected = this.SelectedItems.Contains(x) }));
                return new ObservableCollection<SagaStarter>(list);
            }
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
            this.SelectedItems = this.MessagesList.Items.OfType<SagaStarter>().Where(x => x.IsSelected).Select(x => x.Name).ToList(); //selecteditems is not a dependencyproperty and there are known bugs with binding //may be fixed properly
            this.DialogResult = true;
            this.Close();
        }
    }

    public class SagaStarter
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }

}

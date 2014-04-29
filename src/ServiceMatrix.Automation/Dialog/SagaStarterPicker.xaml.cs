namespace NServiceBusStudio.Automation.Dialog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using NuPattern.Presentation;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for SagaStarterPicker.xaml
    /// </summary>
    public partial class SagaStarterPicker : IDialogWindow, IEventPicker
    {
        public SagaStarterPicker()
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

        public ObservableCollection<SagaStarter> InternalElements
        {
            get
            {
                var list = new List<SagaStarter>();
                Elements.ToList().ForEach(x => list.Add(new SagaStarter() { Name = x, IsSelected = SelectedItems.Contains(x) }));
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
            SelectedItems = MessagesList.Items.OfType<SagaStarter>().Where(x => x.IsSelected).Select(x => x.Name).ToList(); //selecteditems is not a dependencyproperty and there are known bugs with binding //may be fixed properly
            DialogResult = true;
            Close();
        }
    }

    public class SagaStarter
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }

}

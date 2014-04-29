namespace NServiceBusStudio.Automation.Dialog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using NuPattern.Presentation;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for EventReadOnlyPicker.xaml
    /// </summary>
    public partial class EventReadOnlyPicker : IDialogWindow, IEventPicker
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

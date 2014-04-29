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
    public partial class ComponentPicker : IDialogWindow, IServicePicker
    {
        public ComponentPicker()
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
            SelectedItems = ServicesList.SelectedItems.OfType<string>().ToList(); //selecteditems is not a dependencyproperty and there are known bugs with binding //may be fixed properly
            DialogResult = true;
            Close();
        }
    }

}

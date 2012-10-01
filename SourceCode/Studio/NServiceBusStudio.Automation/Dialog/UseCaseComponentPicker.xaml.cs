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
using Microsoft.VisualStudio.Patterning.Common.Presentation;
using Microsoft.VisualStudio.Patterning.Runtime;
using NServiceBusStudio.Automation.Dialog;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using NServiceBusStudio;
using System.ComponentModel;

namespace AbstractEndpoint.Automation.Dialog
{
    /// <summary>
    /// Interaction logic for ComponentPicker.xaml
    /// </summary>
    public partial class UseCaseComponentPicker : CommonDialogWindow, IDialogWindow, INotifyPropertyChanged
    {
        public UseCaseComponentPicker()
        {
            InitializeComponent();
        }

        private IEnumerable<IService> Services { get; set; }

        public void SetServices(IEnumerable<IService> services)
        {
            this.Services = services;
            this.ServiceNameSelector.ItemsSource = services.Select(s => s.InstanceName);
        }

        public string SelectedComponent { get; set; }
        public string SelectedService { get; set; }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.SelectedComponent))
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        public void SetComponentLabel(string value)
        {
            this.ComponentLabel.Content = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Func<IService, IEnumerable<string>> ServiceItemsFillFunction = s => null;

        private void ServiceName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SelectedService = this.ServiceNameSelector.Text;
            var selectedSvc = this.Services.FirstOrDefault(s => s.InstanceName == this.SelectedService);
            if (selectedSvc != null)
            {
                this.ComponentNameSelector.ItemsSource = this.ServiceItemsFillFunction(selectedSvc);
            }
            else
            {
                this.ComponentNameSelector.ItemsSource = new string[] {};
            }
        }

        private void ComponentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SelectedComponent = this.ComponentNameSelector.Text;
        }
    }

}

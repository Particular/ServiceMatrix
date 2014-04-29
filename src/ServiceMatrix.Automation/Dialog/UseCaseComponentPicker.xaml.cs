namespace AbstractEndpoint.Automation.Dialog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using NServiceBusStudio;
    using System.ComponentModel;
    using System.Windows.Threading;
    using NuPattern.Presentation;


    /// <summary>
    /// Interaction logic for ComponentPicker.xaml
    /// </summary>
    public partial class UseCaseComponentPicker : IDialogWindow, INotifyPropertyChanged
    {

        public UseCaseComponentPicker()
        {
            InitializeComponent();
            DispatcherTimer focusTimer = null;
            focusTimer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(200),
                DispatcherPriority.Input,
                (s, e) => 
                {
                    focusTimer.Stop();
                    focusTimer.IsEnabled = false;
                    var textBox = (ServiceNameSelector.Template.FindName("PART_EditableTextBox", ServiceNameSelector) as TextBox);
                        if (textBox != null)
                        {
                            textBox.Focus();
                            textBox.SelectionStart = textBox.Text.Length;
                        }
                },
                Dispatcher)
            {
                IsEnabled = true
            };
            focusTimer.Start();
        }

        private IEnumerable<IService> Services { get; set; }

        public void SetServices(IEnumerable<IService> services)
        {
            Services = services;
            ServiceNameSelector.ItemsSource = services.Select(s => s.InstanceName);
        }

        public string SelectedComponent { get; set; }
        public string SelectedService { get; set; }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedComponent))
            {
                DialogResult = true;
                Close();
            }
        }

        public void SetComponentLabel(string value)
        {
            ComponentLabel.Content = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Func<IService, IEnumerable<string>> ServiceItemsFillFunction = s => null;

        private void ServiceName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectedService = ServiceNameSelector.Text;
            var selectedSvc = Services.FirstOrDefault(s => s.InstanceName == SelectedService);
            if (selectedSvc != null)
            {
                ComponentNameSelector.ItemsSource = ServiceItemsFillFunction(selectedSvc);
            }
            else
            {
                ComponentNameSelector.ItemsSource = new string[] {};
            }
        }

        private void ComponentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectedComponent = ComponentNameSelector.Text;
        }
    }

}

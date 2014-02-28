using NServiceBusStudio.Automation.Diagramming.Views;
using NuPattern.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiceMatrix.Diagramming.ViewModels
{
    public class ServiceMatrixDiagramViewModel : INotifyPropertyChanged
    {
        public ServiceMatrixDiagramAdapter Adapter { get; set; }
        public ServiceMatrixDiagramMindscapeViewModel Diagram { get; set; }

        public ServiceMatrixDiagramViewModel(ServiceMatrixDiagramAdapter adapter)
        {
            this.Adapter = adapter;
            this.Diagram = adapter.ViewModel;

            this.OnShowAddEndpoint = new RelayCommand(() => {
                var window = new AddEndpoint();
                var result = window.ShowDialog();

                if ((result.HasValue ? result.Value : false) && !String.IsNullOrEmpty(window.EndpointName.Text))
                {
                    try
                    {
                        this.Adapter.AddEndpoint(window.EndpointName.Text, window.EndpointHostType.SelectedValue.ToString());
                    }
                    catch (OperationCanceledException) { }
                }
            });

            this.OnShowAddService = new RelayCommand(() =>
            {
                var window = new AddService();
                var result = window.ShowDialog();

                if ((result.HasValue ? result.Value : false) && !String.IsNullOrEmpty(window.ServiceName.Text))
                {
                    try
                    {
                        this.Adapter.AddService(window.ServiceName.Text);
                    }
                    catch (OperationCanceledException) { }
                }
            });
        }

        
        public System.Windows.Input.ICommand OnShowAddService { get; set; }
        public System.Windows.Input.ICommand OnShowAddEndpoint { get; set; }
       
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}

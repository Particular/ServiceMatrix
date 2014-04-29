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
    using System.Windows.Input;
    using NServiceBusStudio.Automation.Model;

    public class ServiceMatrixDiagramViewModel : INotifyPropertyChanged
    {
        public ServiceMatrixDiagramAdapter Adapter { get; set; }
        public ServiceMatrixDiagramMindscapeViewModel Diagram { get; set; }

        public bool IsServiceMatrixLicenseExpired { get; set; }

        public ServiceMatrixDiagramViewModel(ServiceMatrixDiagramAdapter adapter)
        {
            Adapter = adapter;
            Diagram = adapter.ViewModel;
            IsServiceMatrixLicenseExpired = !GlobalSettings.Instance.IsLicenseValid ;

            OnShowAddEndpoint = new RelayCommand(() => {
                var window = new AddEndpoint();
                var result = window.ShowDialog();

                if ((result.HasValue ? result.Value : false) && !String.IsNullOrEmpty(window.EndpointName.Text))
                {
                    try
                    {
                        Adapter.AddEndpoint(window.EndpointName.Text, window.EndpointHostType.SelectedValue.ToString());
                    }
                    catch (OperationCanceledException) { }
                }
            });

            OnShowAddService = new RelayCommand(() =>
            {
                var window = new AddService();
                var result = window.ShowDialog();

                if ((result.HasValue ? result.Value : false) && !String.IsNullOrEmpty(window.ServiceName.Text))
                {
                    try
                    {
                        Adapter.AddService(window.ServiceName.Text);
                    }
                    catch (OperationCanceledException) { }
                }
            });
        }

        
        public ICommand OnShowAddService { get; set; }
        public ICommand OnShowAddEndpoint { get; set; }
       
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}

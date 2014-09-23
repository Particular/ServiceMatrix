using System;
using System.ComponentModel;
using NServiceBusStudio;
using NServiceBusStudio.Automation.Diagramming.ViewModels;
using NServiceBusStudio.Automation.Diagramming.Views;
using NServiceBusStudio.Automation.Model;
using NuPattern.Presentation;
using ICommand = System.Windows.Input.ICommand;

namespace ServiceMatrix.Diagramming.ViewModels
{
    public class ServiceMatrixDiagramViewModel : INotifyPropertyChanged
    {
        public ServiceMatrixDiagramAdapter Adapter { get; set; }

        public ServiceMatrixDiagramMindscapeViewModel Diagram { get; set; }

        public bool IsServiceMatrixLicenseExpired { get; set; }

     
        public ServiceMatrixDiagramViewModel(ServiceMatrixDiagramAdapter adapter, IDialogWindowFactory windowFactory)
        {
            Adapter = adapter;
            Diagram = adapter.ViewModel;
            IsServiceMatrixLicenseExpired = !GlobalSettings.Instance.IsLicenseValid;

            Adapter.DiagramModeChanged += AdapterOnDiagramModeChanged;
            OnShowAddEndpoint = new RelayCommand(() =>
            {
                var viewModel = new AddEndpointViewModel();
                var dialog = windowFactory.CreateDialog<AddEndpoint>(viewModel);
                var result = dialog.ShowDialog();

                if (result.GetValueOrDefault() && !String.IsNullOrEmpty(viewModel.EndpointName))
                {
                    try
                    {
                        Adapter.AddEndpoint(viewModel.EndpointName, viewModel.EndpointType);
                    }
                    catch (OperationCanceledException) { }
                }
            });

            OnShowAddService = new RelayCommand(() =>
            {
                var viewModel = new AddServiceViewModel();
                var dialog = windowFactory.CreateDialog<AddService>(viewModel);
                var result = dialog.ShowDialog();

                if (result.GetValueOrDefault() && !String.IsNullOrEmpty(viewModel.ServiceName))
                {
                    try
                    {
                        Adapter.AddService(viewModel.ServiceName);
                    }
                    catch (OperationCanceledException) { }
                }
            });
        }

        void AdapterOnDiagramModeChanged(object sender, DiagramModeEventArgs diagramModeEventArgs)
        {
            IsReadOnly = diagramModeEventArgs.IsReadOnlyMode;
        }

        public ICommand OnShowAddService { get; set; }

        public ICommand OnShowAddEndpoint { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        bool isReadOnly;

        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            set
            {
                if (value != isReadOnly)
                {
                    isReadOnly = value;
                    NotifyPropertyChanged("IsReadOnly");
                }
            }
        }
    }
}

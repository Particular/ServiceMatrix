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

        public ICommand OnShowAddService { get; set; }

        public ICommand OnShowAddEndpoint { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

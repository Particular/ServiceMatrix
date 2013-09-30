using NuPattern.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels
{
    public class NServiceBusDiagramViewModel : INotifyPropertyChanged
    {
        public NServiceBusDiagramAdapter Adapter { get; set; }
        public NServiceBusDiagramMindscapeViewModel Diagram { get; set; }

        public NServiceBusDiagramViewModel(NServiceBusDiagramAdapter adapter)
        {
            this.Adapter = adapter;
            this.Diagram = adapter.ViewModel;

            this.OnShowAddEndpoint = new RelayCommand(() => this.ShowAddEndpoint = true);
            this.OnShowAddService = new RelayCommand(() => this.ShowAddService = true);
            this.OnAddEndpoint = new RelayCommand(AddEndpoint, () => !string.IsNullOrEmpty (this.EndpointName));
            this.OnAddService = new RelayCommand(AddService, () => !string.IsNullOrEmpty(this.ServiceName));
            this.OnCancel = new RelayCommand(ClearValues);

            this.ClearValues();
        }

        private void ClearValues()
        {
            this.ShowAddEndpoint = false;
            this.EndpointName = "";
            this.EndpointHostType = "NServiceBusHost";

            this.ShowAddService = false;
            this.ServiceName = "";
        }

        private void AddEndpoint()
        {
            this.Adapter.AddEndpoint(this.EndpointName, this.EndpointHostType);
            this.ClearValues();
        }

        private void AddService()
        {
            this.Adapter.AddService(this.ServiceName);
            this.ClearValues();
        }

        
        public System.Windows.Input.ICommand OnShowAddService { get; set; }
        public System.Windows.Input.ICommand OnShowAddEndpoint { get; set; }
        public System.Windows.Input.ICommand OnAddEndpoint { get; set; }
        public System.Windows.Input.ICommand OnAddService { get; set; }
        public System.Windows.Input.ICommand OnCancel { get; set; }

        // Add Endpoint
        private bool _showAddEndpoint;
        public bool ShowAddEndpoint 
        {
            get { return _showAddEndpoint; }
            set
            {
                _showAddEndpoint = value;
                this.OnPropertyChange("ShowAddEndpoint");
            }
        }

        private string _endpointName;
        public string EndpointName 
        {
            get { return _endpointName; }
            set
            {
                _endpointName = value;
                this.OnPropertyChange("EndpointName");
            }
        }

        private string _endpointHostType;
        public string EndpointHostType 
        {
            get { return _endpointHostType; }
            set
            {
                _endpointHostType = value;
                this.OnPropertyChange("EndpointHostType");
            }
        }
        
        // Add Service
        private bool _showAddService;
        public bool ShowAddService
        {
            get { return _showAddService; }
            set
            {
                _showAddService = value;
                this.OnPropertyChange("ShowAddService");
            }
        }

        private string _serviceName;
        public string ServiceName 
        {
            get { return _serviceName; }
            set
            {
                _serviceName = value;
                this.OnPropertyChange("ServiceName");
            }
        }

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

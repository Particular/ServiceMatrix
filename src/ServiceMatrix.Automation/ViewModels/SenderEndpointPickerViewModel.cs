using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AbstractEndpoint;
using NuPattern.Presentation;
using System.Collections.Generic;

namespace NServiceBusStudio.Automation.ViewModels
{
    public class SenderEndpointPickerViewModel : ViewModel
    {
        [Obsolete("Only available to support design-time data", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SenderEndpointPickerViewModel()
        {
            Endpoints = new ObservableCollection<EndpointViewModel>();
        }

        public SenderEndpointPickerViewModel(IApplication app, Func<IAbstractEndpoint, bool> alreadySendingPredicate)
        {
            Endpoints =
                new ObservableCollection<EndpointViewModel>(
                    app.Design.Endpoints.GetAll()
                        .Select(
                            e =>
                            {
                                var alreadySending = alreadySendingPredicate(e);
                                return new EndpointViewModel
                                {
                                    Name = e.InstanceName,
                                    Send = alreadySending,
                                    NotExistingSender = !alreadySending,
                                    Endpoint = e
                                };
                            })
                        .OrderBy(vm => vm.Name));

            InitializeCommands();
        }

        public System.Windows.Input.ICommand AcceptCommand
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            set;
        }

        public ObservableCollection<EndpointViewModel> Endpoints
        {
            get;
            private set;
        }

        public IEnumerable<IAbstractEndpoint> SelectedEndpoints
        {
            get { return Endpoints.Where(vm => vm.Send && vm.NotExistingSender).Select(vm => vm.Endpoint); }
        }

        private void CloseDialog(dynamic dialog)
        {
            dialog.DialogResult = true;
            dialog.Close();
        }

        private void InitializeCommands()
        {
            AcceptCommand = new RelayCommand<dynamic>(dialog => CloseDialog(dialog), dialog => HasNewSelections());
        }

        private bool HasNewSelections()
        {
            return Endpoints.Any(e => e.Send && e.NotExistingSender);
        }
    }

    public class EndpointViewModel : ViewModel
    {
        public string Name { get; set; }

        public bool Send { get; set; }

        public bool NotExistingSender { get; set; }

        public IAbstractEndpoint Endpoint { get; set; }
    }
}

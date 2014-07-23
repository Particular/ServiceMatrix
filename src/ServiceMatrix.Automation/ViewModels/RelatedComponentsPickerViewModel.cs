using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AbstractEndpoint;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    using System.Globalization;

    public class RelatedComponentsPickerViewModel : ViewModel
    {
        [Obsolete("Only available to support design-time data", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RelatedComponentsPickerViewModel()
        {
            Components = new ObservableCollection<SelectItemViewModel>();
        }

        public RelatedComponentsPickerViewModel(IEnumerable<IComponent> components, ICommand command)
        {
            Components = GetComponentsCollection(components, command);

            InitializeCommands();
        }

        private ObservableCollection<SelectItemViewModel> GetComponentsCollection(IEnumerable<IComponent> components, ICommand command)
        {
            var service = command.GetParentService();

            var allDeployedComponents =
                service.Parent.Parent
                    .Endpoints.GetAll()
                    .SelectMany(ep => ep.EndpointComponents.AbstractComponentLinks.Where(l => l.ComponentReference.Value.Parent.Parent == service))
                    .ToDictionary(l => l.ComponentReference.Value, l => l.ParentEndpointComponents.ParentEndpoint);

            return new ObservableCollection<SelectItemViewModel>(
                components
                    .Select(
                        c =>
                        {
                            IAbstractEndpoint endpoint;
                            return new SelectItemViewModel
                            {
                                Item = c,
                                Name = 
                                    string.Format(
                                        CultureInfo.CurrentCulture, 
                                        "{0} ({1})", 
                                        c.InstanceName, 
                                        (allDeployedComponents.TryGetValue(c, out endpoint) ? endpoint.InstanceName : "undeployed")),
                                IsSelected =
                                    c.Publishes.CommandLinks.All(l => l.CommandReference.Value == command)
                                    && !c.Publishes.EventLinks.Any()
                                    && c.Subscribes.ProcessedCommandLinks.All(l => l.CommandReference.Value == command)
                                    && !c.Publishes.EventLinks.Any()
                            };
                        })
                    .OrderBy(n => n.Name));
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

        public ObservableCollection<SelectItemViewModel> Components
        {
            get;
            private set;
        }

        public IEnumerable<IComponent> SelectedComponents
        {
            get { return Components.Where(vm => vm.IsSelected).Select(vm => (IComponent)vm.Item); }
        }

        private void CloseDialog(IDialogWindow dialog)
        {
            dialog.DialogResult = true;
            dialog.Close();
        }

        private void InitializeCommands()
        {
            AcceptCommand = new RelayCommand<IDialogWindow>(dialog => CloseDialog(dialog), dialog => true);
        }
    }
}

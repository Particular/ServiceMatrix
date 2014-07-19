using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FluentValidation;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    public class EndpointPickerViewModel : ValidatingViewModel
    {
        [Obsolete("Only available to support design-time data", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EndpointPickerViewModel()
        {
        }

        public EndpointPickerViewModel(ICollection<string> elements)
        {
            elements = elements ?? new List<string>();
            Elements = new ObservableCollection<SelectItemViewModel>(elements.Select(x => new SelectItemViewModel { Name = x }));
            CreateEndpoint = new CreateEndpointViewModel(this);

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

        public string ComponentName
        {
            get;
            set;
        }

        public ObservableCollection<SelectItemViewModel> Elements
        {
            get;
            private set;
        }

        public ICollection<string> SelectedItems
        {
            get { return Elements.Where(x => x.IsSelected).Select(x => x.Name).ToList(); }
            set
            {
                if (value != null)
                {
                    value.ToList().ForEach(x => Elements.Add(new SelectItemViewModel { Name = x, IsSelected = true }));
                }
            }
        }

        private bool _addNewEndpointVisible;
        public bool AddNewEndpointVisible
        {
            get { return _addNewEndpointVisible; }
            set
            {
                _addNewEndpointVisible = value;
                OnPropertyChanged(() => AddNewEndpointVisible);
            }
        }

        public CreateEndpointViewModel CreateEndpoint
        {
            get;
            private set;
        }

        public void AddEndpoint()
        {
            CreateEndpoint.AddEndpointText = string.Empty;
            AddNewEndpointVisible = true;
        }

        public void CancelEndpoint()
        {
            AddNewEndpointVisible = false;
        }

        protected override IValidator CreateValidator()
        {
            return new EndpointPickerViewModelValidator();
        }

        private void CloseDialog(dynamic dialog)
        {
            dialog.DialogResult = true;
            dialog.Close();
        }

        private void InitializeCommands()
        {
            AcceptCommand = new RelayCommand<dynamic>(dialog => CloseDialog(dialog), dialog => IsValid);
        }

        private void AddElement(string endpoint)
        {
            foreach (var element in Elements)
            {
                element.IsSelected = false;
            }

            Elements.Add(new SelectItemViewModel { Name = endpoint, IsSelected = true });
            AddNewEndpointVisible = false;
        }

        public class CreateEndpointViewModel : ValidatingViewModel
        {
            EndpointPickerViewModel parent;

            public CreateEndpointViewModel(EndpointPickerViewModel parentViewModel)
            {
                parent = parentViewModel;
                EndpointTypes = new ObservableCollection<string> { "NServiceBus Host", "NServiceBus ASP.NET MVC" };
                SelectedEndpointType = EndpointTypes.First();
                InitializeCommands();
            }

            public System.Windows.Input.ICommand AddEndpointItemCommand
            {
                get;
                private set;
            }

            public ObservableCollection<string> EndpointTypes
            {
                get;
                private set;
            }

            private string _addEndpointText;
            public string AddEndpointText
            {
                get { return _addEndpointText; }
                set
                {
                    _addEndpointText = value;
                    OnPropertyChanged(() => AddEndpointText);
                }
            }

            private string _selectedEndpointType;
            public string SelectedEndpointType
            {
                get { return _selectedEndpointType; }
                set
                {
                    _selectedEndpointType = value;
                    OnPropertyChanged(() => SelectedEndpointType);
                }
            }

            private void InitializeCommands()
            {
                AddEndpointItemCommand = new RelayCommand<dynamic>(
                    x =>
                    {
                        var endpoint = String.Format("{0} [{1}]", AddEndpointText, SelectedEndpointType);
                        parent.AddElement(endpoint);
                        AddEndpointText = string.Empty;
                    },
                    x => IsValid);
            }

            protected override IValidator CreateValidator()
            {
                return new CreateEndpointViewModelValidator();
            }
        }

        class EndpointPickerViewModelValidator : AbstractValidator<EndpointPickerViewModel>
        {
            public EndpointPickerViewModelValidator()
            {
                RuleFor(vm => vm.SelectedItems.Count).GreaterThan(0);
            }
        }

        class CreateEndpointViewModelValidator : AbstractValidator<CreateEndpointViewModel>
        {
            public CreateEndpointViewModelValidator()
            {
                RuleFor(vm => vm.AddEndpointText)
                    .NotNull()
                    .Length(ValidationConstants.IdentifierMinLength, ValidationConstants.CompoundIdentifierMaxLength)
                    .Matches(ValidationConstants.CompoundIdentifierPattern).WithMessage(ValidationConstants.InvalidCompoundIdentifierMessage);

                RuleFor(vm => vm.SelectedEndpointType)
                    .NotNull()
                    .NotEmpty();
            }
        }
    }
}

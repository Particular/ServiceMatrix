using FluentValidation;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.Diagramming.ViewModels
{
    public class AddEndpointViewModel : ValidatingViewModel
    {
        public AddEndpointViewModel()
        {
            InitializeCommands();

            EndpointType = "NServiceBusHost";
        }

        public System.Windows.Input.ICommand AcceptCommand
        {
            get;
            private set;
        }

        string endpointName;
        public string EndpointName
        {
            get
            {
                return endpointName;
            }
            set
            {
                if (value != endpointName)
                {
                    endpointName = value;
                    OnPropertyChanged(() => EndpointName);
                }
            }
        }

        string endpointType;
        public string EndpointType
        {
            get
            {
                return endpointType;
            }
            set
            {
                if (value != endpointType)
                {
                    endpointType = value;
                    OnPropertyChanged(() => EndpointType);
                }
            }
        }

        protected override IValidator CreateValidator()
        {
            return new AddEndpointViewModelValidator();
        }

        void CloseDialog(dynamic dialog)
        {
            dialog.DialogResult = true;
            dialog.Close();
        }

        void InitializeCommands()
        {
            AcceptCommand = new RelayCommand<dynamic>(dialog => this.CloseDialog(dialog), dialog => IsValid);
        }

        class AddEndpointViewModelValidator : AbstractValidator<AddEndpointViewModel>
        {
            public AddEndpointViewModelValidator()
            {
                RuleFor(vm => vm.EndpointName)
                    .NotNull().WithMessage("Endpoint name cannot be empty")
                    .Length(ValidationConstants.IdentifierMinLength, ValidationConstants.CompoundIdentifierMaxLength)
                    .Matches(ValidationConstants.CompoundIdentifierPattern).WithMessage(ValidationConstants.InvalidCompoundIdentifierMessage);

                RuleFor(vm => vm.EndpointType)
                    .NotNull()
                    .NotEmpty();
            }
        }
    }
}

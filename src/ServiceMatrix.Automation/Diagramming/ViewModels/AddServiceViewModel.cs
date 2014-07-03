using FluentValidation;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.Diagramming.ViewModels
{
    public class AddServiceViewModel : ValidatingViewModel
    {
        public AddServiceViewModel()
        {
            InitializeCommands();
        }

        public System.Windows.Input.ICommand AcceptCommand
        {
            get;
            private set;
        }

        string serviceName;
        public string ServiceName
        {
            get
            {
                return serviceName;
            }
            set
            {
                if (value != serviceName)
                {
                    serviceName = value;
                    OnPropertyChanged(() => ServiceName);
                }
            }
        }

        protected override IValidator CreateValidator()
        {
            return new AddServiceViewModelValidator();
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

        class AddServiceViewModelValidator : AbstractValidator<AddServiceViewModel>
        {
            public AddServiceViewModelValidator()
            {
                RuleFor(vm => vm.ServiceName)
                    .NotNull()
                    .Length(ValidationConstants.IdentifierMinLength, ValidationConstants.CompoundIdentifierMaxLength)
                    .Matches(ValidationConstants.CompoundIdentifierPattern);
            }
        }
    }
}

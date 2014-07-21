using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using AbstractEndpoint;
using FluentValidation;
using FluentValidation.Validators;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    public class ServiceAndCommandPickerViewModel : ValidatingViewModel
    {
        public const string NewHandler = "[new handler]";
        private IApplication application;

        [Obsolete("Only available to support design-time data", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ServiceAndCommandPickerViewModel()
        {
        }

        public ServiceAndCommandPickerViewModel(IApplication application)
        {
            InitializeCommands();
            this.application = application;

            AllowServiceSelection = true;

            Services = application.Design.Services.Service.Select(s => s.InstanceName).OrderBy(n => n).ToList();
            SelectedService = string.Empty;
            CanProvideCommand = false;
            SelectedCommand = string.Empty;
            CanProvideHandler = false;
            SelectedHandlerComponent = null;
        }

        public ServiceAndCommandPickerViewModel(IService service)
        {
            InitializeCommands();
            application = service.Parent.Parent.Parent;

            AllowServiceSelection = false;

            Services = application.Design.Services.Service.Select(s => s.InstanceName).OrderBy(n => n).ToList();
            CanProvideCommand = false;
            SelectedCommand = string.Empty;
            CanProvideHandler = false;
            SelectedHandlerComponent = null;
            SelectedService = service.InstanceName;
        }

        /// <summary>
        /// Gets the accept command.
        /// </summary>
        public System.Windows.Input.ICommand AcceptCommand
        {
            get;
            private set;
        }

        public bool AllowServiceSelection { get; private set; }

        public ICollection<string> Services
        {
            get;
            private set;
        }

        private string _selectedService;

        public string SelectedService
        {
            get { return _selectedService; }
            set
            {
                if (_selectedService != value)
                {
                    _selectedService = value;
                    OnPropertyChanged(() => SelectedService);

                    if (application != null)
                    {
                        var service = application.Design.Services.Service.FirstOrDefault(s => s.InstanceName == _selectedService);

                        Commands =
                            service != null ? service.Contract.Commands.Command.Select(c => c.InstanceName).OrderBy(n => n).ToList() : new List<string>();
                        SelectedCommand = string.Empty;
                        CanProvideCommand = !string.IsNullOrEmpty(_selectedService);

                        var newHandlerComponents = new List<object> { new { HandlerDescription = NewHandler, Handler = default(IComponent) } };
                        if (service != null)
                        {
                            newHandlerComponents.AddRange(GetComponentDescriptions(service));
                        }
                        HandlerComponents = newHandlerComponents;
                        SelectedHandlerComponent = null;
                        CanProvideHandler = false;
                    }
                }
            }
        }

        ICollection<string> commands;

        public ICollection<string> Commands
        {
            get
            {
                return commands;
            }

            set
            {
                commands = value;
                OnPropertyChanged(() => Commands);
            }
        }

        private bool canProvideCommand;

        public bool CanProvideCommand
        {
            get { return canProvideCommand; }
            set
            {
                if (canProvideCommand != value)
                {
                    canProvideCommand = value;
                    OnPropertyChanged(() => CanProvideCommand);
                }
            }
        }

        private string selectedCommand;

        public string SelectedCommand
        {
            get
            {
                return selectedCommand;
            }
            set
            {
                if (value != selectedCommand)
                {
                    selectedCommand = value;
                    OnPropertyChanged(() => SelectedCommand);

                    if (application != null)
                    {
                        var service = application.Design.Services.Service.FirstOrDefault(s => s.InstanceName == _selectedService);
                        var command = service != null ? service.Contract.Commands.Command.FirstOrDefault(c => c.InstanceName == selectedCommand) : default(ICommand);

                        if (command != null)
                        {
                            SelectedHandlerComponent = service.Components.Component.FirstOrDefault(c => c.Subscribes.ProcessedCommandLinks.Any(pcl => pcl.CommandReference.Value == command));

                            CanProvideHandler = false;
                        }
                        else
                        {
                            if (!CanProvideHandler)
                            {
                                SelectedHandlerComponent = null;
                            }

                            CanProvideHandler = true;
                        }
                    }
                }
            }
        }

        ICollection<object> handlerComponents;

        public ICollection<object> HandlerComponents
        {
            get
            {
                return handlerComponents;
            }

            set
            {
                handlerComponents = value;
                OnPropertyChanged(() => HandlerComponents);
            }
        }

        private bool canProvideHandler;

        public bool CanProvideHandler
        {
            get { return canProvideHandler; }
            set
            {
                if (canProvideHandler != value)
                {
                    canProvideHandler = value;
                    OnPropertyChanged(() => CanProvideHandler);
                }
            }
        }

        private IComponent handlerComponent;

        public IComponent SelectedHandlerComponent
        {
            get
            {
                return handlerComponent;
            }
            set
            {
                if (value != handlerComponent)
                {
                    handlerComponent = value;
                    OnPropertyChanged(() => SelectedHandlerComponent);
                }
            }
        }

        private void CloseDialog(dynamic dialog)
        {
            dialog.DialogResult = true;
            dialog.Close();
        }

        private void InitializeCommands()
        {
            AcceptCommand = new RelayCommand<dynamic>(dialog => this.CloseDialog(dialog), dialog => IsValid);
        }

        protected override IValidator CreateValidator()
        {
            return new ServiceAndCommandPickerViewModelValidator();
        }

        private IEnumerable<object> GetComponentDescriptions(IService service)
        {
            var allDeployedComponents =
                service.Parent.Parent
                    .Endpoints.GetAll()
                    .SelectMany(ep => ep.EndpointComponents.AbstractComponentLinks.Where(l => l.ComponentReference.Value.Parent.Parent == service))
                    .ToDictionary(l => l.ComponentReference.Value, l => l.ParentEndpointComponents.ParentEndpoint);

            return service.Components.Component
                .Where(c => c.IsProcessor)
                .Select(c =>
                {
                    IAbstractEndpoint endpoint;
                    return new
                    {
                        HandlerDescription =
                            c.InstanceName + string.Format(CultureInfo.CurrentCulture, " ({0})", (allDeployedComponents.TryGetValue(c, out endpoint) ? endpoint.InstanceName : "undeployed")),
                        Handler = c
                    };
                })
                .OrderBy(n => n.HandlerDescription);
        }
    }

    class ServiceAndCommandPickerViewModelValidator : AbstractValidator<ServiceAndCommandPickerViewModel>
    {
        public ServiceAndCommandPickerViewModelValidator()
        {
            RuleFor(vm => vm.SelectedService)
                .NotNull()
                .Length(ValidationConstants.IdentifierMinLength, ValidationConstants.CompoundIdentifierMaxLength)
                    .Matches(ValidationConstants.CompoundIdentifierPattern).WithMessage(ValidationConstants.InvalidCompoundIdentifierMessage);

            RuleFor(vm => vm.SelectedCommand)
                .NotNull()
                .Length(ValidationConstants.IdentifierMinLength, ValidationConstants.IdentifierMaxLength)
                .Matches(ValidationConstants.IdentifierPattern).WithMessage(ValidationConstants.InvalidIdentifierMessage)
                .Must(BeDifferentToTheMasterItem)
                .WithMessage("Values must be different");
        }

        private bool BeDifferentToTheMasterItem(ServiceAndCommandPickerViewModel model, string value, PropertyValidatorContext context)
        {
            return !string.Equals(value, model.SelectedService, StringComparison.Ordinal);
        }
    }
}

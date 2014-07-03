using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FluentValidation;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    public class ServicePickerViewModel : ValidatingViewModel
    {
        [Obsolete("Only available to support design-time data", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ServicePickerViewModel()
        {
        }

        public ServicePickerViewModel(ICollection<string> elements)
        {
            InitializeCommands();

            elements = elements ?? new List<string>();
            Elements = new ObservableCollection<SelectItemViewModel>(elements.Select(x => new SelectItemViewModel { Name = x }));
        }

        public System.Windows.Input.ICommand AcceptCommand
        {
            get;
            private set;
        }

        public System.Windows.Input.ICommand AddServiceCommand
        {
            get;
            private set;
        }

        public ObservableCollection<SelectItemViewModel> Elements
        {
            get;
            private set;
        }

        public bool DropdownEditable
        {
            get;
            set;
        }

        public ICollection<string> SelectedItems
        {
            get { return Elements.Where(x => x.IsSelected).Select(x => x.Name).ToList(); }
        }

        private string addServiceText;
        public string AddServiceText
        {
            get { return addServiceText; }
            set
            {
                addServiceText = value;
                OnPropertyChanged(() => AddServiceText);
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
            AddServiceCommand = new RelayCommand<dynamic>(
                x =>
                {
                    Elements.Add(new SelectItemViewModel { Name = AddServiceText, IsSelected = true });
                    AddServiceText = string.Empty;
                },
                x => !string.IsNullOrEmpty(AddServiceText));
        }

        protected override IValidator CreateValidator()
        {
            return new ServicePickerViewModelValidator();
        }

        class ServicePickerViewModelValidator : AbstractValidator<ServicePickerViewModel>
        {
            public ServicePickerViewModelValidator()
            {
                RuleFor(vm => vm.SelectedItems.Count).GreaterThan(0);
            }
        }
    }
}

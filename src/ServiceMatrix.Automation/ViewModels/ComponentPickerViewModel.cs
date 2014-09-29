using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FluentValidation;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    public class ComponentPickerViewModel : ValidatingViewModel
    {
        [Obsolete("Only available to support design-time data", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ComponentPickerViewModel()
        {
        }

        public ComponentPickerViewModel(ICollection<string> elements)
        {
            InitializeCommands();
            DropdownEditable = true;

            elements = elements ?? new List<string>();
            Elements = new ObservableCollection<SelectItemViewModel>(elements.Select(x => new SelectItemViewModel { Name = x }));
        }

        public System.Windows.Input.ICommand AcceptCommand
        {
            get;
            private set;
        }

        public bool DropdownEditable
        {
            get;
            set;
        }

        public string Title
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
        }

        protected override IValidator CreateValidator()
        {
            return new ComponentPickerViewModelValidator();
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

        class ComponentPickerViewModelValidator : AbstractValidator<ComponentPickerViewModel>
        {
            public ComponentPickerViewModelValidator()
            {
                RuleFor(vm => vm.SelectedItems.Count).GreaterThan(0);
            }
        }
    }
}

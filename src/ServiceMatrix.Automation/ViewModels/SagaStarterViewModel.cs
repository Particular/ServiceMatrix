using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FluentValidation;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    public class SagaStarterViewModel : ValidatingViewModel
    {
        [Obsolete("Only available to support design-time data", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SagaStarterViewModel()
        {
        }

        public SagaStarterViewModel(ICollection<string> elements)
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

        public string MasterName
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
                foreach (var element in Elements)
                {
                    if (value.Contains(element.Name))
                    {
                        element.IsSelected = true;
                    }
                    else
                    {
                        element.IsSelected = false;
                    }
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
            return new SagaStarterViewModelValidator();
        }

        class SagaStarterViewModelValidator : AbstractValidator<SagaStarterViewModel>
        {
            public SagaStarterViewModelValidator()
            {
                RuleFor(vm => vm.SelectedItems.Count).GreaterThan(0);
            }
        }
    }
}

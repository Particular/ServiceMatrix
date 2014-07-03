using FluentValidation;
using NuPattern.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NServiceBusStudio.Automation.ViewModels
{
    public class ElementPickerViewModel : ValidatingViewModel
    {
        [Obsolete("Only available to support design-time data", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ElementPickerViewModel()
        {
        }

        public ElementPickerViewModel(ICollection<string> elements)
        {
            InitializeCommands();
            SelectedItem = string.Empty;
            DropDownEditable = true;
            Elements = elements ?? new List<string>();
        }

        public System.Windows.Input.ICommand AcceptCommand { get; private set; }

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;

                var uriSource = default(Uri);

                if (value == "Publish Event")
                {
                    uriSource = new Uri("../Diagramming/Styles/Images/EventIcon.png", UriKind.Relative);
                }
                else if (value == "Send Command")
                {
                    uriSource = new Uri("../Diagramming/Styles/Images/CommandIcon.png", UriKind.Relative);
                }

                if (uriSource != null)
                {
                    TitleImageSource = new BitmapImage(uriSource);
                }
            }
        }

        public string MasterName
        {
            get;
            set;
        }

        public bool DropDownEditable
        {
            get;
            set;
        }

        public ImageSource TitleImageSource
        {
            get;
            set;
        }

        public ICollection<string> Elements
        {
            get;
            private set;
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        protected override IValidator CreateValidator()
        {
            return new ElementPickerViewModelValidator();
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

        class ElementPickerViewModelValidator : AbstractValidator<ElementPickerViewModel>
        {
            public ElementPickerViewModelValidator()
            {
                RuleFor(vm => vm.SelectedItem)
                    .NotNull()
                    .Length(ValidationConstants.IdentifierMinLength, ValidationConstants.IdentifierMaxLength)
                    .Matches(ValidationConstants.IdentifierPattern);
            }
        }
    }
}

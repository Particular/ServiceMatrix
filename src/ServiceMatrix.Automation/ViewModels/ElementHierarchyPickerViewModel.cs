using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FluentValidation;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    public class ElementHierarchyPickerViewModel : ValidatingViewModel
    {
        IValidator validator;

        [Obsolete("Only available to support design-time data", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ElementHierarchyPickerViewModel()
        {
        }

        public ElementHierarchyPickerViewModel(IEnumerable<Tuple<string, ICollection<string>>> elements)
        {
            InitializeCommands();
            DropDownEditable = true;
            var elementsList = elements as IList<Tuple<string, ICollection<string>>> ?? elements.ToList();
            Elements = elementsList.ToDictionary(t => t.Item1, t => t.Item2);
            MasterElements = elementsList.Select(t => t.Item1).ToList();
            validator = new ElementHierarchyPickerViewModelValidator();
        }

        /// <summary>
        /// Gets the accept command.
        /// </summary>
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

        public ImageSource TitleImageSource { get; set; }

        public string MasterName
        {
            get;
            set;
        }

        public string SlaveName
        {
            get;
            set;
        }

        IDictionary<string, ICollection<string>> Elements
        {
            get;
            set;
        }

        public bool DropDownEditable { get; set; }

        public ICollection<string> MasterElements { get; private set; }

        private string _selectedMasterItem;

        public string SelectedMasterItem
        {
            get { return _selectedMasterItem; }
            set
            {
                _selectedMasterItem = value;
                OnPropertyChanged(() => SelectedMasterItem);
                OnPropertyChanged(() => SlaveElements);
                OnPropertyChanged(() => SelectedSlaveItem);
            }
        }

        public ICollection<string> SlaveElements
        {
            get
            {
                ICollection<string> slaveElements;
                if (!Elements.TryGetValue(SelectedMasterItem, out slaveElements) || slaveElements == null)
                {
                    return new List<string>();
                }

                return slaveElements;
            }
        }

        private string selectedSlaveItem;

        public string SelectedSlaveItem
        {
            get
            {
                return selectedSlaveItem;
            }
            set
            {
                if (value != selectedSlaveItem)
                {
                    selectedSlaveItem = value;
                    OnPropertyChanged(() => SelectedSlaveItem);
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

        protected override IValidator GetValidator()
        {
            return validator;
        }
    }
}

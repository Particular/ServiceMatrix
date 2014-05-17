using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    public class ElementHierarchyPickerViewModel : ValidatingViewModel
    {
        public ElementHierarchyPickerViewModel()
        {
            InitializeCommands();
            DropDownEditable = true;
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

                TitleImageSource = new BitmapImage(uriSource);
            }
        }

        public ImageSource TitleImageSource { get; set; }

        public string SlaveName
        {
            get;
            set;
        }

        public bool DropDownEditable { get; set; }

        public IDictionary<string, ICollection<string>> Elements
        {
            get;
            set;
        }

        public ICollection<string> MasterElements
        {
            get { return this.Elements.Keys; }
        }

        private string _selectedMasterItem;

        [Required]
        [RegularExpression(@"[_\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}][\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*")]
        [StringLength(30)]
        public string SelectedMasterItem
        {
            get { return _selectedMasterItem; }
            set
            {
                _selectedMasterItem = value;
                OnPropertyChanged(() => SelectedMasterItem);
                OnPropertyChanged(() => SlaveElements);
            }
        }

        public ICollection<string> SlaveElements
        {
            get
            {
                var master = Elements.FirstOrDefault(x => x.Key == SelectedMasterItem);
                if (master.Value == null)
                {
                    return new List<string>();
                }
                return master.Value;
            }
        }

        private string selectedSlaveItem;

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"[_\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}][\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*")]
        [StringLength(30)]
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
    }
}

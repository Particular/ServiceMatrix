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
            this.InitializeCommands();
            this.DropDownEditable = true;
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
                return this.title;
            }
            set
            {
                this.title = value;

                var uriSource = default(Uri);

                if (value == "Publish Event")
                {
                    uriSource = new Uri("../Diagramming/Styles/Images/EventIcon.png", UriKind.Relative);
                }
                else if (value == "Send Command")
                {
                    uriSource = new Uri("../Diagramming/Styles/Images/CommandIcon.png", UriKind.Relative);
                }

                this.TitleImageSource = new BitmapImage(uriSource);
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
                this.OnPropertyChanged(() => this.SelectedMasterItem);
                this.OnPropertyChanged(() => this.SlaveElements);
            }
        }

        public ICollection<string> SlaveElements
        {
            get
            {
                var master = this.Elements.FirstOrDefault(x => x.Key == this.SelectedMasterItem);
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
                return this.selectedSlaveItem;
            }
            set
            {
                if (value != this.selectedSlaveItem)
                {
                    this.selectedSlaveItem = value;
                    this.OnPropertyChanged(() => this.SelectedSlaveItem);
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
            this.AcceptCommand = new RelayCommand<dynamic>(dialog => this.CloseDialog(dialog), dialog => this.IsValid);
        }
    }
}

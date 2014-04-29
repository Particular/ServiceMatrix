using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using NuPattern.Runtime;
using System.ComponentModel;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.Dialog
{
    /// <summary>
    /// Interaction logic for ElementPicker.xaml
    /// </summary>
    public partial class ElementHierarchyPicker : CommonDialogWindow, IDialogWindow, IElementHierarchyPicker, IComponentConnector, INotifyPropertyChanged
    {
        public ElementHierarchyPicker()
        {
            InitializeComponent();
            SelectedMasterItem = "";
            SelectedSlaveItem = "";
        }

        public new string Title 
        {
            get 
            {
                return lblTitle.Content.ToString();
            }
            set
            {
                lblTitle.Content = value;

                var uriSource = default(Uri);

                if (value == "Publish Event")
                {
                    uriSource = new Uri("../Diagramming/Styles/Images/EventIcon.png", UriKind.Relative);
                }
                else if (value == "Send Command")
                {
                    uriSource = new Uri("../Diagramming/Styles/Images/CommandIcon.png", UriKind.Relative);
                }

                imgTitle.Source = new BitmapImage(uriSource);
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            DataContext = this;
            base.OnActivated(e);

            var textBox = (MasterDropDown.Template.FindName("PART_EditableTextBox", MasterDropDown) as TextBox);
            if (textBox != null)
            {
                textBox.Focus();
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        public string SlaveName
        {
            get
            {
                return SlaveLabel.Content.ToString();
            }
            set
            {
                SlaveLabel.Content = value;
            }
        }

        public IDictionary<string, ICollection<string>> Elements
        {
            get;
            set;
        }

        public ICollection<string> MasterElements
        {
            get { return Elements.Keys; }
        }

        private string _selectedMasterItem;
        public string SelectedMasterItem
        {
            get { return _selectedMasterItem; }
            set
            {
                _selectedMasterItem = value;
                OnPropertyChange("SlaveElements");
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

        public string SelectedSlaveItem
        {
            get;
            set;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public void SetDropdownEditable(bool isEditable)
        {
            MasterDropDown.IsEditable = isEditable;
            SlaveDropDown.IsEditable = isEditable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged (this, new PropertyChangedEventArgs (property));
            }
        }
    }
}

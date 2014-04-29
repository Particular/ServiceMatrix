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
    public partial class ElementPicker : CommonDialogWindow, IDialogWindow, IElementPicker, IComponentConnector
    {
        public ElementPicker()
        {
            InitializeComponent();
            SelectedItem = "";
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

            var textBox = (DropDown.Template.FindName("PART_EditableTextBox", DropDown) as TextBox);
            if (textBox != null)
            {
                textBox.Focus();
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        public string MasterName
        {
            get
            {
                return MasterLabel.Content.ToString();
            }
            set
            {
                MasterLabel.Content = value;
            }
        }

        public ICollection<string> Elements
        {
            get;
            set;
        }

        public string SelectedItem
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
            DropDown.IsEditable = isEditable;
        }
    }
}

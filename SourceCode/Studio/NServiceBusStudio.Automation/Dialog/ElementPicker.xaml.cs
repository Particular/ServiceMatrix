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
using NuPattern.Common.Presentation;
using System.ComponentModel;

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
        }

        protected override void OnActivated(EventArgs e)
        {
            this.DataContext = this;
            base.OnActivated(e);

            var textBox = (this.DropDown.Template.FindName("PART_EditableTextBox", DropDown) as TextBox);
            if (textBox != null)
            {
                textBox.Focus();
                textBox.SelectionStart = textBox.Text.Length;
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
            this.DialogResult = true;
            this.Close();
        }

        public void SetDropdownEditable(bool isEditable)
        {
            this.DropDown.IsEditable = isEditable;
        }
    }
}

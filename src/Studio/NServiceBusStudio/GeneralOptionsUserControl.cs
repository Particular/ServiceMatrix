using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NServiceBusStudio
{
    public partial class GeneralOptionsUserControl : UserControl
    {
        public GeneralOptionsPage GeneralOptionsPage { get; set; }

        public GeneralOptionsUserControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            txtInternalMessages.Text = this.GeneralOptionsPage.ProjectNameInternalMessages;
            txtContracts.Text = this.GeneralOptionsPage.ProjectNameContracts;
            txtCode.Text = this.GeneralOptionsPage.ProjectNameCode;
        }

        private void txtProjectName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') ||
                (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                (e.KeyChar == '.') ||
                (e.KeyChar == '\b'))
            {
                // Accept Only Characters or dots (.)
                e.Handled = false;
            }
            else
            {
                // Discard other keys
                e.Handled = true;
            }
        }

        private void txtInternalMessages_KeyUp(object sender, KeyEventArgs e)
        {
            this.GeneralOptionsPage.ProjectNameInternalMessages = txtInternalMessages.Text;
        }

        private void txtContracts_KeyUp(object sender, KeyEventArgs e)
        {
            this.GeneralOptionsPage.ProjectNameContracts = txtContracts.Text;
        }

        private void txtCode_KeyUp(object sender, KeyEventArgs e)
        {
            this.GeneralOptionsPage.ProjectNameCode = txtCode.Text;
        }
        
    }
}

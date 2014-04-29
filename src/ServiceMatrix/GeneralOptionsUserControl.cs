namespace NServiceBusStudio
{
    using System.Windows.Forms;
    public partial class GeneralOptionsUserControl : UserControl
    {
        public GeneralOptionsPage GeneralOptionsPage { get; set; }

        public GeneralOptionsUserControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            txtInternalMessages.Text = GeneralOptionsPage.ProjectNameInternalMessages;
            txtContracts.Text = GeneralOptionsPage.ProjectNameContracts;
            txtCode.Text = GeneralOptionsPage.ProjectNameCode;
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
            GeneralOptionsPage.ProjectNameInternalMessages = txtInternalMessages.Text;
        }

        private void txtContracts_KeyUp(object sender, KeyEventArgs e)
        {
            GeneralOptionsPage.ProjectNameContracts = txtContracts.Text;
        }

        private void txtCode_KeyUp(object sender, KeyEventArgs e)
        {
            GeneralOptionsPage.ProjectNameCode = txtCode.Text;
        }
        
    }
}

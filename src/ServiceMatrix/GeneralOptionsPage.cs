using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NServiceBusStudio
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class GeneralOptionsPage : DialogPage
    {
        private string projectNameInternalMessages = "Internal";

        public string ProjectNameInternalMessages 
        {
            get { return projectNameInternalMessages; }
            set { projectNameInternalMessages = value; }
        }

        private string projectNameContracts = "Contracts";

        public string ProjectNameContracts 
        {
            get { return projectNameContracts; }
            set { projectNameContracts = value; }
        }

        private string projectNameCode = "Code";

        public string ProjectNameCode
        {
            get { return projectNameCode; }
            set { projectNameCode = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override IWin32Window Window
        {
            get
            {
                GeneralOptionsUserControl page = new GeneralOptionsUserControl();
                page.GeneralOptionsPage = this;
                page.Initialize();
                return page;
            }
        }
    }
}

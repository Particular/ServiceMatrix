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
using NuPattern;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio.Automation.Dialog
{
    /// <summary>
    /// Interaction logic for ElementPicker.xaml
    /// </summary>
    public partial class UserCodeChangeRequired : CommonDialogWindow, IDialogWindow
    {
        public UserCodeChangeRequired()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            this.DataContext = this;
            base.OnActivated(e);
        }

        public string CodeFile { get; set; }

        public string Code { get; set; }

        public IUriReferenceService UriService { get; set; }

        public ISolution Solution { get; set; }

        public IComponent Component { get; set; }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            // Copy code into Clipboard
            Clipboard.SetText(this.Code);

            // Open Component Handler
            var filepath = String.Format("{0}.{1}\\{2}\\{3}.cs",
                this.Component.Parent.Parent.Parent.Parent.Parent.InstanceName,
                this.Component.Parent.Parent.Parent.Parent.Parent.ProjectNameCode,
                this.Component.Parent.Parent.CodeIdentifier,
                this.Component.CodeIdentifier);

            var item = this.Solution.Find(filepath).FirstOrDefault();
            if (item != null)
            {
                this.UriService.Open(item);
            }
            

            this.DialogResult = true;
            this.Close();
        }
    }
}

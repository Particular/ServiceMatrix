namespace NServiceBusStudio.Automation.Dialog
{
    using System;
    using System.Linq;
    using System.Windows;
    using NuPattern.Presentation;
    using NuPattern;
    using NuPattern.VisualStudio.Solution;

    /// <summary>
    /// Interaction logic for ElementPicker.xaml
    /// </summary>
    public partial class UserCodeChangeRequired : IDialogWindow
    {
        public UserCodeChangeRequired()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            DataContext = this;
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
            Clipboard.SetText(Code);

            var endpoint = Component.Parent.Parent.Parent.Parent.Endpoints.GetAll()
                   .FirstOrDefault(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == Component));

            if (endpoint != null)
            {
                // Open Component Handler
                var filepath = String.Format("{0}.{1}\\{2}\\{3}.cs",
                    Component.Parent.Parent.Parent.Parent.Parent.CodeIdentifier,
                    endpoint.InstanceName,
                    Component.Parent.Parent.CodeIdentifier,
                    Component.CodeIdentifier);

                var item = Solution.Find(filepath).FirstOrDefault();
                if (item != null)
                {
                    UriService.Open(item);
                }
            }
            

            DialogResult = true;
            Close();
        }
    }
}

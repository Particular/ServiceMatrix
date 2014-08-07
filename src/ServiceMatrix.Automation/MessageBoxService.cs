using System;
using System.ComponentModel.Composition;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuPattern;
using NuPattern.VisualStudio;

namespace NServiceBusStudio.Automation
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IMessageBoxService))]
    internal class MessageBoxService : IMessageBoxService
    {
        private IVsUIShell uiShell;

        [ImportingConstructor]
        public MessageBoxService([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            uiShell = serviceProvider.GetService<SVsUIShell, IVsUIShell>();
        }

        public MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            return MessageBox.Show(uiShell.GetMainWindow(), messageBoxText, caption, button);
        }

        public MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return MessageBox.Show(uiShell.GetMainWindow(), messageBoxText, caption, button, icon);
        }
    }
}
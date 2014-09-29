using System;
using System.ComponentModel.Composition;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuPattern;
using NuPattern.Presentation;
using NuPattern.VisualStudio;

namespace NServiceBusStudio
{
    /// <summary>
    /// Implements dialog creation in Visual Studio.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IDialogWindowFactory))]
    public class DialogWindowFactory : IDialogWindowFactory
    {
        private IVsUIShell uiShell;

        // Tests can use this to override/wrap the dialogs or automate them as needed.
        internal Func<IDialogWindow, IDialogWindow> CreateDialogCallBack { get; set; }

        [ImportingConstructor]
        public DialogWindowFactory([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            uiShell = serviceProvider.GetService<SVsUIShell, IVsUIShell>();
            CreateDialogCallBack = dialog => dialog;
        }

        public IDialogWindow CreateDialog<TView>() where TView : IDialogWindow, new()
        {
            return ThreadHelper.Generic.Invoke(() =>
            {
                var dialog = new TView();
                var dialogWindow = dialog as Window;
                if (dialogWindow != null)
                {
                    dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    dialogWindow.ShowInTaskbar = false;
                    dialogWindow.Owner = uiShell.GetMainWindow();
                }

                return CreateDialogCallBack(dialog);
            });
        }

        public IDialogWindow CreateDialog<TView>(object context) where TView : IDialogWindow, new()
        {
            return ThreadHelper.Generic.Invoke(() =>
            {
                var dialog = new TView { DataContext = context };
                var dialogWindow = dialog as Window;
                if (dialogWindow != null)
                {
                    dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    dialogWindow.ShowInTaskbar = false;
                    dialogWindow.Owner = uiShell.GetMainWindow();
                }

                return CreateDialogCallBack(dialog);
            });
        }
    }
}

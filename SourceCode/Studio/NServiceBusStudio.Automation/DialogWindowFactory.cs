using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Windows;
using Microsoft.VisualStudio;
using System.Windows.Interop;

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
			this.uiShell = serviceProvider.GetService<SVsUIShell, IVsUIShell>();
			this.CreateDialogCallBack = dialog => dialog;
		}

		public IDialogWindow CreateDialog<TView>() where TView : IDialogWindow, new()
		{
			return ThreadHelper.Generic.Invoke(() =>
			{
				var dialog = new TView();
				var dialogWindow = dialog as Window;
				if (dialogWindow != null)
				{
					IntPtr owner;
					ErrorHandler.ThrowOnFailure(this.uiShell.GetDialogOwnerHwnd(out owner));
					new WindowInteropHelper(dialogWindow).Owner = owner;
					dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
					dialogWindow.ShowInTaskbar = false;
					// This would not set the right owner.
					//dialogWindow.Owner = Application.Current.MainWindow;
				}

				return this.CreateDialogCallBack(dialog);
			});
		}
	}
}

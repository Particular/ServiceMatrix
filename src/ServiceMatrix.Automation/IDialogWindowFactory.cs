namespace NServiceBusStudio
{
    using NuPattern.Presentation;

	/// <summary>
	/// Main factory for dialogs inside automation code.
	/// </summary>
	public interface IDialogWindowFactory
	{
		/// <summary>
		/// Creates a <see cref="Window"/> dialog as child of the main Visual Studio window.
		/// </summary>
		/// <typeparam name="TView">The type of the window to create.</typeparam>
		/// <returns>
		/// The created <see cref="Window"/> dialog.
		/// </returns>
		IDialogWindow CreateDialog<T>() where T : IDialogWindow, new();
	}
}

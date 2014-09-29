using NuPattern.Presentation;

namespace NServiceBusStudio
{
    /// <summary>
    /// Main factory for dialogs inside automation code.
    /// </summary>
    public interface IDialogWindowFactory
    {
        /// <summary>
        /// Creates a <see cref="IDialogWindow"/> dialog as child of the main Visual Studio window.
        /// </summary>
        /// <typeparam name="TView">The type of the window to create.</typeparam>
        /// <returns>
        /// The created <see cref="IDialogWindow"/> dialog.
        /// </returns>
        IDialogWindow CreateDialog<TView>() where TView : IDialogWindow, new();

        /// <summary>
        /// Creates a <see cref="IDialogWindow"/> dialog as child of the main Visual Studio window.
        /// </summary>
        /// <typeparam name="TView">The type of the window to create.</typeparam>
        /// <param name="context">The data context.</param>
        /// <returns>
        /// The created <see cref="IDialogWindow"/> dialog.
        /// </returns>
        IDialogWindow CreateDialog<TView>(object context) where TView : IDialogWindow, new();
    }
}

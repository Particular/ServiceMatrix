using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Patterning.Runtime.Shell.Properties;
using Microsoft.VisualStudio.Patterning.Runtime.Shell;

namespace NServiceBusStudio
{
    /// <summary>
    /// Represents a service to show the tool windows hosted in this package.
    /// </summary>
    internal class PackageToolWindow : IPackageToolWindow
    {
        private Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageToolWindow"/> class.
        /// </summary>
        /// <param name="package">The package.</param>
        public PackageToolWindow(Package package)
        {
            Guard.NotNull(() => package, package);

            this.package = package;
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <typeparam name="T">The type of the tool window to show.</typeparam>
        /// <returns>
        /// The <see cref="ToolWindowPane"/> to show.
        /// </returns>
        public T ShowWindow<T>() where T : ToolWindowPane
        {
            var window = FindWindow<T>();
            ErrorHandler.ThrowOnFailure(((IVsWindowFrame)window.Frame).Show());
            return window;
        }

        /// <summary>
        /// Hides the window.
        /// </summary>
        /// <typeparam name="T">The type of the tool window to hide.</typeparam>
        public void HideWindow<T>() where T : ToolWindowPane
        {
            var window = FindWindow<T>();

            var frame = (IVsWindowFrame)window.Frame;

            if (frame.IsVisible() == 0)
            {
                ErrorHandler.ThrowOnFailure(frame.Hide());
            }
        }

        /// <summary>
        /// Determines whether [is window visible].
        /// </summary>
        /// <typeparam name="T">The type of the tool window.</typeparam>
        /// <returns>
        /// 	<c>true</c> if [is window visible]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsWindowVisible<T>() where T : ToolWindowPane
        {
            var window = FindWindow<T>();

            var frame = (IVsWindowFrame)window.Frame;

            return frame.IsVisible() == 0;
        }

        private T FindWindow<T>() where T : ToolWindowPane
        {
            var window = this.package.FindToolWindow(typeof(T), 0, true);

            if (window == null || window.Frame == null)
            {
                throw new NotSupportedException("Cannot create window");
            }

            return (T)window;
        }
    }
}
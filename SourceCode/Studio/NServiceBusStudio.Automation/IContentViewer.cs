using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace NServiceBusStudio.Automation
{
    /// <summary>
    /// Shows arbitrary WPF content in a document window that 
    /// can be referenced by an identifier.
    /// </summary>
    public interface IContentViewer
    {
        /// <summary>
        /// Shows the given content element in a document window.
        /// </summary>
        /// <param name="id">The identifier for the created window.</param>
        /// <param name="title">The title for the content window to create or show.</param>
        /// <param name="content">The content to display inside the window.</param>
        void ShowContent(Guid id, string title, FrameworkElement content);
    }
}

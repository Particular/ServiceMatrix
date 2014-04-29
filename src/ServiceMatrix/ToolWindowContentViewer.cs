namespace NServiceBusStudio
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.Shell;
    using System.Globalization;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio;
    using NServiceBusStudio.Automation;
    using NuPattern.VisualStudio.Solution;
    using System.Windows;

    internal class ToolWindowContentViewer : IContentViewer
    {
        private Func<Type, int, bool, ToolWindowPane> findToolWindow;
        private Dictionary<Guid, int> idMap = new Dictionary<Guid, int>();

        public ToolWindowContentViewer(ISolutionEvents solutionEvents, Func<Type, int, bool, ToolWindowPane> findToolWindow)
        {
            solutionEvents.SolutionClosing += (sender, args) => ClearWindows();
            solutionEvents.SolutionClosed += (sender, args) => ClearWindows();
            solutionEvents.SolutionOpened += (sender, args) => ClearWindows();
            this.findToolWindow = findToolWindow;
        }

        public void ShowContent(Guid guid, string title, FrameworkElement content)
        {
            int id;
            if (!idMap.TryGetValue(guid, out id))
                idMap.Add(guid, (id = idMap.Count));

            // We get the hashcode of the guid. Should be pretty safe as an integer identifier.
            var window = (ToolWindowContent)findToolWindow(typeof(ToolWindowContent), id, true);
            if (window == null || window.Frame == null)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Failed to show window for '{0}'.",
                    title));
            }

            window.Caption = title;
            window.SetContent(content);
            var frame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(frame.Show());
        }

        private void ClearWindows()
        {
            foreach (var id in idMap.Values)
            {
                var window = (ToolWindowContent)findToolWindow(typeof(ToolWindowContent), id, false);
                if (window != null && window.Frame != null)
                {
                    var frame = (IVsWindowFrame)window.Frame;
                    ErrorHandler.ThrowOnFailure(frame.Hide());
                }
            }

            idMap.Clear();
        }
    }
}

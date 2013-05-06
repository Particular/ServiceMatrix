using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using System.Globalization;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using NServiceBusStudio.Automation;
using NuPattern.Runtime;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio
{
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

        public void ShowContent(Guid guid, string title, System.Windows.FrameworkElement content)
        {
            var id = default(int);
            if (!this.idMap.TryGetValue(guid, out id))
                this.idMap.Add(guid, (id = this.idMap.Count));

            // We get the hashcode of the guid. Should be pretty safe as an integer identifier.
            var window = (ToolWindowContent)this.findToolWindow(typeof(ToolWindowContent), id, true);
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
            foreach (var id in this.idMap.Values)
            {
                var window = (ToolWindowContent)this.findToolWindow(typeof(ToolWindowContent), id, false);
                if (window != null && window.Frame != null)
                {
                    var frame = (IVsWindowFrame)window.Frame;
                    ErrorHandler.ThrowOnFailure(frame.Hide());
                }
            }

            this.idMap.Clear();
        }
    }
}

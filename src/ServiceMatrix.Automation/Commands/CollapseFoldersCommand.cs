namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Shell;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using NuPattern;
    using Command = NuPattern.Runtime.Command;

    [Category("General")]
    [DisplayName("Collapse Solution Explorer Folders")]
    [Description("Collapse solution explorer folders matching names.")]
    [CLSCompliant(false)]
    public class CollapseFoldersCommand : Command
    {
        [Import(typeof(SVsServiceProvider))]
        private IServiceProvider ServiceProvider { get; set; }

        public override void Execute()
        {
            var solutionExplorerWindow = GetSolutionExplorerToolWindow();
            var hierarchy = ServiceProvider.GetService<SVsSolution>() as IVsUIHierarchy;

            CollapseHierarchyItems(solutionExplorerWindow, hierarchy, 0xfffffffe, false, new[] { "GeneratedCode", "References" });
            CollapseHierarchyItems(solutionExplorerWindow, hierarchy, 0xfffffffe, false, new[] { "GeneratedCode", "References" });
            CollapseHierarchyItems(solutionExplorerWindow, hierarchy, 0xfffffffe, false, new[] { "Infrastructure", "GeneratedCode", "References" });
            CollapseHierarchyItems(solutionExplorerWindow, hierarchy, 0xfffffffe, false, new[] { "Infrastructure", "GeneratedCode", "References" });
        }

        private void CollapseHierarchyItems(IVsUIHierarchyWindow toolWindow, IVsHierarchy hierarchy, uint itemid, bool hierIsSolution
            , string[] captions)
        {
            IntPtr ptr;
            uint num2;
            var gUID = typeof(IVsHierarchy).GUID;
            if ((hierarchy.GetNestedHierarchy(itemid, ref gUID, out ptr, out num2) == 0) && (IntPtr.Zero != ptr))
            {
                var objectForIUnknown = Marshal.GetObjectForIUnknown(ptr) as IVsHierarchy;
                Marshal.Release(ptr);
                if (objectForIUnknown != null)
                {
                    CollapseHierarchyItems(toolWindow, objectForIUnknown, num2, false, captions);
                }
            }
            else
            {
                object obj2;
                if (!hierIsSolution)
                {
                    object captionobj;
                    hierarchy.GetProperty(itemid, (int)__VSHPROPID.VSHPROPID_Caption, out captionobj);
                    var caption = (string)captionobj;
                    if (captions.Contains(caption))
                    {
                        string canonicalname;
                        hierarchy.GetCanonicalName(itemid, out canonicalname);
                        ErrorHandler.ThrowOnFailure(toolWindow.ExpandItem(hierarchy as IVsUIHierarchy, itemid, EXPANDFLAGS.EXPF_CollapseFolder));
                    }
                }
                if (hierarchy.GetProperty(itemid, (int)__VSHPROPID.VSHPROPID_FirstVisibleChild, out obj2) == 0)
                {
                    var itemId = GetItemId(obj2);
                    while (itemId != uint.MaxValue)
                    {
                        CollapseHierarchyItems(toolWindow, hierarchy, itemId, false, captions);
                        var hr = hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_NextVisibleSibling, out obj2);
                        if (hr == 0)
                        {
                            itemId = GetItemId(obj2);
                        }
                        else
                        {
                            ErrorHandler.ThrowOnFailure(hr);
                            return;
                        }
                    }
                }
            }
        }

        private uint GetItemId(object pvar)
        {
            if (pvar != null)
            {
                if (pvar is int)
                {
                    return (uint)((int)pvar);
                }
                if (pvar is uint)
                {
                    return (uint)pvar;
                }
                if (pvar is short)
                {
                    return (uint)((short)pvar);
                }
                if (pvar is ushort)
                {
                    return (ushort)pvar;
                }
                if (pvar is long)
                {
                    return (uint)((long)pvar);
                }
            }
            return uint.MaxValue;
        }


        private IVsUIHierarchyWindow GetSolutionExplorerToolWindow()
        {
            IVsUIHierarchyWindow window = null;
            var service = ServiceProvider.GetService<SVsUIShell>() as IVsUIShell;
            if (service != null)
            {
                uint grfFTW = 0;
                var rguidPersistenceSlot = new Guid("{3AE79031-E1BC-11D0-8F78-00A0C9110057}");
                IVsWindowFrame ppWindowFrame;
                service.FindToolWindow(grfFTW, ref rguidPersistenceSlot, out ppWindowFrame);
                if (ppWindowFrame != null)
                {
                    object pvar;
                    ppWindowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out pvar);
                    if (pvar != null)
                    {
                        window = pvar as IVsUIHierarchyWindow;
                    }
                }
            }

            return window;
        }
    }
}

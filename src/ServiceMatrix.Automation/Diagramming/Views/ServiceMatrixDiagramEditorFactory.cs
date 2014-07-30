using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace ServiceMatrix.Diagramming.Views
{
    /// <summary>
    /// Straightforward <see cref="IVsEditorFactory"/> implementation for a file-less editor. 
    /// </summary>
    /// <remarks>
    /// Simple vehicle to create as <see cref="ServiceMatrixDiagramEditorPane"/> when the editor is requested.
    /// </remarks>
    [Guid(GuidList.ServiceMatrixDiagramEditorFactoryGuidString)]
    public sealed class ServiceMatrixDiagramEditorFactory : IVsEditorFactory, IDisposable
    {
        ServiceProvider vsServiceProvider;

        void IDisposable.Dispose()
        {
            if (vsServiceProvider != null)
            {
                vsServiceProvider.Dispose();
            }
        }

        int IVsEditorFactory.Close()
        {
            return VSConstants.S_OK;
        }

        int IVsEditorFactory.CreateEditorInstance(uint grfCreateDoc, string pszMkDocument, string pszPhysicalView, IVsHierarchy pvHier, uint itemid, IntPtr punkDocDataExisting, out IntPtr ppunkDocView, out IntPtr ppunkDocData, out string pbstrEditorCaption, out Guid pguidCmdUI, out int pgrfCDW)
        {
            ppunkDocView = IntPtr.Zero;
            ppunkDocData = IntPtr.Zero;
            pguidCmdUI = GuidList.ServiceMatrixDiagramEditorFactoryGuid;
            pgrfCDW = 0;
            pbstrEditorCaption = null;

            if ((grfCreateDoc & (VSConstants.CEF_OPENFILE | VSConstants.CEF_SILENT)) == 0)
            {
                return VSConstants.E_INVALIDARG;
            }
            if (punkDocDataExisting != IntPtr.Zero)
            {
                return VSConstants.VS_E_INCOMPATIBLEDOCDATA;
            }

            // Create the Document (editor)
            var newEditor = new ServiceMatrixDiagramEditorPane();
            ppunkDocView = Marshal.GetIUnknownForObject(newEditor);
            ppunkDocData = Marshal.GetIUnknownForObject(newEditor);
            pbstrEditorCaption = "";    // caption for the entire frame will be overriden

            return VSConstants.S_OK;
        }

        int IVsEditorFactory.MapLogicalView(ref Guid rguidLogicalView, out string pbstrPhysicalView)
        {
            pbstrPhysicalView = null;

            if (VSConstants.LOGVIEWID_Primary == rguidLogicalView)
            {
                return VSConstants.S_OK;
            }

            return VSConstants.E_NOTIMPL;
        }

        int IVsEditorFactory.SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            vsServiceProvider = new ServiceProvider(psp);
            return VSConstants.S_OK;
        }
    }

    public static class GuidList
    {
        public const string ServiceMatrixDiagramEditorFactoryGuidString = "8556CD79-0349-483E-A6A3-D8B3E38910A5";
        public static readonly Guid ServiceMatrixDiagramEditorFactoryGuid = new Guid(ServiceMatrixDiagramEditorFactoryGuidString);
    }
}

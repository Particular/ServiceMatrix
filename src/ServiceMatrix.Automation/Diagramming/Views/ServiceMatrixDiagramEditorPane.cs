using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NServiceBusStudio;
using NuPattern;
using ServiceMatrix.Diagramming.ViewModels;

namespace ServiceMatrix.Diagramming.Views
{
    /// <summary>
    /// Minimal editor pane which hosts the ServiceMatrix canvas diagram. 
    /// </summary>
    /// <remarks>
    /// Works as both doc view and doc data. The <see cref="IVsPersistDocData"/> implementation is minimal, 
    /// with most methods not implemented, while the <see cref="IVsWindowPane"/> implementation is inherited from
    /// <see cref="WindowPane"/>, using the <see cref="Diagram"/> as its entire content.
    /// </remarks>
    [ComVisible(true)]
    public class ServiceMatrixDiagramEditorPane : WindowPane, IVsPersistDocData
    {
        [Import]
        public ServiceMatrixDiagramAdapter NServiceBusDiagramAdapter { get; set; }

        [Import]
        public IDialogWindowFactory WindowFactory { get; set; }

        public ServiceMatrixDiagramEditorPane() :
            base(null)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            var componentModel = this.GetService<SComponentModel, IComponentModel>();
            componentModel.DefaultCompositionService.SatisfyImportsOnce(this);

            var pane = new Diagram(NServiceBusDiagramAdapter, WindowFactory);
            Content = pane;
        }

        int IVsPersistDocData.Close()
        {
            NServiceBusDiagramAdapter.Close();
            return VSConstants.S_OK;
        }

        int IVsPersistDocData.GetGuidEditorType(out Guid pClassID)
        {
            pClassID = GuidList.ServiceMatrixDiagramEditorFactoryGuid;
            return VSConstants.S_OK;
        }

        int IVsPersistDocData.IsDocDataDirty(out int pfDirty)
        {
            pfDirty = 0;
            return VSConstants.S_OK;
        }

        int IVsPersistDocData.IsDocDataReloadable(out int pfReloadable)
        {
            pfReloadable = 0;
            return VSConstants.S_OK;
        }

        int IVsPersistDocData.LoadDocData(string pszMkDocument)
        {
            NServiceBusDiagramAdapter.Load();
            return VSConstants.S_OK;
        }

        int IVsPersistDocData.OnRegisterDocData(uint docCookie, IVsHierarchy pHierNew, uint itemidNew)
        {
            return VSConstants.S_OK;
        }

        int IVsPersistDocData.ReloadDocData(uint grfFlags)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsPersistDocData.RenameDocData(uint grfAttribs, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsPersistDocData.SaveDocData(VSSAVEFLAGS dwSave, out string pbstrMkDocumentNew, out int pfSaveCanceled)
        {
            pbstrMkDocumentNew = "";
            pfSaveCanceled = 0;
            return VSConstants.E_NOTIMPL;
        }

        int IVsPersistDocData.SetUntitledDocPath(string pszDocDataPath)
        {
            return VSConstants.E_NOTIMPL;
        }
    }
}

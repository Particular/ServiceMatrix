using System;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace NServiceBus.Modeling.EndpointDesign
{
	public class HierarchyNode : IDisposable
	{
		private bool disposed;

		public HierarchyNode(IVsHierarchy hierarchy)
			: this(hierarchy, ResolveItemId(hierarchy))
		{
		}

		public HierarchyNode(IVsHierarchy hierarchy, uint itemId)
		{
			Guard.NotNull(() => hierarchy, hierarchy);

			this.hierarchy = hierarchy;
			this.itemId = itemId;

			this.Data = new DynamicHierarchyNode(this);
		}

		private uint itemId;

		public uint ItemId
		{
			get { return itemId; }
		}

		private IVsHierarchy hierarchy;

		public IVsHierarchy Hierarchy
		{
			get { return hierarchy; }
		}

		public dynamic Data { get; private set; }

		public string Name
		{
			get { return GetProperty<string>(__VSHPROPID.VSHPROPID_Name); }
		}

		public Guid Guid
		{
			get
			{
				var guidString = this.Data.Guid;

				if (!string.IsNullOrEmpty(guidString))
				{
					return new Guid(guidString);
				}

				return Guid.Empty;
			}
		}

		private string fullName;

		public string FullName
		{
			get
			{
				if (string.IsNullOrEmpty(fullName))
				{
					fullName = string.Empty;

					var prj = Hierarchy as IVsProject;

					if (prj != null)
					{
						prj.GetMkDocument(itemId, out fullName);

						if (fullName == null)
						{
							fullName = string.Empty;
						}
					}
				}

				return fullName;
			}
		}

		public object ExtObject
		{
			get { return GetProperty<object>(__VSHPROPID.VSHPROPID_ExtObject); }
		}

		public object BrowsableObject
		{
			get
			{
				return GetProperty<object>(__VSHPROPID.VSHPROPID_BrowseObject);
			}
		}

		public string Subtype
		{
			get
			{
				return GetProperty<string>(__VSHPROPID.VSHPROPID_ItemSubType);
			}
		}

		public override string ToString()
		{
			return this.Name;
		}

		public void Open()
		{
			var item = this.ExtObject as ProjectItem;

			if (item != null)
			{
				var window = item.DTE.OpenFile(EnvDTE.Constants.vsViewKindPrimary, this.FullName);

				window.Activate();
			}
		}

		public T GetObject<T>()
			where T : class
		{
			return (hierarchy as T);
		}

		public T GetProperty<T>(__VSHPROPID propId)
		{
			return GetProperty<T>(propId, this.itemId);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					// Dispose managed resources.
				}
			}
			disposed = true;
		}

		private T GetProperty<T>(__VSHPROPID propId, uint itemid)
		{
			object value = null;
			int hr = this.hierarchy.GetProperty(itemid, (int)propId, out value);
			if (hr != VSConstants.S_OK || value == null)
			{
				return default(T);
			}
			return (T)value;
		}

		private static uint GetItemId(object pvar)
		{
			if (pvar == null) return VSConstants.VSITEMID_NIL;
			if (pvar is int) return (uint)(int)pvar;
			if (pvar is uint) return (uint)pvar;
			if (pvar is short) return (uint)(short)pvar;
			if (pvar is ushort) return (uint)(ushort)pvar;
			if (pvar is long) return (uint)(long)pvar;
			return VSConstants.VSITEMID_NIL;
		}

		private static uint ResolveItemId(IVsHierarchy hierarchy)
		{
			object extObject;
			uint itemId = 0;
			IVsHierarchy tempHierarchy;

			ErrorHandler.ThrowOnFailure(
				hierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_BrowseObject, out extObject));

			var browseObject = extObject as IVsBrowseObject;

			if (browseObject != null)
			{
				browseObject.GetProjectItem(out tempHierarchy, out itemId);
			}

			return itemId;
		}

		~HierarchyNode()
		{
			Dispose(false);
		}
	}
}
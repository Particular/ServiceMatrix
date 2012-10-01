using System;
using System.Diagnostics;
using System.Dynamic;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace NServiceBus.Modeling.EndpointDesign
{
	public class DynamicHierarchyNode : DynamicObject
	{
		private HierarchyNode node;

		public DynamicHierarchyNode(HierarchyNode node)
		{
			this.node = node;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			return this.SetValue(binder.Name, value);
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = this.GetValue(binder.Name);

			return true;
		}

		[DebuggerStepThrough]
		public object GetValue(string name)
		{
			string value = null;

			// First try automation properties.
			var item = this.node.ExtObject as ProjectItem;

			if (item != null)
			{
				Property property = null;

				try
				{
					if (item.Properties != null)
					{
						property = item.Properties.Item(name);
					}
				}
				catch (ArgumentException)
				{
					property = null;
				}

				if (property != null)
				{
					return property.Value;
				}
			}

			// Try MSBuild properties
			var storage = this.node.GetObject<IVsHierarchy>() as IVsBuildPropertyStorage;

			if (storage != null)
			{
				storage.GetItemAttribute(this.node.ItemId, name, out value);
			}

			return value;
		}

		[DebuggerStepThrough]
		public bool SetValue(string name, object value)
		{
			// First try automation properties.
			var item = this.node.ExtObject as ProjectItem;

			if (item != null)
			{
				Property property = null;

				try
				{
					if (item.Properties != null)
					{
						property = item.Properties.Item(name);
					}
				}
				catch (ArgumentException)
				{
					property = null;
				}

				if (property != null)
				{
					try
					{
						property.Value = value;
						return true;
					}
					catch
					{
						return false;
					}
				}
			}

			// Try MSBuild properties.
			if (value == null)
			{
				throw new NotSupportedException("Cannot set null value for custom MSBuild item properties.");
			}

			var storage = this.node.GetObject<IVsHierarchy>() as IVsBuildPropertyStorage;

			if (storage != null)
			{
				return ErrorHandler.Succeeded(storage.SetItemAttribute(this.node.ItemId, name, value.ToString()));
			}

			return false;
		}
	}
}
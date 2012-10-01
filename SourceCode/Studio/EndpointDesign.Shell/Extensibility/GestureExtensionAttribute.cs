using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Modeling.Diagrams.ExtensionEnablement;

namespace NServiceBus.Modeling.EndpointDesign
{
	[MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class GestureExtensionAttribute : ExportAttribute
	{
		public GestureExtensionAttribute()
			: base(typeof(IGestureExtension))
		{
		}

		public object MetadataFilter
		{
			get { return ExtensibilityConstants.MetadataFilter; }
		}
	}
}
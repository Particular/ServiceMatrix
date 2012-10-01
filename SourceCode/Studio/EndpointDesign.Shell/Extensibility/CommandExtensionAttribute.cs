using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;

namespace NServiceBus.Modeling.EndpointDesign
{
	[MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class CommandExtensionAttribute : ExportAttribute
	{
		public CommandExtensionAttribute()
			: base(typeof(ICommandExtension))
		{
		}

		public object MetadataFilter
		{
			get { return ExtensibilityConstants.MetadataFilter; }
		}
	}
}
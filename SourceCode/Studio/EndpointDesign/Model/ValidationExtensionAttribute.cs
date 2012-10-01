using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Modeling.Validation;

namespace NServiceBus.Modeling.EndpointDesign
{
	[MetadataAttribute, AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class ValidationExtensionAttribute : ExportAttribute
	{
		public ValidationExtensionAttribute()
			: base(typeof(Action<ValidationContext, object>))
		{
		}

		public object MetadataFilter
		{
			get { return ExtensibilityConstants.MetadataFilter; }
		}
	}
}
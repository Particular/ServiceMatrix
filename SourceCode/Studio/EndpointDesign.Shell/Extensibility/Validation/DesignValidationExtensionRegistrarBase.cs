using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;

namespace NServiceBus.Modeling.EndpointDesign
{
	internal abstract partial class DesignValidationExtensionRegistrarBase : ValidationExtensionRegistrar
	{
		private const string MetadataFilterProperty = "MetadataFilter";

		protected override string MetadataFilter
		{
			get { return ExtensibilityConstants.MetadataFilter; }
		}

		protected override bool CanImport(Lazy<Delegate, IDictionary<string, object>> lazyImport)
		{
			Guard.NotNull(() => lazyImport, lazyImport);

			if (!string.IsNullOrEmpty(this.MetadataFilter))
			{
				return lazyImport.Metadata.ContainsKey(MetadataFilterProperty) &&
					lazyImport.Metadata[MetadataFilterProperty].Equals(this.MetadataFilter);
			}

			return true;
		}
	}
}
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using Microsoft.VisualStudio.Modeling.Shell.ExtensionEnablement;

namespace NServiceBus.Modeling.EndpointDesign
{
	internal abstract partial class DesignCommandExtensionRegistrarBase : CommandExtensionRegistrar
	{
		private const string MetadataFilterProperty = "MetadataFilter";
		private readonly Guid commandSetGuid;

		protected DesignCommandExtensionRegistrarBase()
		{
			this.commandSetGuid = new Guid(Constants.EndpointDesignCommandSetId);
		}

		protected override int CommandExtensionDefaultStartId
		{
			get { return 0x4000; }
		}

		protected override Guid CommandSetGuid
		{
			get { return this.commandSetGuid; }
		}

		protected override string MetadataFilter
		{
			get { return ExtensibilityConstants.MetadataFilter; }
		}

		protected override bool CanImport(Lazy<ICommandExtension, IDictionary<string, object>> lazyImport)
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
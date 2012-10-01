using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using System.Collections.Generic;

namespace NServiceBus.Modeling.EndpointDesign
{
    abstract partial class EndpointDesignPackageBase
	{
		partial void InitializeExtensions()
		{
			var commandExtensionRegistrar = new DesignCommandExtensionRegistrar();

			if(ModelingCompositionContainer.CompositionService != null)
			{
				ModelingCompositionContainer.CompositionService.SatisfyImportsOnce(commandExtensionRegistrar);

				commandExtensionRegistrar.Initialize(this);
			}
		}
	}
}

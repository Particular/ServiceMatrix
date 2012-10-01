using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Validation;

namespace NServiceBus.Modeling.EndpointDesign
{
	internal abstract partial class EndpointDesignDocDataBase
	{
		partial void SetValidationExtensionRegistrar(ValidationController validationController)
		{
			var validationExtensionRegistrar = new DesignValidationExtensionRegistrar();

			if(ModelingCompositionContainer.CompositionService != null)
			{
				ModelingCompositionContainer.CompositionService.SatisfyImportsOnce(validationExtensionRegistrar);
			}

			if(validationController != null)
			{
				validationController.ValidationExtensionRegistrar = validationExtensionRegistrar;
			}
		}
	}
}
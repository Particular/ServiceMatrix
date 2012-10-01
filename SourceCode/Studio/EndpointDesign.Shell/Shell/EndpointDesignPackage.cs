using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.VisualStudio.Modeling.Shell;

namespace NServiceBus.Modeling.EndpointDesign
{
	public partial class EndpointDesignPackage
	{
		protected override void Initialize()
		{
			((CompositionContainer)ModelingCompositionContainer.CompositionService).ComposeExportedValue<ModelingPackage>(this);

			base.Initialize();
		}
	}
}

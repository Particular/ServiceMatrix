using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;

namespace NServiceBus.Modeling.EndpointDesign
{
	public abstract class ModelingCommand<TTarget> : ICommandExtension where TTarget : class
	{
		private IMonitorSelectionService monitorSelection;

		public virtual string Text
		{
			get { return string.Empty; }
		}

		[CLSCompliant(false)]
		protected IMonitorSelectionService MonitorSelection
		{
			get
			{
				if (this.monitorSelection == null)
				{
					this.monitorSelection =
						(IMonitorSelectionService)((IServiceProvider)this.ModelingPackage)
							.GetService(typeof(IMonitorSelectionService));
				}

				return this.monitorSelection;
			}
		}

		[CLSCompliant(false)]
		protected SingleDiagramDocView View
		{
			get
			{
				return this.MonitorSelection.CurrentDocumentView as SingleDiagramDocView;
			}
		}

		[CLSCompliant(false)]
		protected ModelingDocData DocData
		{
			get
			{
				return this.MonitorSelection.CurrentDocument as ModelingDocData;
			}
		}

		protected virtual IEnumerable<TTarget> CurrentSelection
		{
			get
			{
				var currentSelectionContainer = this.MonitorSelection.CurrentSelectionContainer as ModelingWindowPane;

				if (currentSelectionContainer != null)
				{
					var selectedShapes = currentSelectionContainer.GetSelectedComponents().OfType<ShapeElement>();

					return selectedShapes.Where(shape => shape.ModelElement is TTarget)
						.Select(shape => shape.ModelElement as TTarget);
				}

				return null;
			}
		}

		[Import(typeof(SVsServiceProvider))]
		private IServiceProvider ServiceProvider { get; set; }

		[Import(typeof(ModelingPackage))]
		private ModelingPackage ModelingPackage { get; set; }

		public virtual void Execute(IMenuCommand command)
		{
		}

		public virtual void QueryStatus(IMenuCommand command)
		{
			Guard.NotNull(() => command, command);

			command.Visible = command.Enabled =
				this.CurrentSelection != null && this.CurrentSelection.Count() > 0;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using NServiceBusStudio.Automation.Properties;
using System.Globalization;
using Microsoft.VisualStudio.Patterning.Library.Commands;

namespace NServiceBusStudio.Automation.Commands
{
	[CLSCompliant(false)]
	[Category("Pattern Automation")]
	public class VerifyElementIsValid : FeatureCommand
	{
		private static readonly ITraceSource tracer = Tracer.GetSourceFor<VerifyElementIsValid>();
        private const bool DefaultValidateAscendants = false;
        private const bool DefaultValidateDescendants = true;

        /// <summary>
        /// Gets or sets whether to validate the ascendants of the current element.
        /// </summary>
        [DefaultValue(DefaultValidateDescendants)]
        public bool ValidateAscendants { get; set; }

		/// <summary>
		/// Gets or sets whether to validate the descendants of the current element.
		/// </summary>
		[DefaultValue(DefaultValidateDescendants)]
		public bool ValidateDescendants { get; set; }

		/// <summary>
		/// Gets the current element.
		/// </summary>
		[Required]
		[Import(AllowDefault = true)]
		public IProductElement CurrentElement { get; set; }

		/// <summary>
		/// Gets the product manager.
		/// </summary>
		[Required]
		[Import(AllowDefault = true)]
		public IPatternManager ProductManager { get; set; }

		public override void Execute()
		{
			Validator.ValidateObject(this, new ValidationContext(this, null, null));

			tracer.TraceInformation(
				Resources.VerifyElementIsValid_TraceInitial, this.CurrentElement.InstanceName, this.ValidateDescendants);

			var instances = this.ValidateDescendants ? this.CurrentElement.Traverse().OfType<IInstanceBase>() : new[] { this.CurrentElement };
            instances = instances.Concat(this.ValidateAscendants ? RecursiveElementParent (this.CurrentElement.Parent) : Enumerable.Empty<IInstanceBase>());

			var elements = instances.Concat(instances.OfType<IProductElement>().SelectMany(e => e.Properties));

			var result = this.ProductManager.Validate(elements);

			tracer.TraceInformation(
				Resources.VerifyElementIsValid_TraceEvaluation, this.CurrentElement.InstanceName, this.ValidateDescendants, result);

			if (!result)
			{
				var format = this.ValidateDescendants ?
					Resources.VerifyElementIsValid_ElementOrDescendentNotValid :
					Resources.VerifyElementIsValid_ElementNotValid;

				var message = String.Format(CultureInfo.CurrentCulture, format, this.CurrentElement.InstanceName);

				throw new OperationCanceledException(message);
			}
		}

        private IEnumerable<IInstanceBase> RecursiveElementParent(IInstanceBase parent)
        {
            if (parent == null)
            {
                return Enumerable.Empty<IInstanceBase>();
            }

            return new[] { parent }.Concat(RecursiveElementParent(parent.Parent));
        }
	}
}

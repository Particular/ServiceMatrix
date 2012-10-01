using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;

namespace NServiceBus.Modeling.EndpointDesign
{
	public class EndpointModelValidation
	{
		[ValidationExtensionAttribute]
		[ValidationMethod(ValidationCategories.Menu)]
		public void Validate(ValidationContext context, EndpointModel element)
		{
			ValidateDuplicateElementNames(context, element);
		}

		private static void ValidateDuplicateElementNames(ValidationContext context, EndpointModel element)
		{
			var group = element.Endpoints.GroupBy(e => e.Name);

			var duplicateNames = group.Where(e => e.Count() > 1).Select(g => g.Key);

			if(duplicateNames.Any())
			{
				string error = String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.DuplicateNamesException,
					string.Join(",", duplicateNames.ToArray()));

				context.LogError(
					error,
					Properties.Resources.ErrorCode103,
					element);
			}
		}
	}
}

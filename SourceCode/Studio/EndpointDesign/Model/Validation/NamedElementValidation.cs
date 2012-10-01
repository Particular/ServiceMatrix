using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.CSharp;
using System.Globalization;

namespace NServiceBus.Modeling.EndpointDesign
{
	public class NamedElementValidation
	{
		[ValidationExtensionAttribute]
		[ValidationMethod(ValidationCategories.Menu)]
		public void Validate(ValidationContext context, NamedElement element)
		{
			ValidateNameNotEmpty(context, element);
			ValidateIdentifier(context, element);
		}

		private static void ValidateNameNotEmpty(ValidationContext context, NamedElement element)
		{
			if(String.IsNullOrEmpty(element.Name))
			{
				string error = String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.NameIsEmptyException,
					element.GetDomainClass().Name);

				context.LogError(
					error,
					Properties.Resources.ErrorCode101,
					element);
			}
		}

		private static void ValidateIdentifier(ValidationContext context, NamedElement element)
		{
			if(!String.IsNullOrEmpty(element.Name))
			{
				var provider = CSharpCodeProvider.CreateProvider("CSharp");

				if(!provider.IsValidIdentifier(element.Name))
				{
					string error = String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.NameInvalidIdentifierException,
						element.Name);

					context.LogError(
						error,
						Properties.Resources.ErrorCode102,
						element);
				}
			}
		}
	}
}

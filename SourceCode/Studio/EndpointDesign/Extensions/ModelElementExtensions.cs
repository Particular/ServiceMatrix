using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.Modeling;

namespace NServiceBus.Modeling.EndpointDesign
{
	public static class ModelElementExtensions
	{
		public static T WithTransaction<T>(this T modelElement, Action<T> action) where T : ModelElement
		{
			if (modelElement.Store.TransactionManager.InTransaction)
			{
				action(modelElement);
			}
			else
			{
				modelElement.Store.TransactionManager.DoWithinTransaction(() => action(modelElement));
			}

			return modelElement;
		}
	
		public static T Create<T>(this ModelElement parent) where T : ModelElement
		{
            Guard.NotNull(() => parent, parent);

			if (!parent.Store.TransactionManager.InTransaction)
			{
				using (var tx = parent.Store.TransactionManager.BeginTransaction("Creating: " + typeof(T).Name))
				{
					var result = CreateChildElement<T>(parent);
					tx.Commit();
					return result;
				}
			}

			return CreateChildElement<T>(parent);
		}

		private static T CreateChildElement<T>(ModelElement parent) where T : ModelElement
		{
			var childClass = parent.Partition.DomainDataDirectory.DomainClasses
				.FirstOrDefault(dci => typeof(T).IsAssignableFrom(dci.ImplementationClass) && !dci.ImplementationClass.IsAbstract);
			var elementOperations = new ElementOperations(parent.Store, parent.Partition);

			if (elementOperations != null)
			{
				var elementGroupPrototype = new ElementGroupPrototype(parent.Partition, childClass.Id);

				var partition = elementOperations.ChooseMergeTarget(parent, elementGroupPrototype).Partition;
				var element = (T)partition.ElementFactory.CreateElement(childClass);
				var elementGroup = new ElementGroup(partition);
				elementGroup.Add(element);
				elementGroup.MarkAsRoot(element);
				elementOperations.MergeElementGroup(parent, elementGroup);
				return element;
			}

			return default(T);
		}
	}
}
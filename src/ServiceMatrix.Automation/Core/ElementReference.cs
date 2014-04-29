using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;
using NServiceBusStudio.Automation.Properties;

namespace NServiceBusStudio.Core
{
	/// <summary>
	/// Encapsulates the behavior for properties that reference other elements 
	/// within the toolkit.
	/// </summary>
	public class ElementReference<T> : IElementReference<T>, IEquatable<ElementReference<T>>
		where T : IToolkitElement
	{
		private Func<IEnumerable<T>> allValidValues;
		private PropertyReference<string> idProperty;
		private PropertyReference<string> nameProperty;

		public ElementReference(
			Func<IEnumerable<T>> allValidValues,
			PropertyReference<string> idProperty,
			PropertyReference<string> nameProperty)
		{
			this.allValidValues = allValidValues;
			this.idProperty = idProperty;
			this.nameProperty = nameProperty;
			NoneText = Strings.ElementReference.DefaultNone;
			NoneDescription = Strings.ElementReference.DefaultNoneDescription;
		}

		/// <summary>
		/// Gets or sets the default string to display when 
		/// no reference is set.
		/// </summary>
		public string NoneText { get; set; }

		/// <summary>
		/// Gets or sets the description to display for the None selection.
		/// </summary>
		public string NoneDescription { get; set; }

		/// <summary>
		/// Refreshes the reference that this instance points to.
		/// </summary>
		public void Refresh()
		{
			if (string.IsNullOrEmpty(idProperty.Value))
			{
				if (!string.IsNullOrEmpty(nameProperty.Value))
					nameProperty.Value = string.Empty;

				return;
			}

			var id = new Guid(idProperty.Value);
			var referencedElement = allValidValues()
				.Select(x => x.As<IProductElement>())
				.FirstOrDefault(x => x.Id == id);

			if (referencedElement == null)
			{
				idProperty.Value = string.Empty;
				nameProperty.Value = string.Empty;

				return;
			}

			if (nameProperty.Value != referencedElement.InstanceName)
				nameProperty.Value = referencedElement.InstanceName;
		}

		/// <summary>
		/// Gets or sets the referenced value.
		/// </summary>
		public T Value
		{
			get
			{
				if (string.IsNullOrEmpty(idProperty.Value))
					return default(T);

				var id = new Guid(idProperty.Value);
				return allValidValues()
					.FirstOrDefault(x => x.As<IProductElement>().Id == id);
			}
			set
			{
				if (value == null)
				{
					idProperty.Value = string.Empty;
					nameProperty.Value = string.Empty;
				}
				else
				{
					var element = value.As<IProductElement>();
					idProperty.Value = element.Id.ToString();
					nameProperty.Value = value.InstanceName;
                    value.InstanceNameChanged += (s, e) =>
                    {
                        Refresh();
                    };
				}
			}
		}

		#region Equality

		public bool Equals(ElementReference<T> other)
		{
			return Equals(this, other);
		}

		public override bool Equals(object obj)
		{
			return Equals(this, obj as ElementReference<T>);
		}

		public static bool Equals(ElementReference<T> obj1, ElementReference<T> obj2)
		{
			if (Object.Equals(null, obj1) ||
				Object.Equals(null, obj2) ||
				obj1.GetType() != obj2.GetType() ||
				Equals(null, obj1.Value) ||
				Equals(null, obj2.Value))
				return false;

			if (ReferenceEquals(obj1, obj2)) return true;

			var element1 = obj1.Value.As<IProductElement>();
			var element2 = obj2.Value.As<IProductElement>();

			if (Equals(null, element1) ||
				Equals(null, element2))
				return false;

			return element1.Id == element2.Id;
		}

		public override int GetHashCode()
		{
			if (Equals(default(T), Value))
				return base.GetHashCode();
			else
				return Value.As<IProductElement>().Id.GetHashCode();
		}

		#endregion
	}
}

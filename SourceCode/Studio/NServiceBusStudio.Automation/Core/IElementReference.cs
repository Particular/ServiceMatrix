using System;
using NuPattern.Runtime;
using NuPattern.Runtime.ToolkitInterface;

namespace NServiceBusStudio.Core
{
	public interface IElementReference<T>
		where T : IToolkitInterface
	{
		/// <summary>
		/// Gets or sets the default string to display when 
		/// no reference is set.
		/// </summary>
		string NoneText { get; set; }

		/// <summary>
		/// Gets or sets the description to display for the None selection.
		/// </summary>
		string NoneDescription { get; set; }

		/// <summary>
		/// Refreshes the reference that this instance points to.
		/// </summary>
		void Refresh();

		/// <summary>
		/// Gets or sets the referenced value.
		/// </summary>
		T Value { get; set; }
	}
}

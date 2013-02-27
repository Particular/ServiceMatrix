using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;

namespace NServiceBusStudio
{
	/// <summary>
	/// Extends the <see cref="IToolkitInterface"/> implemented by 
	/// all the interface layer with additional members that 
	/// are implemented by all toolkit elements.
	/// </summary>
	public interface IToolkitElement : IToolkitInterface, INamedInstance
	{
		/// <summary>
		/// Occurs when the element instance name changed.
		/// </summary>
		event EventHandler InstanceNameChanged;
	}
}

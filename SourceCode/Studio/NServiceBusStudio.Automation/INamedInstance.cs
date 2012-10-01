using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBusStudio
{
	/// <summary>
	/// An element that has an instance name.
	/// </summary>
	public interface INamedInstance
	{
		/// <summary>
		/// Gets or sets the name of the instance.
		/// </summary>
		string InstanceName { get; set; }
	}
}

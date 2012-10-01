using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBus.Modeling.EndpointDesign.Interfaces
{
	public enum EndpointType
	{
		NServiceBusHost,
		SelfHost,
		Web,
		WebMVC,
		WinForms,
		WPF
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;
using NuPattern.Runtime.ToolkitInterface;
using NuPattern.ComponentModel;


namespace NServiceBusStudio.Core.Design
{
	public interface IElementReferenceStrategy<in TOwner, TReference>
		where TOwner : class, IToolkitInterface
		where TReference : class, IToolkitInterface
	{
		IElementReference<TReference> GetReference(TOwner owner);
		IEnumerable<StandardValue> GetStandardValues(TOwner owner);
		TReference NullValue { get; }
	}
}

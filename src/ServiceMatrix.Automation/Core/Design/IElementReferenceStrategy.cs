namespace NServiceBusStudio.Core.Design
{
    using System.Collections.Generic;
    using NuPattern.Runtime.ToolkitInterface;
    using NuPattern.ComponentModel;

	public interface IElementReferenceStrategy<in TOwner, TReference>
		where TOwner : class, IToolkitInterface
		where TReference : class, IToolkitInterface
	{
		IElementReference<TReference> GetReference(TOwner owner);
		IEnumerable<StandardValue> GetStandardValues(TOwner owner);
		TReference NullValue { get; }
	}
}

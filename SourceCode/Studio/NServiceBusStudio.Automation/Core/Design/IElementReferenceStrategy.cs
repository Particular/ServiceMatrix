using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Design;

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

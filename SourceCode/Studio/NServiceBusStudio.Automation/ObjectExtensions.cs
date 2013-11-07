using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;
using System.ComponentModel;
using NuPattern.Runtime.ToolkitInterface;

namespace NServiceBusStudio
{
	/// <summary>
	/// Extension methods for working with untyped objects 
	/// that are toolkit elements.
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		/// Attempts to convert the given object to a 
		/// toolkit interface layer type or runtime type.
		/// </summary>
		public static T As<T>(this object element)
			where T : class
		{
			if (element == null)
				return default(T);

			var typed = element as T;
			if (typed != null)
				return typed;

            var interfaceLayer = element as IToolkitInterface;
            if (interfaceLayer != null)
                // Conversion on the typed interface layer itself.
                return interfaceLayer.As<T>();

            try
            {
                var instanceBase = element as IInstanceBase;
                if (instanceBase != null)
                    // Invokes the ToolkitInterfaceLayer
                    return ToolkitInterfaceLayer.As<T>(instanceBase);
            }
            catch (NotSupportedException)
            {
                var ielement = element as IElement;
                if (element != null)
                {
                    // Invokes the ToolkitInterfaceLayer
                    var telement = ToolkitInterfaceLayer.As<IToolkitInterface>(ielement) as T;
                    if (telement != null)
                    {
                        return telement;
                    }
                }
            }

			return default(T);
		}
    }
}

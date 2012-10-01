using System;

namespace NServiceBus.Modeling.EndpointDesign
{
	public static class ServiceProviderExtensions
	{
		public static T GetService<T>(this IServiceProvider provider)
		{
			return (T)provider.GetService(typeof(T));
		}

		public static TService GetService<TReg, TService>(this IServiceProvider provider)
		{
			return (TService)provider.GetService(typeof(TReg));
		}
	}
}
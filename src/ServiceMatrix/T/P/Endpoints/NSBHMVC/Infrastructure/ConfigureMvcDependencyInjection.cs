using System;
using System.Linq;
using System.Web.Mvc;
using NServiceBus;

namespace $safeprojectname$.Infrastructure
{
    public static class ConfigureMvcDependecyInjection
    {
        public static Configure ForMvc(this Configure configure)
        {
            // Register our controller activator with NSB
            configure.Configurer.RegisterSingleton(typeof(IControllerActivator),
                new NServiceBusControllerActivator());

            // Find every controller class so that we can register it
            var controllers = Configure.TypesToScan
                .Where(t => typeof(IController).IsAssignableFrom(t));

            // Register each controller class with the NServiceBus container
            foreach (Type type in controllers)
                configure.Configurer.ConfigureComponent(type, DependencyLifecycle.InstancePerCall);

            // Find every NServiceBus component class so that we can register it
            var components = Configure.TypesToScan
                .Where(t => typeof(INServiceBusComponent).IsAssignableFrom(t));

            // Register each NServiceBus component class with the NServiceBus container
            foreach (Type type in components)
                configure.Configurer.ConfigureComponent(type, DependencyLifecycle.InstancePerCall);

            // Set the MVC dependency resolver to use our resolver
            DependencyResolver.SetResolver(new NServiceBusDependencyResolverAdapter(configure.Builder));

            return configure;
        }
    }
}
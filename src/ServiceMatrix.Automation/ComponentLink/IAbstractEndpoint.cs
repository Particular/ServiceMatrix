using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio;
using NuPattern.Runtime.ToolkitInterface;
using NuPattern.Runtime;
using NServiceBusStudio.Automation.Exceptions;

namespace AbstractEndpoint
{
    public interface IAbstractEndpoint : IToolkitInterface, IProjectReferenced
    {
        string Namespace { get; }
        string InstanceName { get; }

        IAbstractEndpointComponents EndpointComponents { get; }

        // For Usage: Use CustomizationFuncs() extension method instead
        EndpointCustomizationFuncs Customization { get; }

        // Propagate application property values on change
        IEnumerable<string> OverridenProperties { get; }
        string ErrorQueue { get; set; }
        string ForwardReceivedMessagesTo { get; set; }
        bool CommandSenderNeedsRegistration { get; set; }

        string TargetNsbVersion { get; set; }
    }

    public static class AbstractEndpointExtensions
    {
        public static void CheckNameUniqueness(this IAbstractEndpoint endpoint)
        {
            // If opening existing endpoint, return
            if (endpoint.As<IProductElement>().IsSerializing)
            {
                return;
            }

            var endpoints = endpoint.As<IProductElement>().Root.As<IApplication>().Design.Endpoints.GetAll();

            if (endpoints.Any(x => String.Compare(x.InstanceName, endpoint.InstanceName, true) == 0 && x != endpoint))
            {
                var error = "There is already an endpoint with the same name. Please, select a new name for your endpoint.";
                System.Windows.MessageBox.Show(error, "ServiceMatrix - New Endpoint Name Uniqueness", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                throw new ElementAlreadyExistsException(error, endpoint.As<IProductElement>().DefinitionName, endpoint.InstanceName);
            }
        }

        public static void RaiseOnInitializing(this IAbstractEndpoint endpoint)
        {
            endpoint.As<IProductElement>().Root.As<IApplication>().RaiseOnInitializingEndpoint(endpoint);
        }

        public static void RaiseOnInstantiated(this IAbstractEndpoint endpoint)
        {
            endpoint.As<IProductElement>().Root.As<IApplication>().RaiseOnInstantiatedEndpoint(endpoint);
        }

        public static EndpointCustomizationFuncs CustomizationFuncs(this IAbstractEndpoint endpoint)
        {
            if (DefaultCustomizationFuncs == null)
            {
                DefaultCustomizationFuncs = EndpointCustomizationFuncs.CreateDefault();
            }
            if (endpoint == null || endpoint.Customization == null)
            {
                return DefaultCustomizationFuncs;
            }
            else
            {
                return endpoint.Customization;
            }
        }

        private static EndpointCustomizationFuncs DefaultCustomizationFuncs { get; set; }
    }

    public class EndpointCustomizationFuncs
    {
        public Func<IComponent, string> GetBaseSenderType { get; set; }
        public Func<IAbstractEndpoint, IService, string> BuildNamespaceForComponentCode { get; set; }
        public Func<IAbstractEndpoint, IService, string, bool, string> BuildPathForComponentCode { get; set; }
        public Func<IComponent, string> GetClassNameSuffix { get; set; }

        public static EndpointCustomizationFuncs CreateDefault()
        {
            return new EndpointCustomizationFuncs
            {
                GetBaseSenderType = c => string.Empty,
                BuildNamespaceForComponentCode = DefaultBuildNamespaceForComponentCode,
                BuildPathForComponentCode = DefaultBuildPathForComponentCode,
                GetClassNameSuffix = c => string.Empty,
            };
        }

        private static string DefaultBuildNamespaceForComponentCode(IAbstractEndpoint endpoint, IService service)
        {
            return endpoint == null ? string.Empty : string.Format(@"{0}.{1}", endpoint.Project.Data.RootNamespace, service.CodeIdentifier);
        }

        private static string DefaultBuildPathForComponentCode(IAbstractEndpoint endpoint, IService service, string subPath, bool useNewServiceName)
        {
            var result = string.Format(@"{0}\{1}", endpoint.Project.Name, (useNewServiceName) ? service.InstanceName : service.OriginalInstanceName);
            if (subPath != string.Empty && subPath != null)
            {
                result = string.Format(@"{0}\Infrastructure\{1}\{2}", endpoint.Project.Name, subPath, (useNewServiceName) ? service.InstanceName : service.OriginalInstanceName);
            }
            return result;
        }

    }
}

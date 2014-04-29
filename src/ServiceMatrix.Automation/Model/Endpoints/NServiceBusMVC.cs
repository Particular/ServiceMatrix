using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Automation.Extensions;
using abs = AbstractEndpoint;
using NServiceBusStudio;
using System.IO;
using AbstractEndpoint;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio
{
    using System.Windows;

    partial interface INServiceBusMVC : IAbstractEndpoint
    {

    }

    partial class NServiceBusMVC
    {
        public IProject Project
        {
            get { return AsElement().GetProject(); }
        }

        public IAbstractEndpointComponents EndpointComponents
        {
            get { return (NServiceBusMVCComponents)NServiceBusMVCComponents; }
        }

        partial void Initialize()
        {
            CheckMVCIsInstalled();

            AbstractEndpointExtensions.CheckNameUniqueness(this);

            AbstractEndpointExtensions.RaiseOnInitializing(this);

            ErrorQueueChanged += (s, e) =>
            {
                SetOverridenProperties("ErrorQueue", ErrorQueue != AsElement().Root.As<IApplication>().ErrorQueue);
            };
            ForwardReceivedMessagesToChanged += (s, e) =>
            {
                SetOverridenProperties("ForwardReceivedMessagesTo", ForwardReceivedMessagesTo != AsElement().Root.As<IApplication>().ForwardReceivedMessagesTo);
            };
        }

        private void CheckMVCIsInstalled()
        {
            if (!AsElement().IsSerializing)
            {
                var programFiles = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Microsoft ASP.NET\ASP.NET MVC 4\" + VSVersion);
                var programFilesX86 = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\Microsoft ASP.NET\ASP.NET MVC 4\" + VSVersion);

                if (!Directory.Exists(programFiles) &&
                    !Directory.Exists(programFilesX86))
                {
                    var error = "You cannot create this endpoint because ASP.NET MVC 4 is not installed. Install ASP.NET MVC 4 and try again.";
                    MessageBox.Show(error, "ServiceMatrix - ASP.NET MVC 4 not installed", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new OperationCanceledException(error);
                }
            }
        }

        private List<string> overridenProperties = new List<string>();

        public IEnumerable<string> OverridenProperties
        {
            get { return overridenProperties; }
        }

        private void SetOverridenProperties(string propertyName, bool doOverride)
        {
            if (!doOverride)
            {
                if (overridenProperties.Contains(propertyName))
                {
                    overridenProperties.Remove(propertyName);
                }
            }
            else
            {
                if (!overridenProperties.Contains(propertyName))
                {
                    overridenProperties.Add(propertyName);
                }
            }
        }

        private EndpointCustomizationFuncs customization;

        public EndpointCustomizationFuncs Customization
        {
            get
            {
                if (customization == null)
                {
                    customization = EndpointCustomizationFuncs.CreateDefault();
                    customization.GetBaseSenderType = GetBaseSenderType;
                    customization.BuildPathForComponentCode = CustomBuildPathForComponentCode;
                    customization.BuildNamespaceForComponentCode = CustomBuildNamespaceForComponentCode;
                }
                return customization;
            }
        }

        private Func<IComponent, string> GetBaseSenderType
        {
            get
            {
                return c => string.Format("I{0}, ServiceMatrix.Shared.INServiceBusComponent", c.CodeIdentifier);
            }
        }

        private static string CustomBuildNamespaceForComponentCode(IAbstractEndpoint endpoint, IService service)
        {
            return endpoint == null ? string.Empty : string.Format(@"{0}.Components.{1}", endpoint.Project.Data.RootNamespace, service.CodeIdentifier);
        }

        private static string CustomBuildPathForComponentCode(IAbstractEndpoint endpoint, IService service, string subPath, bool useNewServiceName)
        {
            var result = string.Format(@"{0}\Components\{1}", endpoint.Project.Name, (useNewServiceName) ? service.InstanceName : service.OriginalInstanceName);
            if (subPath != string.Empty && subPath != null)
            {
                result = string.Format(@"{0}\Infrastructure\{1}\{2}", endpoint.Project.Name, subPath, (useNewServiceName) ? service.InstanceName : service.OriginalInstanceName);
            }
            return result;
        }
    }
}

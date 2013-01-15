using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using NServiceBusStudio.Automation.Extensions;
using abs = AbstractEndpoint;
using NServiceBusStudio;
using System.IO;
using AbstractEndpoint;

namespace NServiceBusStudio
{
    partial interface INServiceBusMVC : IAbstractEndpoint
    {

    }

    partial class NServiceBusMVC
    {
        public IProject Project
        {
            get { return this.AsElement().GetProject(); }
        }

        public abs.IAbstractEndpointComponents EndpointComponents
        {
            get { return (NServiceBusMVCComponents)this.NServiceBusMVCComponents; }
        }

        partial void Initialize()
        {
            CheckMVCIsInstalled();

            abs.AbstractEndpointExtensions.RaiseOnInitializing(this);

            this.ErrorQueueChanged += (s, e) =>
            {
                this.SetOverridenProperties("ErrorQueue", this.ErrorQueue != this.AsElement().Root.As<IApplication>().ErrorQueue);
            };
            this.ForwardReceivedMessagesToChanged += (s, e) =>
            {
                this.SetOverridenProperties("ForwardReceivedMessagesTo", this.ForwardReceivedMessagesTo != this.AsElement().Root.As<IApplication>().ForwardReceivedMessagesTo);
            };
        }

        private void CheckMVCIsInstalled()
        {
            if (!this.AsElement().IsSerializing)
            {
                var programFiles = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Microsoft ASP.NET\ASP.NET MVC 4");
                var programFilesX86 = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\Microsoft ASP.NET\ASP.NET MVC 4");

                if (!Directory.Exists(programFiles) &&
                    !Directory.Exists(programFilesX86))
                {
                    var error = "You cannot create this endpoint because ASP.NET MVC 4 is not installed. Install ASP.NET MVC 4 and try again.";
                    System.Windows.MessageBox.Show(error, "NService Bus ASP NET MVC Endpoint", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    throw new OperationCanceledException(error);
                }
            }
        }

        private List<string> overridenProperties = new List<string>();

        public IEnumerable<string> OverridenProperties
        {
            get { return this.overridenProperties; }
        }

        private void SetOverridenProperties(string propertyName, bool doOverride)
        {
            if (!doOverride)
            {
                if (this.overridenProperties.Contains(propertyName))
                {
                    this.overridenProperties.Remove(propertyName);
                }
            }
            else
            {
                if (!this.overridenProperties.Contains(propertyName))
                {
                    this.overridenProperties.Add(propertyName);
                }
            }
        }

        private abs.EndpointCustomizationFuncs customization;

        public abs.EndpointCustomizationFuncs Customization
        {
            get
            {
                if (this.customization == null)
                {
                    this.customization = abs.EndpointCustomizationFuncs.CreateDefault();
                    this.customization.GetBaseSenderType = GetBaseSenderType;
                    this.customization.BuildPathForComponentCode = CustomBuildPathForComponentCode;
                    this.customization.BuildNamespaceForComponentCode = CustomBuildNamespaceForComponentCode;
                }
                return this.customization;
            }
        }



        private Func<IComponent, string> GetBaseSenderType
        {
            get
            {
                return c => string.Format("I{0}, NServiceBus.INServiceBusComponent", c.CodeIdentifier, this.Project.Data.RootNamespace);
            }
        }

        private static string CustomBuildNamespaceForComponentCode(abs.IAbstractEndpoint endpoint, IService service)
        {
            return endpoint == null ? string.Empty : string.Format(@"{0}.Components.{1}", endpoint.Project.Data.RootNamespace, service.CodeIdentifier);
        }

        private static string CustomBuildPathForComponentCode(abs.IAbstractEndpoint endpoint, IService service, string subPath)
        {
            var result = string.Format(@"{0}\Components\{1}", endpoint.Project.Name, service.InstanceName);
            if (subPath != string.Empty && subPath != null)
            {
                result = string.Format(@"{0}\Infrastructure\{1}\{2}", endpoint.Project.Name, subPath, service.InstanceName);
            }
            return result;
        }
    }
}

using NServiceBusStudio.Automation.Infrastructure;
using NuPattern;
using NuPattern.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Automation.Extensions;

namespace NServiceBusStudio
{
    partial interface IService
    {
        string OriginalInstanceName { get; set; }
        void Rename(IUriReferenceService uriService, RefactoringManager refactoringManager);
    }

    partial class Service : IRenameRefactoringNamespace
    {
        public void Rename(IUriReferenceService uriService, RefactoringManager refactoringManager)
        {
            var element = this.As<IProductElement>();
            var app = element.Root.As<IApplication>();

            // Remove existing links to Endpoints
            foreach (var component in this.Components.Component)
            {
                foreach (var endpoint in component.DeployedTo)
                {
                    component.RemoveLinks(endpoint);
                }
            }

            // Perform renaming
            var renameNamespaces = new Dictionary<string, string>();
            renameNamespaces.Add(String.Format("{0}.{1}", app.InstanceName, this.OriginalInstanceName),
                                 String.Format("{0}.{1}", app.InstanceName, this.InstanceName));
            renameNamespaces.Add(String.Format("{0}.{1}.{2}", app.InstanceName, app.ProjectNameInternalMessages, this.OriginalInstanceName),
                                 String.Format("{0}.{1}.{2}", app.InstanceName, app.ProjectNameInternalMessages, this.InstanceName));
            renameNamespaces.Add(String.Format("{0}.{1}.{2}", app.InstanceName, app.ProjectNameContracts, this.OriginalInstanceName),
                                 String.Format("{0}.{1}.{2}", app.InstanceName, app.ProjectNameContracts, this.InstanceName));
            renameNamespaces.Add(String.Format("Console.WriteLine(\"{0} received \" + message.GetType().Name);", this.OriginalInstanceName),
                                 String.Format("Console.WriteLine(\"{0} received \" + message.GetType().Name);", this.InstanceName));


            refactoringManager.RenameNamespaces(this.OriginalInstanceName, this.InstanceName, renameNamespaces);
            element.RenameArtifactLinks(uriService, this.OriginalInstanceName, this.InstanceName);

            // Restore existing links to Endpoints
            foreach (var component in this.Components.Component)
            {
                foreach (var endpoint in component.DeployedTo)
                {
                    component.AddLinks(endpoint);
                }
            }
        }

    }
}

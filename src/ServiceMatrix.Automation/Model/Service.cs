namespace NServiceBusStudio
{
    using NServiceBusStudio.Automation.Infrastructure;
    using NuPattern;
    using NuPattern.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBusStudio.Automation.Extensions;

    using System.Windows;

    partial interface IService
    {
        string OriginalInstanceName { get; set; }
        void Rename(IUriReferenceService uriService, RefactoringManager refactoringManager);
    }

    partial class Service : IRenameRefactoringNamespace
    {

        partial void Initialize()
        {
            CheckNameUniqueness(this);
        }

        private void CheckNameUniqueness(Service service)
        {
            // If opening existing service, return
            if (service.As<IProductElement>().IsSerializing)
            {
                return;
            }

            var services = service.As<IProductElement>().Root.As<IApplication>().Design.Services.Service;

            if (services.Any(x => String.Compare(x.InstanceName, service.InstanceName, true) == 0 && x != service))
            {
                var error = "There is already a service with the same name. Please, select a new name for your service.";
                MessageBox.Show(error, "ServiceMatrix - New Service Name Uniqueness", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new OperationCanceledException(error);
            }
        }

        public void Rename(IUriReferenceService uriService, RefactoringManager refactoringManager)
        {
            var element = As<IProductElement>();
            var app = element.Root.As<IApplication>();

            // Remove existing links to Endpoints
            foreach (var component in Components.Component)
            {
                foreach (var endpoint in component.DeployedTo)
                {
                    component.RemoveLinks(endpoint);
                }
            }

            // Perform renaming
            var renameNamespaces = new Dictionary<string, string>
            {
                {String.Format("{0}.{1}", app.InstanceName, OriginalInstanceName), String.Format("{0}.{1}", app.InstanceName, InstanceName)},
                {String.Format("{0}.{1}.{2}", app.InstanceName, app.ProjectNameInternalMessages, OriginalInstanceName), String.Format("{0}.{1}.{2}", app.InstanceName, app.ProjectNameInternalMessages, InstanceName)},
                {String.Format("{0}.{1}.{2}", app.InstanceName, app.ProjectNameContracts, OriginalInstanceName), String.Format("{0}.{1}.{2}", app.InstanceName, app.ProjectNameContracts, InstanceName)},
                {String.Format("Console.WriteLine(\"{0} received \" + message.GetType().Name);", OriginalInstanceName), String.Format("Console.WriteLine(\"{0} received \" + message.GetType().Name);", InstanceName)}
            };

            refactoringManager.RenameNamespaces(OriginalInstanceName, InstanceName, renameNamespaces);
            element.RenameArtifactLinks(uriService, OriginalInstanceName, InstanceName);

            // Restore existing links to Endpoints
            foreach (var component in Components.Component)
            {
                foreach (var endpoint in component.DeployedTo)
                {
                    component.AddLinks(endpoint);
                }
            }
        }

    }
}

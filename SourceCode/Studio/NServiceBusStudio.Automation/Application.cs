using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using NuPattern.Runtime;
using Microsoft.VisualStudio.Shell.Interop;
using NServiceBusStudio.Core;
using NuPattern;

using AbstractEndpoint;
using NuPattern.Diagnostics;

namespace NServiceBusStudio
{
	partial class Application
	{
		private static bool versionInitialized;
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Sets or Gets if the model has changed
        /// and code needs to be regenerated
        /// before next build
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        /// Sets or Gets if the license is Valid
        /// or the trial has not expired yet
        /// </summary>
        public bool IsValidLicensed { get; set; }

		[Import(typeof(SVsServiceProvider))]
		public virtual IServiceProvider ServiceProvider
		{
			set
			{
				// We ensure at this very first chance that the MSBuild 
				// targets file has been initilaized in its proper location 
				// and version. This is used by extending toolkits to reference 
				// our binaries.
				if (!versionInitialized && value != null)
				{
					var shell = value.TryGetService<SVsShell, IVsShell>();
					if (shell != null)
					{
						VersionHelper.SyncTargets(Tracer.Get<Application>(), shell.GetHive());
						versionInitialized = true;
					}
                }
                serviceProvider = value;
			}

            get { return serviceProvider; }
		}

        partial void Create()
        {
            var bag = this.AsProduct().ProductState.PropertyBag;
        }

        public event EventHandler OnApplicationLoaded;

        public void RaiseOnApplicationLoaded()
        {
            if (OnApplicationLoaded != null)
            {
                OnApplicationLoaded(this, EventArgs.Empty);
            }

            // IsDirty should be initialized to true
            this.IsDirty = true;

            // IsValidLicensed should be initialized to true
            this.IsValidLicensed = true;
        }

        // This event is raised even initializing after deserializing the endpoint
        public event EventHandler OnInitializingEndpoint;

        // This event is raised just when it was explicitly instantiated and not
        // on deserialization
        public event EventHandler OnInstantiatedEndpoint;

        // This event is raised just when it was explicitly instantiated and not
        // on deserialization
        public event EventHandler OnInstantiatedComponent;

        public void RaiseOnInitializingEndpoint(IAbstractEndpoint endpoint)
        {
            if (OnInitializingEndpoint != null)
            {
                OnInitializingEndpoint(endpoint, EventArgs.Empty);
            }
        }

        public void RaiseOnInstantiatedEndpoint(IAbstractEndpoint endpoint)
        {
            if (OnInstantiatedEndpoint != null)
            {
                OnInstantiatedEndpoint(endpoint, EventArgs.Empty);
            }
        }

        public void RaiseOnInstantiatedComponent(IComponent component)
        {
            if (OnInstantiatedComponent != null)
            {
                OnInstantiatedComponent(component, EventArgs.Empty);
            }
        }

        private bool firstBuild = false;
        public void CheckForFirstBuild()
        {
            if (!firstBuild)
            {
                firstBuild = true;
                RaiseOnApplicationLoaded();
            }
        }

        public static void ResetIsDirtyFlag()
        {
            if (Application.currentApplication != null)
            {
                Application.currentApplication.IsDirty = true;
            }
        }
    }


    partial interface IApplication
    {
        event EventHandler OnApplicationLoaded;
        void RaiseOnApplicationLoaded();
        bool IsDirty { get; set; }
        bool IsValidLicensed { get; set; }

        // This event is raised even initializing after deserializing the endpoint
        event EventHandler OnInitializingEndpoint;

        // This event is raised just when it was explicitly instantiated and not
        // on deserialization
        event EventHandler OnInstantiatedEndpoint;
        
        // This event is raised just when it was explicitly instantiated and not
        // on deserialization
        event EventHandler OnInstantiatedComponent;

        void RaiseOnInitializingEndpoint(IAbstractEndpoint endpoint);
        void RaiseOnInstantiatedEndpoint(IAbstractEndpoint endpoint);
        void RaiseOnInstantiatedComponent(IComponent component);
        void CheckForFirstBuild();
    }
}

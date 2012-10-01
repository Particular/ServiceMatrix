using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using NServiceBusStudio;

namespace NServiceBusHost
{
	#region Interfaces

	partial interface IAbstractEndpoint : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler NamespaceChanged;
		event EventHandler MasterNodeChanged;
		event EventHandler SLAChanged;
		event EventHandler MessageEndpointMappingsConfigChanged;
		event EventHandler CommandSenderNeedsRegistrationChanged;
		event EventHandler MessageConventionsChanged;
		event EventHandler ComponentsOrderDefinitionChanged;
		event EventHandler ErrorQueueChanged;
		event EventHandler ForwardReceivedMessagesToChanged;
		event EventHandler NumberOfWorkerThreadsChanged;
		event EventHandler MaxRetriesChanged;
		event EventHandler SLEnabledChanged;
		event EventHandler SLNumberOfRetriesChanged;
		event EventHandler SLTimeIncreaseChanged;
	}

	partial interface IComponentLink : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler ComponentIdChanged;
		event EventHandler ComponentNameChanged;
		event EventHandler OrderChanged;
	}

	partial interface IEndpointComponents : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	#endregion Interfaces

	#region Implementations


	partial class AbstractEndpoint : ISupportInitialize
	{
		static AbstractEndpoint()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(AbstractEndpoint)), typeof(AbstractEndpoint));
		}

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsProduct();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IAbstractEndpoint).FullName)
					.GetExports<IAutomationExtension, IFeatureComponentMetadata>()
					.FromFeaturesCatalog()
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			Initialize();
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		#endregion

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };
		public event EventHandler MasterNodeChanged = (sender, args) => { };
		public event EventHandler SLAChanged = (sender, args) => { };
		public event EventHandler MessageEndpointMappingsConfigChanged = (sender, args) => { };
		public event EventHandler CommandSenderNeedsRegistrationChanged = (sender, args) => { };
		public event EventHandler MessageConventionsChanged = (sender, args) => { };
		public event EventHandler ComponentsOrderDefinitionChanged = (sender, args) => { };
		public event EventHandler ErrorQueueChanged = (sender, args) => { };
		public event EventHandler ForwardReceivedMessagesToChanged = (sender, args) => { };
		public event EventHandler NumberOfWorkerThreadsChanged = (sender, args) => { };
		public event EventHandler MaxRetriesChanged = (sender, args) => { };
		public event EventHandler SLEnabledChanged = (sender, args) => { };
		public event EventHandler SLNumberOfRetriesChanged = (sender, args) => { };
		public event EventHandler SLTimeIncreaseChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsProduct().PropertyChanged += OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			ApplicationIsDirty.SetTrue();
			switch (args.PropertyName)
			{
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "MasterNode":
					MasterNodeChanged(sender, args);
					break;
				case "SLA":
					SLAChanged(sender, args);
					break;
				case "MessageEndpointMappingsConfig":
					MessageEndpointMappingsConfigChanged(sender, args);
					break;
				case "CommandSenderNeedsRegistration":
					CommandSenderNeedsRegistrationChanged(sender, args);
					break;
				case "MessageConventions":
					MessageConventionsChanged(sender, args);
					break;
				case "ComponentsOrderDefinition":
					ComponentsOrderDefinitionChanged(sender, args);
					break;
				case "ErrorQueue":
					ErrorQueueChanged(sender, args);
					break;
				case "ForwardReceivedMessagesTo":
					ForwardReceivedMessagesToChanged(sender, args);
					break;
				case "NumberOfWorkerThreads":
					NumberOfWorkerThreadsChanged(sender, args);
					break;
				case "MaxRetries":
					MaxRetriesChanged(sender, args);
					break;
				case "SLEnabled":
					SLEnabledChanged(sender, args);
					break;
				case "SLNumberOfRetries":
					SLNumberOfRetriesChanged(sender, args);
					break;
				case "SLTimeIncrease":
					SLTimeIncreaseChanged(sender, args);
					break;
				case "InstanceName":
					InstanceNameChanged(sender, args);
					break;					
				default:
					break;
			}
		}
	}

	partial class ComponentLink : ISupportInitialize
	{
		static ComponentLink()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComponentLink)), typeof(ComponentLink));
		}

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IComponentLink).FullName)
					.GetExports<IAutomationExtension, IFeatureComponentMetadata>()
					.FromFeaturesCatalog()
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			Initialize();
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		#endregion

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler ComponentIdChanged = (sender, args) => { };
		public event EventHandler ComponentNameChanged = (sender, args) => { };
		public event EventHandler OrderChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			ApplicationIsDirty.SetTrue();
			switch (args.PropertyName)
			{
				case "ComponentId":
					ComponentIdChanged(sender, args);
					break;
				case "ComponentName":
					ComponentNameChanged(sender, args);
					break;
				case "Order":
					OrderChanged(sender, args);
					break;
				case "InstanceName":
					InstanceNameChanged(sender, args);
					break;					
				default:
					break;
			}
		}
	}

	partial class EndpointComponents : ISupportInitialize
	{
		static EndpointComponents()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(EndpointComponents)), typeof(EndpointComponents));
		}

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IEndpointComponents).FullName)
					.GetExports<IAutomationExtension, IFeatureComponentMetadata>()
					.FromFeaturesCatalog()
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			Initialize();
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		#endregion

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			ApplicationIsDirty.SetTrue();
			switch (args.PropertyName)
			{
				case "InstanceName":
					InstanceNameChanged(sender, args);
					break;					
				default:
					break;
			}
		}
	}

	#endregion Implementations
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NuPattern.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using NServiceBusStudio.Automation.Infrastructure;
using NServiceBusStudio.Automation.Extensions;

namespace NServiceBusStudio
{
	#region Interfaces

	partial interface IApplication : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler ForwardReceivedMessagesToChanged;
		event EventHandler ErrorQueueChanged;
		event EventHandler NServiceBusVersionChanged;
		event EventHandler ExtensionPathChanged;
		event EventHandler TransportChanged;
		event EventHandler TransportConnectionStringChanged;
	}

	partial interface IService : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IEvent : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler DoNotAutogenerateComponentsChanged;
	}

	partial interface ICommand : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler DoNotAutogenerateComponentsChanged;
	}

	partial interface IComponent : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler NamespaceChanged;
		event EventHandler AdditionalUsingsChanged;
		event EventHandler InheritsChanged;
		event EventHandler ClassBodyChanged;
		event EventHandler CustomClassBodyChanged;
		event EventHandler IsSagaChanged;
	}

	partial interface IEventLink : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler EventIdChanged;
		event EventHandler EventNameChanged;
		event EventHandler NamespaceChanged;
		event EventHandler ComponentNameChanged;
	}

	partial interface ICommandLink : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler CommandIdChanged;
		event EventHandler CommandNameChanged;
		event EventHandler NamespaceChanged;
		event EventHandler ComponentNameChanged;
		event EventHandler SenderNeedsRegistrationChanged;
		event EventHandler ComponentBaseTypeChanged;
	}

	partial interface ISubscribedEventLink : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler EventIdChanged;
		event EventHandler EventNameChanged;
		event EventHandler NamespaceChanged;
		event EventHandler PublishesAnEventChanged;
		event EventHandler EventCodeIdentifierChanged;
		event EventHandler HandlerFileNameChanged;
		event EventHandler StartsSagaChanged;
	}

	partial interface IProcessedCommandLink : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler CommandIdChanged;
		event EventHandler CommandNameChanged;
		event EventHandler NamespaceChanged;
		event EventHandler PublishesAnEventChanged;
		event EventHandler CommandCodeIdentifierChanged;
		event EventHandler StartsSagaChanged;
	}

	partial interface ILibraryReference : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler LibraryIdChanged;
	}

	partial interface IServiceLibrary : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler FilePathChanged;
	}

	partial interface INServiceBusHost : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler CommandSenderNeedsRegistrationChanged;
		event EventHandler ComponentsOrderDefinitionChanged;
		event EventHandler ErrorQueueChanged;
		event EventHandler ForwardReceivedMessagesToChanged;
		event EventHandler MasterNodeChanged;
		event EventHandler MaxRetriesChanged;
		event EventHandler MessageConventionsChanged;
		event EventHandler MessageEndpointMappingsConfigChanged;
		event EventHandler NamespaceChanged;
		event EventHandler NumberOfWorkerThreadsChanged;
		event EventHandler SLAChanged;
		event EventHandler SLEnabledChanged;
		event EventHandler SLNumberOfRetriesChanged;
		event EventHandler SLTimeIncreaseChanged;
	}

	partial interface INServiceBusHostComponentLink : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler ComponentIdChanged;
		event EventHandler ComponentNameChanged;
		event EventHandler OrderChanged;
	}

	partial interface INServiceBusWeb : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler CommandSenderNeedsRegistrationChanged;
		event EventHandler ErrorQueueChanged;
		event EventHandler ForwardReceivedMessagesToChanged;
		event EventHandler HasComponentsThatPublishEventChanged;
		event EventHandler MaxRetriesChanged;
		event EventHandler MessageConventionsChanged;
		event EventHandler MessageEndpointMappingsConfigChanged;
		event EventHandler NamespaceChanged;
		event EventHandler NumberOfWorkerThreadsChanged;
		event EventHandler SenderBaseTypeChanged;
		event EventHandler SLAChanged;
	}

	partial interface INServiceBusWebComponentLink : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler ComponentIdChanged;
		event EventHandler ComponentNameChanged;
		event EventHandler OrderChanged;
	}

	partial interface INServiceBusMVC : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler CommandSenderNeedsRegistrationChanged;
		event EventHandler ErrorQueueChanged;
		event EventHandler ForwardReceivedMessagesToChanged;
		event EventHandler HasComponentsThatPublishEventChanged;
		event EventHandler MaxRetriesChanged;
		event EventHandler MessageConventionsChanged;
		event EventHandler MessageEndpointMappingsConfigChanged;
		event EventHandler NamespaceChanged;
		event EventHandler NumberOfWorkerThreadsChanged;
		event EventHandler SLAChanged;
	}

	partial interface INServiceBusMVCComponentLink : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler ComponentIdChanged;
		event EventHandler ComponentNameChanged;
		event EventHandler OrderChanged;
	}

	partial interface IContractsProject : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IInternalMessagesProject : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IAuthentication : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler NamespaceChanged;
		event EventHandler CodePathChanged;
		event EventHandler CustomCodePathChanged;
		event EventHandler LocalNamespaceChanged;
	}

	partial interface IUseCase : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IUseCaseStep : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler PatternValueChanged;
		event EventHandler SourceEndpointNameChanged;
		event EventHandler TargetEndpointNameChanged;
		event EventHandler ComponentNameChanged;
		event EventHandler EventNameChanged;
		event EventHandler CommandNameChanged;
		event EventHandler ServiceNameValueChanged;
	}

	partial interface IUseCaseLink : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler ElementTypeChanged;
		event EventHandler LinkedElementIdChanged;
		event EventHandler StartsUseCaseChanged;
	}

	partial interface ILibrary : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler FilePathChanged;
	}

	partial interface IServices : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IContract : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IEvents : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler NamespaceChanged;
	}

	partial interface ICommands : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler NamespaceChanged;
	}

	partial interface IComponents : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IPublishes : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface ISubscribes : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface ILibraryReferences : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IServiceLibraries : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler NamespaceChanged;
	}

	partial interface IEndpoints : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface INServiceBusHostComponents : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface INServiceBusWebComponents : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface INServiceBusMVCComponents : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IInfrastructure : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface ISecurity : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IDummyCollection : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface IUseCases : IToolkitElement
	{
		string CodeIdentifier { get; }

	}

	partial interface ILibraries : IToolkitElement
	{
		string CodeIdentifier { get; }

		event EventHandler NamespaceChanged;
	}

	#endregion Interfaces

	#region Implementations


	partial class Application : ISupportInitialize
	{
		static Application()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Application)), typeof(Application));
			Application.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsProduct();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IApplication).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IApplication).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler ForwardReceivedMessagesToChanged = (sender, args) => { };
		public event EventHandler ErrorQueueChanged = (sender, args) => { };
		public event EventHandler NServiceBusVersionChanged = (sender, args) => { };
		public event EventHandler ExtensionPathChanged = (sender, args) => { };
		public event EventHandler TransportChanged = (sender, args) => { };
		public event EventHandler TransportConnectionStringChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsProduct().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "ForwardReceivedMessagesTo":
					ForwardReceivedMessagesToChanged(sender, args);
					break;
				case "ErrorQueue":
					ErrorQueueChanged(sender, args);
					break;
				case "NServiceBusVersion":
					NServiceBusVersionChanged(sender, args);
					break;
				case "ExtensionPath":
					ExtensionPathChanged(sender, args);
					break;
				case "Transport":
					TransportChanged(sender, args);
					break;
				case "TransportConnectionString":
					TransportConnectionStringChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsProduct().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Service : ISupportInitialize
	{
		static Service()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Service)), typeof(Service));
			Service.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IService).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IService).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Event : ISupportInitialize
	{
		static Event()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Event)), typeof(Event));
			Event.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IEvent).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IEvent).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler DoNotAutogenerateComponentsChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "DoNotAutogenerateComponents":
					DoNotAutogenerateComponentsChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Command : ISupportInitialize
	{
		static Command()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Command)), typeof(Command));
			Command.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(ICommand).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(ICommand).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler DoNotAutogenerateComponentsChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "DoNotAutogenerateComponents":
					DoNotAutogenerateComponentsChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Component : ISupportInitialize
	{
		static Component()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Component)), typeof(Component));
			Component.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IComponent).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IComponent).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };
		public event EventHandler AdditionalUsingsChanged = (sender, args) => { };
		public event EventHandler InheritsChanged = (sender, args) => { };
		public event EventHandler ClassBodyChanged = (sender, args) => { };
		public event EventHandler CustomClassBodyChanged = (sender, args) => { };
		public event EventHandler IsSagaChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "AdditionalUsings":
					AdditionalUsingsChanged(sender, args);
					break;
				case "Inherits":
					InheritsChanged(sender, args);
					break;
				case "ClassBody":
					ClassBodyChanged(sender, args);
					break;
				case "CustomClassBody":
					CustomClassBodyChanged(sender, args);
					break;
				case "IsSaga":
					IsSagaChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class EventLink : ISupportInitialize
	{
		static EventLink()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(EventLink)), typeof(EventLink));
			EventLink.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IEventLink).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IEventLink).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler EventIdChanged = (sender, args) => { };
		public event EventHandler EventNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };
		public event EventHandler ComponentNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "EventId":
					EventIdChanged(sender, args);
					break;
				case "EventName":
					EventNameChanged(sender, args);
					break;
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "ComponentName":
					ComponentNameChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class CommandLink : ISupportInitialize
	{
		static CommandLink()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(CommandLink)), typeof(CommandLink));
			CommandLink.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(ICommandLink).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(ICommandLink).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler CommandIdChanged = (sender, args) => { };
		public event EventHandler CommandNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };
		public event EventHandler ComponentNameChanged = (sender, args) => { };
		public event EventHandler SenderNeedsRegistrationChanged = (sender, args) => { };
		public event EventHandler ComponentBaseTypeChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "CommandId":
					CommandIdChanged(sender, args);
					break;
				case "CommandName":
					CommandNameChanged(sender, args);
					break;
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "ComponentName":
					ComponentNameChanged(sender, args);
					break;
				case "SenderNeedsRegistration":
					SenderNeedsRegistrationChanged(sender, args);
					break;
				case "ComponentBaseType":
					ComponentBaseTypeChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class SubscribedEventLink : ISupportInitialize
	{
		static SubscribedEventLink()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(SubscribedEventLink)), typeof(SubscribedEventLink));
			SubscribedEventLink.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(ISubscribedEventLink).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(ISubscribedEventLink).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler EventIdChanged = (sender, args) => { };
		public event EventHandler EventNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };
		public event EventHandler PublishesAnEventChanged = (sender, args) => { };
		public event EventHandler EventCodeIdentifierChanged = (sender, args) => { };
		public event EventHandler HandlerFileNameChanged = (sender, args) => { };
		public event EventHandler StartsSagaChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "EventId":
					EventIdChanged(sender, args);
					break;
				case "EventName":
					EventNameChanged(sender, args);
					break;
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "PublishesAnEvent":
					PublishesAnEventChanged(sender, args);
					break;
				case "EventCodeIdentifier":
					EventCodeIdentifierChanged(sender, args);
					break;
				case "HandlerFileName":
					HandlerFileNameChanged(sender, args);
					break;
				case "StartsSaga":
					StartsSagaChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class ProcessedCommandLink : ISupportInitialize
	{
		static ProcessedCommandLink()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ProcessedCommandLink)), typeof(ProcessedCommandLink));
			ProcessedCommandLink.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IProcessedCommandLink).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IProcessedCommandLink).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler CommandIdChanged = (sender, args) => { };
		public event EventHandler CommandNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };
		public event EventHandler PublishesAnEventChanged = (sender, args) => { };
		public event EventHandler CommandCodeIdentifierChanged = (sender, args) => { };
		public event EventHandler StartsSagaChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "CommandId":
					CommandIdChanged(sender, args);
					break;
				case "CommandName":
					CommandNameChanged(sender, args);
					break;
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "PublishesAnEvent":
					PublishesAnEventChanged(sender, args);
					break;
				case "CommandCodeIdentifier":
					CommandCodeIdentifierChanged(sender, args);
					break;
				case "StartsSaga":
					StartsSagaChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class LibraryReference : ISupportInitialize
	{
		static LibraryReference()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(LibraryReference)), typeof(LibraryReference));
			LibraryReference.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(ILibraryReference).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(ILibraryReference).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler LibraryIdChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "LibraryId":
					LibraryIdChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class ServiceLibrary : ISupportInitialize
	{
		static ServiceLibrary()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ServiceLibrary)), typeof(ServiceLibrary));
			ServiceLibrary.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IServiceLibrary).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IServiceLibrary).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler FilePathChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "FilePath":
					FilePathChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class NServiceBusHost : ISupportInitialize
	{
		static NServiceBusHost()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NServiceBusHost)), typeof(NServiceBusHost));
			NServiceBusHost.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(INServiceBusHost).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(INServiceBusHost).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler CommandSenderNeedsRegistrationChanged = (sender, args) => { };
		public event EventHandler ComponentsOrderDefinitionChanged = (sender, args) => { };
		public event EventHandler ErrorQueueChanged = (sender, args) => { };
		public event EventHandler ForwardReceivedMessagesToChanged = (sender, args) => { };
		public event EventHandler MasterNodeChanged = (sender, args) => { };
		public event EventHandler MaxRetriesChanged = (sender, args) => { };
		public event EventHandler MessageConventionsChanged = (sender, args) => { };
		public event EventHandler MessageEndpointMappingsConfigChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };
		public event EventHandler NumberOfWorkerThreadsChanged = (sender, args) => { };
		public event EventHandler SLAChanged = (sender, args) => { };
		public event EventHandler SLEnabledChanged = (sender, args) => { };
		public event EventHandler SLNumberOfRetriesChanged = (sender, args) => { };
		public event EventHandler SLTimeIncreaseChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "CommandSenderNeedsRegistration":
					CommandSenderNeedsRegistrationChanged(sender, args);
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
				case "MasterNode":
					MasterNodeChanged(sender, args);
					break;
				case "MaxRetries":
					MaxRetriesChanged(sender, args);
					break;
				case "MessageConventions":
					MessageConventionsChanged(sender, args);
					break;
				case "MessageEndpointMappingsConfig":
					MessageEndpointMappingsConfigChanged(sender, args);
					break;
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "NumberOfWorkerThreads":
					NumberOfWorkerThreadsChanged(sender, args);
					break;
				case "SLA":
					SLAChanged(sender, args);
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
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class NServiceBusHostComponentLink : ISupportInitialize
	{
		static NServiceBusHostComponentLink()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NServiceBusHostComponentLink)), typeof(NServiceBusHostComponentLink));
			NServiceBusHostComponentLink.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(INServiceBusHostComponentLink).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(INServiceBusHostComponentLink).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

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
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
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
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class NServiceBusWeb : ISupportInitialize
	{
		static NServiceBusWeb()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NServiceBusWeb)), typeof(NServiceBusWeb));
			NServiceBusWeb.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(INServiceBusWeb).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(INServiceBusWeb).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler CommandSenderNeedsRegistrationChanged = (sender, args) => { };
		public event EventHandler ErrorQueueChanged = (sender, args) => { };
		public event EventHandler ForwardReceivedMessagesToChanged = (sender, args) => { };
		public event EventHandler HasComponentsThatPublishEventChanged = (sender, args) => { };
		public event EventHandler MaxRetriesChanged = (sender, args) => { };
		public event EventHandler MessageConventionsChanged = (sender, args) => { };
		public event EventHandler MessageEndpointMappingsConfigChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };
		public event EventHandler NumberOfWorkerThreadsChanged = (sender, args) => { };
		public event EventHandler SenderBaseTypeChanged = (sender, args) => { };
		public event EventHandler SLAChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "CommandSenderNeedsRegistration":
					CommandSenderNeedsRegistrationChanged(sender, args);
					break;
				case "ErrorQueue":
					ErrorQueueChanged(sender, args);
					break;
				case "ForwardReceivedMessagesTo":
					ForwardReceivedMessagesToChanged(sender, args);
					break;
				case "HasComponentsThatPublishEvent":
					HasComponentsThatPublishEventChanged(sender, args);
					break;
				case "MaxRetries":
					MaxRetriesChanged(sender, args);
					break;
				case "MessageConventions":
					MessageConventionsChanged(sender, args);
					break;
				case "MessageEndpointMappingsConfig":
					MessageEndpointMappingsConfigChanged(sender, args);
					break;
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "NumberOfWorkerThreads":
					NumberOfWorkerThreadsChanged(sender, args);
					break;
				case "SenderBaseType":
					SenderBaseTypeChanged(sender, args);
					break;
				case "SLA":
					SLAChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class NServiceBusWebComponentLink : ISupportInitialize
	{
		static NServiceBusWebComponentLink()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NServiceBusWebComponentLink)), typeof(NServiceBusWebComponentLink));
			NServiceBusWebComponentLink.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(INServiceBusWebComponentLink).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(INServiceBusWebComponentLink).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

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
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
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
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class NServiceBusMVC : ISupportInitialize
	{
		static NServiceBusMVC()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NServiceBusMVC)), typeof(NServiceBusMVC));
			NServiceBusMVC.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(INServiceBusMVC).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(INServiceBusMVC).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler CommandSenderNeedsRegistrationChanged = (sender, args) => { };
		public event EventHandler ErrorQueueChanged = (sender, args) => { };
		public event EventHandler ForwardReceivedMessagesToChanged = (sender, args) => { };
		public event EventHandler HasComponentsThatPublishEventChanged = (sender, args) => { };
		public event EventHandler MaxRetriesChanged = (sender, args) => { };
		public event EventHandler MessageConventionsChanged = (sender, args) => { };
		public event EventHandler MessageEndpointMappingsConfigChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };
		public event EventHandler NumberOfWorkerThreadsChanged = (sender, args) => { };
		public event EventHandler SLAChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "CommandSenderNeedsRegistration":
					CommandSenderNeedsRegistrationChanged(sender, args);
					break;
				case "ErrorQueue":
					ErrorQueueChanged(sender, args);
					break;
				case "ForwardReceivedMessagesTo":
					ForwardReceivedMessagesToChanged(sender, args);
					break;
				case "HasComponentsThatPublishEvent":
					HasComponentsThatPublishEventChanged(sender, args);
					break;
				case "MaxRetries":
					MaxRetriesChanged(sender, args);
					break;
				case "MessageConventions":
					MessageConventionsChanged(sender, args);
					break;
				case "MessageEndpointMappingsConfig":
					MessageEndpointMappingsConfigChanged(sender, args);
					break;
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "NumberOfWorkerThreads":
					NumberOfWorkerThreadsChanged(sender, args);
					break;
				case "SLA":
					SLAChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class NServiceBusMVCComponentLink : ISupportInitialize
	{
		static NServiceBusMVCComponentLink()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NServiceBusMVCComponentLink)), typeof(NServiceBusMVCComponentLink));
			NServiceBusMVCComponentLink.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(INServiceBusMVCComponentLink).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(INServiceBusMVCComponentLink).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

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
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
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
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class ContractsProject : ISupportInitialize
	{
		static ContractsProject()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ContractsProject)), typeof(ContractsProject));
			ContractsProject.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IContractsProject).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IContractsProject).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class InternalMessagesProject : ISupportInitialize
	{
		static InternalMessagesProject()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(InternalMessagesProject)), typeof(InternalMessagesProject));
			InternalMessagesProject.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IInternalMessagesProject).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IInternalMessagesProject).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Authentication : ISupportInitialize
	{
		static Authentication()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Authentication)), typeof(Authentication));
			Authentication.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IAuthentication).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IAuthentication).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };
		public event EventHandler CodePathChanged = (sender, args) => { };
		public event EventHandler CustomCodePathChanged = (sender, args) => { };
		public event EventHandler LocalNamespaceChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "CodePath":
					CodePathChanged(sender, args);
					break;
				case "CustomCodePath":
					CustomCodePathChanged(sender, args);
					break;
				case "LocalNamespace":
					LocalNamespaceChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class UseCase : ISupportInitialize
	{
		static UseCase()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UseCase)), typeof(UseCase));
			UseCase.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IUseCase).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IUseCase).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class UseCaseStep : ISupportInitialize
	{
		static UseCaseStep()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UseCaseStep)), typeof(UseCaseStep));
			UseCaseStep.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IUseCaseStep).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IUseCaseStep).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler PatternValueChanged = (sender, args) => { };
		public event EventHandler SourceEndpointNameChanged = (sender, args) => { };
		public event EventHandler TargetEndpointNameChanged = (sender, args) => { };
		public event EventHandler ComponentNameChanged = (sender, args) => { };
		public event EventHandler EventNameChanged = (sender, args) => { };
		public event EventHandler CommandNameChanged = (sender, args) => { };
		public event EventHandler ServiceNameValueChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "PatternValue":
					PatternValueChanged(sender, args);
					break;
				case "SourceEndpointName":
					SourceEndpointNameChanged(sender, args);
					break;
				case "TargetEndpointName":
					TargetEndpointNameChanged(sender, args);
					break;
				case "ComponentName":
					ComponentNameChanged(sender, args);
					break;
				case "EventName":
					EventNameChanged(sender, args);
					break;
				case "CommandName":
					CommandNameChanged(sender, args);
					break;
				case "ServiceNameValue":
					ServiceNameValueChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class UseCaseLink : ISupportInitialize
	{
		static UseCaseLink()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UseCaseLink)), typeof(UseCaseLink));
			UseCaseLink.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IUseCaseLink).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IUseCaseLink).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler ElementTypeChanged = (sender, args) => { };
		public event EventHandler LinkedElementIdChanged = (sender, args) => { };
		public event EventHandler StartsUseCaseChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "ElementType":
					ElementTypeChanged(sender, args);
					break;
				case "LinkedElementId":
					LinkedElementIdChanged(sender, args);
					break;
				case "StartsUseCase":
					StartsUseCaseChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Library : ISupportInitialize
	{
		static Library()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Library)), typeof(Library));
			Library.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsElement();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(ILibrary).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(ILibrary).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler FilePathChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsElement().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "FilePath":
					FilePathChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsElement().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Services : ISupportInitialize
	{
		static Services()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Services)), typeof(Services));
			Services.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IServices).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IServices).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Contract : ISupportInitialize
	{
		static Contract()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Contract)), typeof(Contract));
			Contract.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IContract).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IContract).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Events : ISupportInitialize
	{
		static Events()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Events)), typeof(Events));
			Events.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IEvents).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IEvents).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Commands : ISupportInitialize
	{
		static Commands()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Commands)), typeof(Commands));
			Commands.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(ICommands).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(ICommands).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Components : ISupportInitialize
	{
		static Components()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Components)), typeof(Components));
			Components.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IComponents).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IComponents).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Publishes : ISupportInitialize
	{
		static Publishes()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Publishes)), typeof(Publishes));
			Publishes.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IPublishes).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IPublishes).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Subscribes : ISupportInitialize
	{
		static Subscribes()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Subscribes)), typeof(Subscribes));
			Subscribes.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(ISubscribes).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(ISubscribes).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class LibraryReferences : ISupportInitialize
	{
		static LibraryReferences()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(LibraryReferences)), typeof(LibraryReferences));
			LibraryReferences.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(ILibraryReferences).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(ILibraryReferences).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class ServiceLibraries : ISupportInitialize
	{
		static ServiceLibraries()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ServiceLibraries)), typeof(ServiceLibraries));
			ServiceLibraries.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IServiceLibraries).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IServiceLibraries).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Endpoints : ISupportInitialize
	{
		static Endpoints()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Endpoints)), typeof(Endpoints));
			Endpoints.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IEndpoints).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IEndpoints).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class NServiceBusHostComponents : ISupportInitialize
	{
		static NServiceBusHostComponents()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NServiceBusHostComponents)), typeof(NServiceBusHostComponents));
			NServiceBusHostComponents.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(INServiceBusHostComponents).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(INServiceBusHostComponents).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class NServiceBusWebComponents : ISupportInitialize
	{
		static NServiceBusWebComponents()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NServiceBusWebComponents)), typeof(NServiceBusWebComponents));
			NServiceBusWebComponents.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(INServiceBusWebComponents).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(INServiceBusWebComponents).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class NServiceBusMVCComponents : ISupportInitialize
	{
		static NServiceBusMVCComponents()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NServiceBusMVCComponents)), typeof(NServiceBusMVCComponents));
			NServiceBusMVCComponents.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(INServiceBusMVCComponents).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(INServiceBusMVCComponents).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Infrastructure : ISupportInitialize
	{
		static Infrastructure()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Infrastructure)), typeof(Infrastructure));
			Infrastructure.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IInfrastructure).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IInfrastructure).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Security : ISupportInitialize
	{
		static Security()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Security)), typeof(Security));
			Security.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(ISecurity).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(ISecurity).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class DummyCollection : ISupportInitialize
	{
		static DummyCollection()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(DummyCollection)), typeof(DummyCollection));
			DummyCollection.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IDummyCollection).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IDummyCollection).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class UseCases : ISupportInitialize
	{
		static UseCases()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UseCases)), typeof(UseCases));
			UseCases.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(IUseCases).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(IUseCases).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	partial class Libraries : ISupportInitialize
	{
		static Libraries()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Libraries)), typeof(Libraries));
			Libraries.StaticInitialization();
		}

		static partial void StaticInitialization();

		#region Initialization

		public event EventHandler Initialized = (sender, args) => { };

		public bool IsInitialized { get; private set; }
		
		[Import(AllowDefault = true)]
		public IFeatureCompositionService CompositionService { get; set; }

		[Import]
        public IFxrUriReferenceService UriService { get; set; }

        [Import]
        public RefactoringManager RefactoringManager { get; set; }

		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
			if (this.CompositionService != null)
			{
				var element = this.AsCollection();
				var automations = this.CompositionService
//					.GetExports<IAutomationExtension, IFeatureComponentMetadata>(typeof(ILibraries).FullName)
					.GetExports<IAutomationExtension, IDictionary<string, object>>()
					.FromFeaturesCatalog()
					.Where(x => x.Metadata["ContractName"] == typeof(ILibraries).FullName)
					.Select(x => x.Value);

				foreach (var automation in automations)
				{
					// This already calls BeginInit/EndInit and SatisfyImports as needed
					// on the base ProductElement class in the runtime.
					element.AddAutomationExtension(automation);
				}
			}

			this.OriginalInstanceName = this.InstanceName;

			Initialize();
			if (ElementInitialized != null)
			{
				ElementInitialized(this, EventArgs.Empty);
			}
			this.IsInitialized = true;
			this.Initialized(this, EventArgs.Empty);
		}

		/// <summary>
		/// Optional partial method that is invoked when EndInit is called 
		/// on the element.
		/// </summary>
		partial void Initialize();

		public static event EventHandler ElementInitialized;

		#endregion

		public string OriginalInstanceName { get; set; }

		public event EventHandler InstanceNameChanged = (sender, args) => { };
		public event EventHandler NamespaceChanged = (sender, args) => { };

		public string CodeIdentifier
		{
			get { return System.Xml.Serialization.CodeIdentifier.MakeValid(this.target.InstanceName); }
		}

		partial void OnCreated()
		{
			this.AsCollection().PropertyChanged += OnPropertyChanged;
			Create();
		}

		partial void Create();

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Application.ResetIsDirtyFlag();
			switch (args.PropertyName)
			{
				case "Namespace":
					NamespaceChanged(sender, args);
					break;
				case "InstanceName":
					if (this.OriginalInstanceName != null) {
						if (this.InstanceName != this.OriginalInstanceName && 
							this.AsCollection().RenameElement(this, this.UriService, this.RefactoringManager)) 
						{
							InstanceNameChanged(sender, args);
							this.OriginalInstanceName = this.InstanceName;
						}
						else
						{
							this.InstanceName = this.OriginalInstanceName;
						}
					}
					break;					
				default:
					break;
			}
		}
	}

	#endregion Implementations
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.Patterning.Runtime;
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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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

	partial class ContractsProject : ISupportInitialize
	{
		static ContractsProject()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ContractsProject)), typeof(ContractsProject));
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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
		}

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

	partial class Infrastructure : ISupportInitialize
	{
		static Infrastructure()
		{
			TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Infrastructure)), typeof(Infrastructure));
		}

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
		}

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
		}

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
		}

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
		}

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Core;
using NServiceBusStudio.Core.Design;
using NServiceBusStudio;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using NuPattern.ComponentModel;
using NuPattern.VisualStudio.Solution;


namespace AbstractEndpoint
{
    using ICommand = NServiceBusStudio.ICommand;

    public class ComponentReferenceConverter : ElementReferenceConverter<IAbstractComponentLink, IComponent, ComponentReferenceStrategy> { }

    public class ComponentReferenceEditor : ElementReferenceEditor<IAbstractComponentLink, IComponent, ComponentReferenceStrategy> { }

    public class ComponentReferenceStrategy : IElementReferenceStrategy<IAbstractComponentLink, IComponent>
    {
        public IElementReference<IComponent> GetReference(IAbstractComponentLink owner)
        {
            return owner.ComponentReference;
        }

        public IEnumerable<StandardValue> GetStandardValues(IAbstractComponentLink owner)
        {
            var components = owner.As<IProductElement>().Root.As<IApplication>().Design.Services.Service.SelectMany(s => s.Components.Component);

            return components
                .Except(owner.Siblings.Select(link => link.ComponentReference.Value)
                    .Except(new[] { owner.ComponentReference.Value }))
                .Select(component => new StandardValue(component.InstanceName, component, "", component.Parent.Parent.InstanceName));
        }

        public IComponent NullValue
        {
            get { return NullComponent.Instance; }
        }

        /// <summary>
        /// Implements the null pattern for components. 
        /// </summary>
        /// <devdoc>
        /// Need this to workaround the behavior of the FB StandardValuesEditor 
        /// that considers a null value as equal to selecting the original 
        /// value.
        /// </devdoc>
        private class NullComponent : IComponent
        {
            static NullComponent()
            {
                Instance = new NullComponent();
            }

            private NullComponent()
            {
            }

            public static IComponent Instance { get; private set; }

            public string MessageName { get; set; }

            public string MessageId { get; set; }

            public string InstanceName { get; set; }

            public IEnumerable<IReference> References { get; private set; }

            public string Notes { get; set; }

            public bool InTransaction { get; private set; }

            public bool IsSerializing { get; private set; }

            public IComponents Parent { get; private set; }

            public void Delete()
            {
            }

            public IElement AsElement()
            {
                return null;
            }

            public string CodeIdentifier { get; private set; }

            public event EventHandler MessageNameChanged;

            public event EventHandler MessageIdChanged;

            public TRuntimeInterface As<TRuntimeInterface>() where TRuntimeInterface : class
            {
                throw new NotImplementedException();
            }

            public event EventHandler InstanceNameChanged;


            public IPublishes Publishes { get; private set; }

            public IPublishes CreatePublishes(string name, Action<IPublishes> initializer = null, bool raiseInstantiateEvents = true)
            {
                throw new NotImplementedException();
            }

            public ISubscribes Subscribes
            {
                get { throw new NotImplementedException(); }
            }

            public ISubscribes CreateSubscribes(string name, Action<ISubscribes> initializer = null, bool raiseInstantiateEvents = true)
            {
                throw new NotImplementedException();
            }

            public string Namespace
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public event EventHandler NamespaceChanged;

            public IProject Project
            {
                get { throw new NotImplementedException(); }
            }


            public double InstanceOrder
            {
                get
                {
                    return 0;
                }
                set
                {
                }
            }

            public void EndpointDefined(IAbstractEndpoint endpoint)
            {
            }


            public string AdditionalUsings { get; set; }
            public string Inherits { get; set; }
            public string ClassBody { get; set; }
            public string CustomClassBody { get; set; }
            public bool IsSaga { get; set; }


            public event EventHandler AdditionalUsingsChanged;

            public event EventHandler InheritsChanged;

            public event EventHandler ClassBodyChanged;

            public event EventHandler CustomClassBodyChanged;

            public event EventHandler IsSagaChanged;

            public string BaseType { get;set;}

            public IEnumerable<IAbstractEndpoint> DeployedTo { get;set;}


            public void DeployTo(IAbstractEndpoint endpoint)
            {
            }

            public void Publish(IEvent @event)
            {
            }

            public void Subscribe(ICommand command)
            {
            }


            public ILibraryReferences LibraryReferences
            {
                get { throw new NotImplementedException(); }
            }

            public ILibraryReferences CreateLibraryReferences(string name, Action<ILibraryReferences> initializer = null, bool raiseInstantiateEvents = true)
            {
                throw new NotImplementedException();
            }


            public void RemoveLinks(IAbstractEndpoint endpoint)
            {
                throw new NotImplementedException();
            }

            public void AddLinks(IAbstractEndpoint endpoint)
            {
                throw new NotImplementedException();
            }


            public event EventHandler AutoPublishEventsChanged;


            public bool IsSender
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsProcessor
            {
                get { throw new NotImplementedException(); }
            }



            public string InterfaceBody
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }


            public void AddSagaLinks(IAbstractEndpoint endpoint)
            {
                throw new NotImplementedException();
            }

            public void RemoveSagaLinks(IAbstractEndpoint endpoint)
            {
                throw new NotImplementedException();
            }


            public string ConfigureHowToFindSagaBody
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }


            public event EventHandler InterfaceBodyChanged;

            public event EventHandler ConfigureHowToFindSagaBodyChanged;


            public bool UnfoldedCustomCode
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }


            public event EventHandler UnfoldedCustomCodeChanged;
        }
    }
}

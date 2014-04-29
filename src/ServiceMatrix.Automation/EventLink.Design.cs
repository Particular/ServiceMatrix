using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Core.Design;
using NServiceBusStudio.Core;
using NuPattern.ComponentModel;


namespace NServiceBusStudio
{
    using NuPattern.Runtime;

    public class EventReferenceConverter : ElementReferenceConverter<IEventLink, IEvent, EventReferenceStrategy> { }

	public class EventReferenceEditor : ElementReferenceEditor<IEventLink, IEvent, EventReferenceStrategy> { }

	public class EventReferenceStrategy : IElementReferenceStrategy<IEventLink, IEvent>
	{
		public IElementReference<IEvent> GetReference(IEventLink owner)
		{
			return owner.EventReference;
		}

		public IEnumerable<StandardValue> GetStandardValues(IEventLink owner)
		{
			var thisService = owner.Parent.Parent;

			return owner.Parent.Parent.Parent.Parent.Contract.Events.Event
				.Except(owner.Parent.EventLinks.Select(link => link.EventReference.Value)
					.Except(new [] { owner.EventReference.Value }))
				.Select(e => new StandardValue(e.InstanceName, e));
		}

		public IEvent NullValue
		{
			get { return NullEvent.Instance; }
		}

		/// <summary>
		/// Implements the null pattern for components. 
		/// </summary>
		/// <devdoc>
		/// Need this to workaround the behavior of the FB StandardValuesEditor 
		/// that considers a null value as equal to selecting the original 
		/// value.
		/// </devdoc>
		private class NullEvent : IEvent
		{
			static NullEvent()
			{
				Instance = new NullEvent();
			}

			private NullEvent()
			{
			}

			public static IEvent Instance { get; private set; }

			// ------------------------------------------------- // 

			public string InstanceName { get; set; }

			public IEnumerable<IReference> References { get; private set; }

			public string Notes { get; set; }

			public bool InTransaction { get; private set; }

			public bool IsSerializing { get; private set; }

            public IEvents Parent
            {
                get { throw new NotImplementedException(); }
            }

			public void Delete()
			{
				throw new NotImplementedException();
			}

			public IElement AsElement()
			{
				throw new NotImplementedException();
			}

			public string CodeIdentifier { get; private set; }

			public TRuntimeInterface As<TRuntimeInterface>() where TRuntimeInterface : class
			{
				throw new NotImplementedException();
			}

			public event EventHandler InstanceNameChanged;


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

            public bool DoNotAutogenerateComponents { get; set; }


            public event EventHandler DoNotAutogenerateComponentsChanged;
        }
	}

}

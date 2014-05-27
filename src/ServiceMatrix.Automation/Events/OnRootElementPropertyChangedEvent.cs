using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using NuPattern;
using NuPattern.Runtime;
using NuPattern.Runtime.Events;

namespace NServiceBusStudio.Automation.Events
{
    public interface IOnRootElementPropertyChangedEvent : IObservable<IEvent<PropertyChangedEventArgs>>, IObservableEvent
    {
    }

    [DisplayName("Root element property changed")]
    [Description(@"OnElementPropertyChangedEvent_Description")]
    [Category(@"Automation")]
    [Event(typeof(IOnRootElementPropertyChangedEvent))]
    [Export(typeof(IOnRootElementPropertyChangedEvent))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class OnRootElementPropertyChangedEvent : IOnRootElementPropertyChangedEvent, IDisposable
    {
        /// <summary>
        /// The root element.
        /// </summary>
        private readonly IProductElement rootProductElement;

        /// <summary>
        /// The event that is the source to which subscribers subscribe. 
        /// In this case, it's our own event that we republish from 
        /// the underlying event.
        /// </summary>
        private readonly IObservable<IEvent<PropertyChangedEventArgs>> localSourceEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnRootElementPropertyChangedEvent"/> class.
        /// </summary>
        [ImportingConstructor]
        public OnRootElementPropertyChangedEvent([Import(AllowDefault = true)] IProductElement productElement)
        {
            if (productElement != null && productElement.Root != null)
            {
                rootProductElement = productElement.Root;
                rootProductElement.PropertyChanged += OnPropertyChanged;
            }

            localSourceEvent = WeakObservable.FromEvent<PropertyChangedEventArgs>(
                handler => ElementPropertyChanged += handler,
                handler => ElementPropertyChanged -= handler);
        }

        /// <summary>
        /// Private event used to re-publish the state event.
        /// </summary>
        private event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged = (sender, args) => { };

        /// <summary>
        /// Subscribes the specified observer.
        /// </summary>
        public IDisposable Subscribe(IObserver<IEvent<PropertyChangedEventArgs>> observer)
        {
            Guard.NotNull(() => observer, observer);

            return localSourceEvent.Subscribe(observer);
        }

        /// <summary>
        /// Cleans up subscriptions.
        /// </summary>
        public void Dispose()
        {
            if (rootProductElement != null)
            {
                rootProductElement.PropertyChanged -= OnPropertyChanged;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ElementPropertyChanged(sender, e);
        }
    }
}
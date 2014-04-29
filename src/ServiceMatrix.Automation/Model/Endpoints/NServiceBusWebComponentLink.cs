namespace NServiceBusStudio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AbstractEndpoint;
    using NServiceBusStudio.Core;
    using NuPattern.Runtime;

    partial interface INServiceBusWebComponentLink : IAbstractComponentLink
    {
    }

    partial class NServiceBusWebComponentLink
    {
        public IEnumerable<IAbstractComponentLink> Siblings
        {
            get
            {
                return Parent.NServiceBusWebComponentLinks;
            }
        }

        private ElementReference<IComponent> componentReference;

        public IElementReference<IComponent> ComponentReference
        {
            get
            {
                var components = As<IProductElement>().Root.As<IApplication>().Design.Services.Service.SelectMany(s => s.Components.Component);

                return componentReference ??
                    (componentReference = new ElementReference<IComponent>(
                        () => components,
                        new PropertyReference<string>(() => ComponentId, value => ComponentId = value),
                        new PropertyReference<string>(() => ComponentName, value => ComponentName = value)));
            }
        }

        partial void Initialize()
        {
            Action<object, EventArgs> nameChange = (sender, args) => InstanceName = ComponentReference.Value == null ? "(None)" : string.Format("{0:D2}. {1}.{2}", Order, ComponentReference.Value.Parent.Parent.InstanceName, ComponentReference.Value.InstanceName);

            ComponentIdChanged += new EventHandler(nameChange);
            ComponentNameChanged += new EventHandler(nameChange);
            OrderChanged += (sender, args) => { reorderNext(this); nameChange(sender, args); };
            if (ComponentReference.Value == null)
                InstanceName = "(None)";
            else
                nameChange(null, null);
        }

        public void SetNextOrderNumber()
        {
            var allOrders = Parent.NServiceBusWebComponentLinks.Select(cl => cl.Order);

            Order = allOrders.Where(p => allOrders.All(s => s != (p + 1))).Min() + 1;
        }

        private void reorderNext(INServiceBusWebComponentLink componentLink)
        {
            var next = componentLink.Parent.NServiceBusWebComponentLinks.FirstOrDefault(s => s.Order == componentLink.Order && s != componentLink);
            if (next != null)
            {
                next.Order++;
            }
        }

        public IAbstractEndpointComponents ParentEndpointComponents
        {
            get { return Parent as IAbstractEndpointComponents; }
        }
    }
}

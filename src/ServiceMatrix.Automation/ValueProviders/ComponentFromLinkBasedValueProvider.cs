namespace NServiceBusStudio.Automation.ValueProviders
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using NServiceBusStudio.Automation.Model;

    public abstract class ComponentFromLinkBasedValueProvider : ValueProvider
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }

        IComponent component;
        IService service;

        protected IComponent Component
        {
            get
            {
                if (component == null)
                {
                    ProvideValues();
                }
                return component;
            }
        }

        protected IService Service
        {
            get
            {
                if (service == null)
                {
                    ProvideValues();
                }
                return service;
            }
        }

        private void ProvideValues()
        {
            component = Helpers.GetComponentFromLinkedElement(CurrentElement);
            service = component.Parent.Parent;
        }

    }
}

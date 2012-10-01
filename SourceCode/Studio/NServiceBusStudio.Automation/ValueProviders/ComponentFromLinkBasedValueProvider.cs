using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace NServiceBusStudio.Automation.ValueProviders
{
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
                if (this.component == null)
                {
                    this.ProvideValues();
                }
                return this.component;
            }
        }

        protected IService Service
        {
            get
            {
                if (this.service == null)
                {
                    this.ProvideValues();
                }
                return this.service;
            }
        }

        private void ProvideValues()
        {
            this.component = Model.Helpers.GetComponentFromLinkedElement(this.CurrentElement);
            this.service = component.Parent.Parent;
        }

    }
}

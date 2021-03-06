﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio.Automation.Conditions
{
    [CLSCompliant(false)]
    [DisplayName("Component Has Endpoint Defined")]
    [Category("General")]
    [Description("True if the component is already referenced from an endpoint.")]
    public class HasEndpointDefinedCondition : Condition
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

        public override bool Evaluate()
        {
            var component = Model.Helpers.GetComponentFromLinkedElement(this.CurrentElement);

            if (component != null)
            {
                var service = component.Parent.Parent;

                var endpoints = service.Parent.Parent.Endpoints.GetAll()
                    .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component));

                return (endpoints.Any());
            }
            else
            {
                return true;
            }
        }
    }
}

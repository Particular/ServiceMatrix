using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Patterning.Runtime;
using AbstractEndpoint;

namespace NServiceBusStudio.Automation.ValueProviders
{
    [CLSCompliant(false)]
    [DisplayName("ComponentFuncFromEndpointValueProvider")]
    [Category("General")]
    [Description("Evaluates a Func on the endpoint passing the current component")]
    public class ComponentFuncFromEndpointValueProvider : ComponentFromLinkBasedValueProvider
    {
        public string FuncName { get; set; }

        public override object Evaluate()
        {
            try
            {
                var endpoints = this.Service.Parent.Parent.Endpoints.As<IAbstractElement>().Extensions
                    .Select(e => (e.As<IToolkitInterface>() as IAbstractEndpoint))
                    .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == this.Component));

                return endpoints.Select(endpoint =>
                    {
                        var func = typeof(EndpointCustomizationFuncs).GetProperty(this.FuncName).GetValue(endpoint.CustomizationFuncs(), null) as Func<IComponent, string>;

                        if (func != null)
                        {
                            return func(this.Component);
                        }
                        return string.Empty;
                    })
                    .Where(s => s != string.Empty)
                    .FirstOrDefault();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

namespace NServiceBusStudio.Automation.ValueProviders
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using AbstractEndpoint;

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
                var endpoints = Service.Parent.Parent.Endpoints.GetAll()
                    .Where(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == Component));

                return endpoints.Select(endpoint =>
                    {
                        var func = typeof(EndpointCustomizationFuncs).GetProperty(FuncName).GetValue(endpoint.CustomizationFuncs(), null) as Func<NServiceBusStudio.IComponent, string>;

                        if (func != null)
                        {
                            return func(Component);
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

namespace NServiceBusStudio.Automation.ValueProviders
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using NuPattern.Runtime;

    [CLSCompliant(false)]
    [DisplayName("GetScriptsPathInMvcEndpoint")]
    [Category("General")]
    [Description("Get the location for the Scripts folder")]
    public class GetScriptsPathInMvcEndpoint : ValueProvider
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override object Evaluate()
        {
            try
            {
                var app = ((IProductElement)this.CurrentElement).Root.As<IApplication>();
                var mvcEndpoint = NServiceBusStudio.Automation.Model.Helpers.GetMvcEndpointFromLinkedElement(CurrentElement).As<IProductElement>();

                if (mvcEndpoint != null)
                {
                    var path = String.Format("{0}.{1}", app.InstanceName, mvcEndpoint.InstanceName);
                    path += "\\Scripts";
                    return path;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}

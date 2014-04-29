namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    using System;
    using System.ComponentModel;
    using NuPattern.Runtime;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using abs = AbstractEndpoint;
    using NuPattern.Runtime.ToolkitInterface;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Raise On Instantiated")]
    [Description("Raise On Instantiated")]
    [CLSCompliant(false)]
    public class RaiseOnInstantiated : Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentItem { get; set; }

        public override void Execute()
        {
            abs.AbstractEndpointExtensions.RaiseOnInstantiated(CurrentItem.As<IToolkitInterface>() as abs.IAbstractEndpoint);
        }
    }
}

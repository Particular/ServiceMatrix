namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Composition;
    using NuPattern.Runtime;
    using NuPattern.Runtime.ToolkitInterface;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("ExecuteUnfoldEndpointDiagram")]
    [Description("ExecuteUnfoldEndpointDiagram")]
    [CLSCompliant(false)]
    public class ExecuteUnfoldEndpointDiagram : Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            var endpoint = CurrentElement.Parent.Parent.Parent;
            var product = endpoint.As<IToolkitInterface>().As<IProductElement>();
            var automation = product.AutomationExtensions.FirstOrDefault(x => x.Name == "UnfoldDiagramFile");
            if (automation != null)
            {
                automation.Execute();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    [DisplayName("ExecuteUnfoldEndpointDiagram")]
    [Description("ExecuteUnfoldEndpointDiagram")]
    [CLSCompliant(false)]
    public class ExecuteUnfoldEndpointDiagram : FeatureCommand
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        public override void Execute()
        {
            var endpoint = this.CurrentElement.Parent.Parent.Parent;
            var product = endpoint.As<IToolkitInterface>().As<IProductElement>();
            var automation = product.AutomationExtensions.FirstOrDefault(x => x.Name == "UnfoldDiagramFile");
            if (automation != null)
            {
                automation.Execute();
            }
        }
    }
}

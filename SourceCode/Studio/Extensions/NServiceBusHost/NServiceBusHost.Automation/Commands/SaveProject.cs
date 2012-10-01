using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using AbstractEndpoint;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using NServiceBusStudio.Automation.Extensions;

namespace NServiceBusHost.Automation.Commands
{
    [DisplayName("Save associated Project")]
    [Description("Saves the project")]
    [CLSCompliant(false)]
    public class SaveProject : FeatureCommand
    {
        [Required]
        [Import(AllowDefault = true)]
        public IAbstractEndpoint Endpoint { get; set; }

        public override void Execute()
        {
            var prj = this.Endpoint.Project.As<EnvDTE.Project>();
            prj.Save();
        }
    }
}

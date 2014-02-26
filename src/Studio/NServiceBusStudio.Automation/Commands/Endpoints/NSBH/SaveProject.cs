using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using AbstractEndpoint;
using NServiceBusStudio.Automation.Extensions;

namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    [DisplayName("Save associated Project")]
    [Description("Saves the project")]
    [CLSCompliant(false)]
    public class SaveProject : NuPattern.Runtime.Command
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

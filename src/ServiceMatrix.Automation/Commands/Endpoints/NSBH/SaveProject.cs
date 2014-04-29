namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBH
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using AbstractEndpoint;
    using EnvDTE;
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Save associated Project")]
    [Description("Saves the project")]
    [CLSCompliant(false)]
    public class SaveProject : Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IAbstractEndpoint Endpoint { get; set; }

        public override void Execute()
        {
            var prj = Endpoint.Project.As<Project>();
            prj.Save();
        }
    }
}

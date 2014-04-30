namespace NServiceBusStudio.Automation.Commands
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using AbstractEndpoint;
    using NuPattern.Runtime;
    using System.Linq;
    using System.IO;
    using NuPattern.VisualStudio.Solution;
    using EnvDTE;
    using Command = NuPattern.Runtime.Command;

    [Category("General")]
    [DisplayName("Set StartUp Projects")]
    [Description("Set Endpoint projects as StartUp Projects")]
    [CLSCompliant(false)]
    public class SetStartUpProjects : Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }

        [Import(AllowDefault = true)]
        public IPatternManager ProductManager { get; set; }

        [Import(AllowDefault = true)]
        public ISolution Solution { get; set; }

        public override void Execute()
        {
            var app = ProductManager.Products.First().As<IApplication>();
            app.CheckForFirstBuild();
            var endpoints = app.Design.Endpoints.GetAll();
            var abstractEndpoints = endpoints as IAbstractEndpoint[] ?? endpoints.ToArray();
            var arrayStartUpProjects = new object[abstractEndpoints.Count()];

            var solutionFolder = new Uri(Solution.PhysicalPath);

            for (var i = 0; i < abstractEndpoints.Count(); i++)
            {
                var abstractEndpoint = abstractEndpoints.ElementAt(i);
                if (abstractEndpoint.Project != null)
                {
                    var projectUri = new Uri(abstractEndpoint.Project.PhysicalPath);

                    var relativeUri = solutionFolder.MakeRelativeUri(projectUri);
                    var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

                    arrayStartUpProjects.SetValue(relativePath.Replace('/', Path.DirectorySeparatorChar), i);
                }
            }

            var envDTESolution = Solution.As<Solution>();
            envDTESolution.SolutionBuild.StartupProjects = arrayStartUpProjects;

            // Disable Build Configuration for place holder projects (Libraries and Components)
            foreach (var sc in envDTESolution.SolutionBuild.SolutionConfigurations)
            {
                var solutionConfiguration = sc as SolutionConfiguration;
                foreach (var sctx in solutionConfiguration.SolutionContexts)
                {
                    var context = sctx as SolutionContext;
                    //if (projectNamesToDisable.Contains(context.ProjectName))
                    if (context.ProjectName.EndsWith(String.Format (".{0}.csproj", CurrentElement.Root.As<IApplication>().ProjectNameCode)))
                    {
                        context.ShouldBuild = false;
                        context.ShouldDeploy = false;
                    }
                }
            }
        }

    }
}

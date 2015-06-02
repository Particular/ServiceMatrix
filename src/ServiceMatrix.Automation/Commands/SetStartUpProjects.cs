using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;
using AbstractEndpoint;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio.Automation.Commands
{
    [Category("General")]
    [DisplayName("Set StartUp Projects")]
    [Description("Set Endpoint projects as StartUp Projects")]
    [CLSCompliant(false)]
    public class SetStartUpProjects : NuPattern.Runtime.Command
    {
        public SetStartUpProjects()
        {
        }

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
            try
            {
                var app = this.ProductManager.Products.First().As<IApplication>();
                app.CheckForFirstBuild();
                var endpoints = app.Design.Endpoints.GetAll().ToList();
                var arrayStartUpProjects = new object[endpoints.Count];
                var solutionFolder = new Uri(this.Solution.PhysicalPath);

                for (int i = 0; i < endpoints.Count(); i++)
                {
                    var abstractEndpoint = endpoints.ElementAt(i);
                    if (abstractEndpoint.Project != null)
                    {
                        Uri projectUri = new Uri(abstractEndpoint.Project.PhysicalPath);

                        Uri relativeUri = solutionFolder.MakeRelativeUri(projectUri);
                        String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

                        arrayStartUpProjects[i] = relativePath.Replace('/', Path.DirectorySeparatorChar);
                    }
                }

                var envDTESolution = Solution.As<EnvDTE.Solution>();
                var alreadySetAsAsStartupProjects = envDTESolution.SolutionBuild.StartupProjects as object[] ?? new object[0];
                var startupProjects = arrayStartUpProjects.Concat(alreadySetAsAsStartupProjects).Distinct().ToArray();
                envDTESolution.SolutionBuild.StartupProjects = startupProjects;

                // Disable Build Configuration for place holder projects (Libraries and Components)
                foreach (var sc in envDTESolution.SolutionBuild.SolutionConfigurations)
                {
                    var solutionConfiguration = sc as EnvDTE.SolutionConfiguration;
                    foreach (var sctx in solutionConfiguration.SolutionContexts)
                    {
                        var context = sctx as EnvDTE.SolutionContext;
                        if (context.ProjectName.EndsWith(String.Format(".{0}.csproj", this.CurrentElement.Root.As<IApplication>().ProjectNameCode)))
                        {
                            context.ShouldBuild = false;
                            context.ShouldDeploy = false;
                        }
                    }
                }
            }
            catch
            {
            }
        }

    }
}

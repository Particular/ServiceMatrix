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
                var app = this.ProductManager.Products.First().As<NServiceBusStudio.IApplication>();
                app.CheckForFirstBuild();
                var endpoints = app.Design.Endpoints.GetAll();
                var arrayStartUpProjects = System.Array.CreateInstance(typeof(Object), endpoints.Count());

                var solutionFolder = new Uri(this.Solution.PhysicalPath);

                for (int i = 0; i < endpoints.Count(); i++)
                {
                    var abstractEndpoint = endpoints.ElementAt(i);
                    if (abstractEndpoint.Project != null)
                    {
                        Uri projectUri = new Uri(abstractEndpoint.Project.PhysicalPath);

                        Uri relativeUri = solutionFolder.MakeRelativeUri(projectUri);
                        String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

                        arrayStartUpProjects.SetValue(relativePath.Replace('/', Path.DirectorySeparatorChar), i);
                    }
                }

                var envDTESolution = Solution.As<EnvDTE.Solution>();
                envDTESolution.SolutionBuild.StartupProjects = arrayStartUpProjects;

                // Disable Build Configuration for place holder projects (Libraries and Components)
                var projectNamesToDisable = new string[] { };
                foreach (var sc in envDTESolution.SolutionBuild.SolutionConfigurations)
                {
                    var solutionConfiguration = sc as EnvDTE.SolutionConfiguration;
                    foreach (var sctx in solutionConfiguration.SolutionContexts)
                    {
                        var context = sctx as EnvDTE.SolutionContext;
                        //if (projectNamesToDisable.Contains(context.ProjectName))
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

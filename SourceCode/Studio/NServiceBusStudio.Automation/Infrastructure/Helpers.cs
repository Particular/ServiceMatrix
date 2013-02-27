using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using AbstractEndpoint;
using NuPattern.Library.Commands;
using NServiceBusStudio.Automation.Extensions;
using NuPattern.Library.Automation;

namespace NServiceBusStudio.Automation.Infrastructure
{
    public static class Helpers
    {
        /// <summary>
        /// If the infrastructure project doesn't exist, it creates the project
        /// and adds references to the project on each existing endpoint project.
        /// Also it adds the references when a new endpoint is created
        /// by listening the OnInstantiated event.
        /// </summary>
        /// <param name="infrastructure">Infrastructure</param>
        /// <param name="solution">Solution</param>
        /// <returns>The infrastructure project</returns>
        public static IProject GenerateInfrastructureProjectIfNeeded(IInfrastructure infrastructure, ISolution solution)
        {
            var projectName = infrastructure.Parent.Parent.InstanceName + ".Infrastructure";
            if (!solution.Items.Any(i => i.Name == projectName))
            {
                // Unfold the project
                infrastructure.As<IProductElement>().Execute("GenerateProjectCommand");
                infrastructure.As<IProductElement>().Execute("UnfoldPackagesConfig");

                // Add the references on existing projects
                AddInfrastructureReferences(infrastructure.Parent.Parent, solution);
                infrastructure.Parent.Parent.OnInstantiatedEndpoint += (s, e) =>
                {
                    // Add the references on each new endpoint project
                    AddInfrastructureReferences(infrastructure.Parent.Parent, solution);
                };
            }

            return solution.Items.First(i => i.Name == projectName).As<IProject>();
        }

        // Add a reference to the infrastructure project on each endpoint project
        public static void AddInfrastructureReferences(IApplication app, ISolution solution)
        {
            foreach (var endpoint in app.Design.Endpoints.GetAll())
            {
                endpoint.Project.AddReference(app.Design.Infrastructure.As<IProductElement>().GetProject());
            }
        }

        public static void Execute(this IProductElement product, string automation)
        {
            var command = product.AutomationExtensions.FirstOrDefault(x => x.Name == automation);
            if (command != null)
            {
                command.Execute();
            }
        }

        public static IEnumerable<IAbstractEndpoint> GetAbstractEndpoints(this IApplication app)
        {
            return app.Design.Endpoints.As<IAbstractElement>().Extensions
                .Select(ep => ep.As<IToolkitInterface>() as IAbstractEndpoint);
        }


        public static void SetEndpointsMenuItems(this IApplication app
            , string caption
            , Action<IAbstractEndpoint, IApplication> handler
            , Func<IAbstractEndpoint, IApplication, bool> visibilityEvaluator
            )
        {
            var endpoints = app.Design.Endpoints.GetAll();
            try
            {
                foreach (var endpoint in endpoints)
                {
                    var ep = endpoint.As<IProductElement>();
                    var menuItem = ep.AutomationExtensions.OfType<MenuCommand>().FirstOrDefault(a => a.Text == caption);
                    if (menuItem == null)
                    {
                        menuItem = new MenuCommand(ep
                            , caption
                            , () => handler(endpoint, app));
                        ep.AddAutomationExtension(menuItem);
                    }

                    menuItem.Visible = visibilityEvaluator(endpoint, app);
                }
            }
            // We might not able to get the IToolkitInterface if
            // the package is being initialized
            catch (ArgumentNullException) { }
            catch (NullReferenceException) { }
        }

        public static void SetElementMenuItems(this IApplication app
            , IEnumerable<IProductElement> elements
            , string caption
            , Action<IProductElement, IApplication> handler
            , Func<IProductElement, IApplication, bool> visibilityEvaluator
            )
        {
            try
            {
                foreach (var el in elements)
                {
                    var localElement = el;
                    var menuItem = el.AutomationExtensions.OfType<MenuCommand>().FirstOrDefault(a => a.Text == caption);
                    if (menuItem == null)
                    {
                        menuItem = new MenuCommand(el
                            , caption
                            , () => handler(localElement, app));
                        el.AddAutomationExtension(menuItem);
                    }

                    menuItem.Visible = visibilityEvaluator(el, app);
                }
            }
            // We might not able to get the IToolkitInterface if
            // the package is being initialized
            catch (ArgumentNullException) { }
        }

        public static GenerateProductCodeCommand CreateGenerateCodeCommand(this IProductElement element,
            IServiceProvider sp
            , string targetFileName
            , string targetPath
            , string templateUri
            , string namePrefix = "GenerateCode"
            , string buildAction = "Compile")
        {
            var command = CreateTempGenerateCodeCommand(element, sp, targetFileName, targetPath, templateUri, namePrefix, buildAction);
            var automation = new InnerCommandAutomation(command) { Name = namePrefix + command.Settings.Id.ToString(), Owner = element };
            element.AddAutomationExtension(automation);
            return command;
        }

        public static GenerateProductCodeCommand CreateTempGenerateCodeCommand(this IProductElement element,
            IServiceProvider sp
            , string targetFileName
            , string targetPath
            , string templateUri
            , string namePrefix = "GenerateCode"
            , string buildAction = "Compile")
        {
            var guid = Guid.NewGuid();
            ISolution solution = sp.TryGetService<ISolution>();
            IPatternManager patternManager = sp.TryGetService<IPatternManager>();
            IFxrUriReferenceService uriService = sp.TryGetService<IFxrUriReferenceService>();
            var command = new GenerateProductCodeCommand
            {
                TargetBuildAction = buildAction,
                TargetCopyToOutput = NuPattern.Extensibility.CopyToOutput.DoNotCopy,
                Settings = new EmptySettings { Name = String.Format("{0}{1}", namePrefix, guid.ToString()), Id = guid },
                PatternManager = patternManager,
                UriService = uriService,
                Solution = solution,
                ServiceProvider = sp,
                TargetFileName = targetFileName,
                TargetPath = targetPath,
                CurrentElement = element,
                TemplateUri = new Uri(templateUri)
            };
            return command;
        }
    }
}

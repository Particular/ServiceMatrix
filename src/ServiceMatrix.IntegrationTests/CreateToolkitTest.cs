using System;
using System.Linq;
using EnvDTE80;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using NuPattern.VisualStudio.Solution;

namespace ServiceMatrix.IntegrationTests
{
    /// <summary>
    /// A test class that handles creation of patterns from installed toolkits.
    /// </summary>
    public abstract class CreateToolkitTest : IntegrationTest
    {
        protected string projectName;

        public override void InitializeContext()
        {
            base.InitializeContext();

            // Project name is random, but must start with a letter, and contain no dashes
            projectName = "a" + Guid.NewGuid().ToString("D").Replace("-", string.Empty);
            solution.CreateInstance(DeploymentDirectory, projectName);
        }

        public virtual void Cleanup()
        {
            // Close any opened solutions
            CloseSolution(dte);
            CleanDeploymentDirectory();
        }

        protected void CreatePatternFromProjectTemplate()
        {
            Assert.IsNotNull(TestContext.DataRow, "This test must be configured as a data-driven test");
            Assert.IsNotNull(TestContext.DataRow["TemplateId"], "Ensure the data source has a TemplateId column");
            var templatePath = ((Solution2)dte.Solution).GetProjectTemplate((string)TestContext.DataRow["TemplateId"], "CSharp");
            var template = new VsProjectTemplate(templatePath);
            template.Unfold(projectName, solution);
        }

        protected void CreatePatternFromPatternManager(string patternToolkitId)
        {
            // Create project from Solution Builder
            var toolkit = patternManager.InstalledToolkits.Single(f => f.Id == patternToolkitId);
            UIThreadInvoker.Invoke(new Action(() => patternManager.CreateProduct(toolkit, projectName)));
        }
    }
}

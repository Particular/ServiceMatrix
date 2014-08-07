using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using Moq;
using NServiceBusStudio;
using NuPattern;
using NuPattern.Presentation;
using NuPattern.Runtime;
using NuPattern.VisualStudio.Solution;

namespace ServiceMatrix.IntegrationTests
{
    /// <summary>
    /// A test class that handles creation of patterns from installed toolkits.
    /// </summary>
    public abstract class CreateToolkitTest : IntegrationTest
    {
        protected string projectName;
        protected ISolution solution;
        protected IPatternManager patternManager;
        protected IUriReferenceService uriProvider;
        protected DTE dte;

        public virtual void InitializeContext()
        {
            // Project name is random, but must start with a letter, and contain no dashes
            projectName = "a" + Guid.NewGuid().ToString("D").Replace("-", string.Empty);

            solution = VsIdeTestHostContext.ServiceProvider.GetService<ISolution>();
            dte = VsIdeTestHostContext.ServiceProvider.GetService<DTE>();
            patternManager = VsIdeTestHostContext.ServiceProvider.GetService<IPatternManager>();
            uriProvider = VsIdeTestHostContext.ServiceProvider.GetService<IUriReferenceService>();

            solution.CreateInstance(DeploymentDirectory, projectName);
        }

        public virtual void Cleanup()
        {
            // Close any opened solutions
            VsIdeTestHostContext.Dte.Solution.Close(false);
        }

        protected virtual void CreatePatternFromProjectTemplate()
        {
            Assert.IsNotNull(TestContext.DataRow, "This test must be configured as a data-driven test");
            Assert.IsNotNull(TestContext.DataRow["TemplateId"], "Ensure the data source has a TemplateId column");
            var templatePath = ((Solution2)dte.Solution).GetProjectTemplate((string)TestContext.DataRow["TemplateId"], "CSharp");
            var template = new VsProjectTemplate(templatePath);
            template.Unfold(projectName, solution);
        }

        protected virtual void CreatePatternFromPatternManager(string patternToolkitId)
        {
            // Create project from Solution Builder
            var toolkit = patternManager.InstalledToolkits.Single(f => f.Id == patternToolkitId);
            UIThreadInvoker.Invoke(new Action(() => patternManager.CreateProduct(toolkit, projectName)));
        }

        protected static Task<bool> BuildSolutionAsync(DTE dte)
        {
            return Task.Factory.StartNew(() =>
            {
                var mre = new ManualResetEventSlim();
                var events = dte.Events.BuildEvents;
                _dispBuildEvents_OnBuildDoneEventHandler done = (scope, action) => mre.Set();
                events.OnBuildDone += done;
                try
                {
                    // Let build run async.
                    dte.Solution.SolutionBuild.Build(false);

                    // Wait until it's done.
                    mre.Wait();

                    // LastBuildInfo == # of projects that failed to build.
                    return dte.Solution.SolutionBuild.LastBuildInfo == 0;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    // Cleanup handler.
                    events.OnBuildDone -= done;
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        protected static IReadOnlyCollection<string> GetBuildErrors(DTE dte)
        {
            var errorItems = ((DTE2)dte).ToolWindows.ErrorList.ErrorItems;
            return Enumerable.Range(1, errorItems.Count)
                    .Select(i =>
                    {
                        var errorItem = errorItems.Item(i);
                        return string.Format(
                            CultureInfo.CurrentCulture,
                            "{0}({1},{2}): {3}",
                            errorItem.FileName,
                            errorItem.Line,
                            errorItem.Column,
                            errorItem.Description);
                    })
                    .ToList().AsReadOnly();
        }

        protected static void SetupDialogInteraction<TDialog, TViewModel>(
            Mock<IDialogWindowFactory> factoryMock,
            Func<TViewModel, bool?> showDialogAction)
            where TDialog : class, IDialogWindow, new()
            where TViewModel : class
        {
            TViewModel viewModel = null;

            var dialogMock = new Mock<IDialogWindow>().SetupAllProperties();
            dialogMock
                .Setup(w => w.ShowDialog())
                .Returns(() => showDialogAction(viewModel));

            factoryMock
                .Setup(d => d.CreateDialog<TDialog>(It.IsAny<object>()))
                .Returns((object dc) => { viewModel = (TViewModel)dc; return dialogMock.Object; });
        }
    }
}

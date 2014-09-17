using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    /// Base class for integration tests that need to deploy content.
    /// </summary>
    public abstract class IntegrationTest
    {
        private TestContext context;
        protected ISolution solution;
        protected IPatternManager patternManager;
        protected DTE dte;

        public virtual void InitializeContext()
        {
            solution = VsIdeTestHostContext.ServiceProvider.GetService<ISolution>();
            dte = VsIdeTestHostContext.ServiceProvider.GetService<DTE>();
            patternManager = VsIdeTestHostContext.ServiceProvider.GetService<IPatternManager>();
        }

        /// <summary>
        /// Gets or sets the location of the per-test location directory.
        /// </summary>
        protected string DeploymentDirectory { get; set; }

        /// <summary>
        /// Gets or sets the test context.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return context;
            }

            set
            {
                context = value;
                DeploymentDirectory = Path.Combine(Path.GetTempPath(), TestDirectory);
                CleanDeploymentDirectory();
            }
        }

        private string TestDirectory
        {
            get { return ((uint)context.TestName.GetHashCode()).ToString(CultureInfo.InvariantCulture); }
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
                    dte.Solution.SolutionBuild.Build();

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

        protected static void CloseSolution(DTE dte)
        {
            var mre = new ManualResetEventSlim();
            var events = dte.Events.SolutionEvents;
            _dispSolutionEvents_AfterClosingEventHandler done = () => mre.Set();
            events.AfterClosing += done;
            try
            {
                UIThreadInvoker.Invoke((Action)(() =>
                {
                    dte.Solution.Close(false);
                    mre.Wait();
                }));
            }
            finally
            {
                events.AfterClosing -= done;
            }
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

        /// <summary>
        /// Retruns a path relative to the test deployment directory.
        /// </summary>
        /// <param name="deploymentRelativePath">A relative path that is resolved from the deployment directory.</param>
        /// <returns>The path relative to the test deployment directory.</returns>
        protected string PathTo(string deploymentRelativePath)
        {
            return Path.Combine(DeploymentDirectory, deploymentRelativePath);
        }

        protected void CleanDeploymentDirectory()
        {
            if (Directory.Exists(DeploymentDirectory))
            {
                // Clear readonly attributes
                foreach (var file in Directory.EnumerateFiles(DeploymentDirectory, "*.*", SearchOption.AllDirectories))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                }

                try
                {
                    // Delete the direectory
                    Directory.Delete(DeploymentDirectory, true);
                }
                catch (UnauthorizedAccessException)
                {
                    // Ignore and continue
                }
                catch (IOException)
                {
                    // Ignore and continue
                }
            }
        }
    }
}
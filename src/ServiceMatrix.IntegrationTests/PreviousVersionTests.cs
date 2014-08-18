using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NServiceBusStudio;
using NuPattern.VisualStudio.Solution;

namespace ServiceMatrix.IntegrationTests
{
    [TestClass]
    public class PreviousVersionTests : IntegrationTest
    {
        [TestInitialize]
        public override void InitializeContext()
        {
            base.InitializeContext();

            //System.Windows.MessageBox.Show("Before extraction");

            Assert.IsNotNull(TestContext.DataRow, "This test must be configured as a data-driven test");
            var archiveName = (string)TestContext.DataRow["Archive"];
            Assert.IsNotNull(TestContext.DataRow["Archive"], archiveName);

            if (archiveName.Contains("-2013") && ToolkitConstants.SupportedVsVersion != "12.0")
            {
                Assert.Inconclusive("Archive {0} can only be tested in VS 2013", archiveName);
            }

            var solutionFolder = Path.Combine(DeploymentDirectory, archiveName);

            ExtractSolution(archiveName, solutionFolder);
            var solutionFile =
                Directory.EnumerateFiles(solutionFolder, "*.sln", SearchOption.AllDirectories)
                    .FirstOrDefault(f => Path.GetExtension(f) == ".sln");
            Assert.IsNotNull(solutionFile, "Could not find a solution file in archive {0}", archiveName);

            solution.Open(solutionFile);
        }

        [TestCleanup]
        public void Cleanup()
        {
            CloseSolution(dte);
            CleanDeploymentDirectory();
        }

        [HostType("VS IDE")]
        [TestMethod, TestCategory("Integration")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TestSolutions.csv", "TestSolutions#csv", DataAccessMethod.Sequential)]
        public void ThenSolutionCanBeBuilt()
        {
            if (!BuildSolutionAsync(dte).Result)
            {
                Assert.Fail("Build failed:{1}{0}", string.Join(Environment.NewLine, GetBuildErrors(dte)), Environment.NewLine);
            }
        }

        private static void ExtractSolution(string name, string destinationFolder)
        {
            var solutionArchivePath = Path.GetFullPath(name + ".zip");

            Assert.IsTrue(File.Exists(solutionArchivePath), "Test solution does not exist at {0}", solutionArchivePath);

            using (var solutionStream = File.OpenRead(solutionArchivePath))
            using (var archive = new ZipArchive(solutionStream, ZipArchiveMode.Read, true))
            {
                archive.ExtractToDirectory(destinationFolder);
            }
        }
    }
}
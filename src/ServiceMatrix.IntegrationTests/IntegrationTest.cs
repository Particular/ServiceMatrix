using System;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceMatrix.IntegrationTests
{
    /// <summary>
    /// Base class for integration tests that need to deploy content.
    /// </summary>
    public abstract class IntegrationTest
    {
        private TestContext context;

        /// <summary>
        /// Gets or sets the location of the per-test location directory.
        /// </summary>
        public string DeploymentDirectory { get; set; }

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

        /// <summary>
        /// Retruns a path relative to the test deployment directory.
        /// </summary>
        /// <param name="deploymentRelativePath">A relative path that is resolved from the deployment directory.</param>
        /// <returns>The path relative to the test deployment directory.</returns>
        protected string PathTo(string deploymentRelativePath)
        {
            return Path.Combine(DeploymentDirectory, deploymentRelativePath);
        }

        private void CleanDeploymentDirectory()
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
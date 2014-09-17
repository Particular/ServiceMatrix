using System;
using System.IO;
using System.Linq;
using System.Windows;
using AbstractEndpoint;
using AbstractEndpoint.Automation.Commands;
using AbstractEndpoint.Automation.Dialog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using Moq;
using NServiceBusStudio;
using NServiceBusStudio.Automation;
using NServiceBusStudio.Automation.Commands;
using NServiceBusStudio.Automation.Dialog;
using NServiceBusStudio.Automation.ViewModels;
using NuPattern;
using NuPattern.Runtime;
using NuPattern.VisualStudio.Solution;

namespace ServiceMatrix.IntegrationTests.CreateApplicationSpec
{
    [TestClass]
    public class GivenAddNewDialog : CreateToolkitTest
    {
        [TestInitialize]
        public override void InitializeContext()
        {
            base.InitializeContext();

            UIThreadInvoker.Invoke((Action)(CreatePatternFromProjectTemplate));
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [HostType("VS IDE")]
        [TestMethod, TestCategory("Integration")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TemplateVersions.csv", "TemplateVersions#csv", DataAccessMethod.Sequential)]
        public void ThenProjectTemplateUnfolded()
        {
            // Project are unfolded into solution
            var contractsProject = solution.Find<IProject>(p => p.Name == projectName + ".Contracts").FirstOrDefault();
            Assert.IsNotNull(contractsProject, "Contracts project not found");

            var internalProject = solution.Find<IProject>(p => p.Name == projectName + ".Internal").FirstOrDefault();
            Assert.IsNotNull(internalProject, "Internal project not found");

            // Pattern exists 
            var pattern = patternManager.Products.FirstOrDefault(p => p.InstanceName == solution.Name);
            Assert.IsNotNull(pattern, "Pattern not found");

            var application = pattern.As<IApplication>();

            // Elements linked t the project
            Assert.IsTrue(
                application.Design.ContractsProject.References.Any(r => r.Value == string.Format("solution://{0}/", contractsProject.Id)),
                "Contracts project not linked");
            Assert.IsTrue(
                application.Design.InternalMessagesProject.References.Any(r => r.Value == string.Format("solution://{0}/", internalProject.Id)),
                "Internal project not linked");
        }

        [HostType("VS IDE")]
        [TestMethod, TestCategory("Integration")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TemplateVersions.csv", "TemplateVersions#csv", DataAccessMethod.Sequential)]
        public void ThenSolutionCanBeBuilt()
        {
            if (!BuildSolutionAsync(dte).Result)
            {
                Assert.Fail("Build failed:{1}{0}", string.Join(Environment.NewLine, GetBuildErrors(dte)), Environment.NewLine);
            }
        }
    }

    [TestClass]
    public class GivenApplicationWithSingleMvcEndpoint : CreateToolkitTest
    {
        [TestInitialize]
        public override void InitializeContext()
        {
            base.InitializeContext();

            UIThreadInvoker.Invoke((Action)delegate
            {
                CreatePatternFromProjectTemplate();

                var application = patternManager.Products.First(p => p.InstanceName == solution.Name).As<IApplication>();
                application.Design.Endpoints.CreateNServiceBusMVC("FrontEnd");
            });
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [HostType("VS IDE")]
        [TestMethod, TestCategory("Integration")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TemplateVersions.csv", "TemplateVersions#csv", DataAccessMethod.Sequential)]
        public void ThenEndpointProjectIsCreated()
        {
            var endpoint =
                patternManager.Products.First(p => p.InstanceName == solution.Name).As<IApplication>()
                    .Design.Endpoints.GetAll().FirstOrDefault(e => e.InstanceName == "FrontEnd");
            Assert.IsNotNull(endpoint);

            // Project is unfolded into solution
            var endpointProject = solution.Find<IProject>(p => p.Name == solution.Name + "." + endpoint.InstanceName).FirstOrDefault();
            Assert.IsNotNull(endpointProject, "Endpoint project not found");

            // Elements linked to the project
            Assert.IsTrue(
                endpoint.As<IProductElement>().References.Any(r => r.Value == string.Format("solution://{0}/", endpointProject.Id)),
                "Endpoint project not linked");
        }

        [HostType("VS IDE")]
        [TestMethod, TestCategory("Integration")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TemplateVersions.csv", "TemplateVersions#csv", DataAccessMethod.Sequential)]
        public void ThenSolutionCanBeBuilt()
        {
            if (!BuildSolutionAsync(dte).Result)
            {
                Assert.Fail("Build failed:{1}{0}", string.Join(Environment.NewLine, GetBuildErrors(dte)), Environment.NewLine);
            }
        }
    }

    [TestClass]
    public class GivenSolutionBuilder : CreateToolkitTest
    {
        [TestInitialize]
        public override void InitializeContext()
        {
            base.InitializeContext();

            UIThreadInvoker.Invoke((Action)delegate
            {
                base.CreatePatternFromPatternManager(ServiceMatrixToolkitInfo.VsixIdentifier);
            });
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [HostType("VS IDE")]
        [TestMethod, TestCategory("Integration")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TemplateVersions.csv", "TemplateVersions#csv", DataAccessMethod.Sequential)]
        public void ThenPatternElementCreated()
        {
            // Project are unfolded into solution
            var contractsProject = solution.Find<IProject>(p => p.Name == projectName + ".Contracts").FirstOrDefault();
            Assert.IsNotNull(contractsProject, "Contracts project not found");

            var internalProject = solution.Find<IProject>(p => p.Name == projectName + ".Internal").FirstOrDefault();
            Assert.IsNotNull(internalProject, "Internal project not found");

            // Pattern exists 
            var pattern = patternManager.Products.FirstOrDefault(p => p.InstanceName == solution.Name);
            Assert.IsNotNull(pattern, "Pattern not found");

            var application = pattern.As<IApplication>();

            // Elements linked t the project
            Assert.IsTrue(
                application.Design.ContractsProject.References.Any(r => r.Value == string.Format("solution://{0}/", contractsProject.Id)),
                "Contracts project not linked");
            Assert.IsTrue(
                application.Design.InternalMessagesProject.References.Any(r => r.Value == string.Format("solution://{0}/", internalProject.Id)),
                "Internal project not linked");
        }
    }

    [TestClass]
    public class GivenApplicationWithSenderAndHandlerOfCommand : CreateToolkitTest
    {
        [TestInitialize]
        public override void InitializeContext()
        {
            base.InitializeContext();

            // create the model
            UIThreadInvoker.Invoke((Action)delegate
            {
                CreatePatternFromProjectTemplate();

                try
                {
                    var application = patternManager.Products.First(p => p.InstanceName == solution.Name).As<IApplication>();
                    IAbstractEndpoint endpoint;

                    // Create the endpoint
                    using (var tx = application.As<IProductElement>().BeginTransaction())
                    {
                        endpoint = application.Design.Endpoints.CreateNServiceBusMVC("FrontEnd");
                        tx.Commit();
                    }

                    var dialogFactoryMock = new Mock<IDialogWindowFactory>();
                    var messageBoxServiceMock = new Mock<IMessageBoxService>();

                    // Send the command
                    using (var tx = endpoint.As<IProductElement>().BeginTransaction())
                    {
                        SetupDialogInteraction<ServiceAndCommandPicker, ServiceAndCommandPickerViewModel>(
                            dialogFactoryMock,
                            vm =>
                            {
                                vm.SelectedService = "Sales";
                                vm.SelectedCommand = "SendOrder";
                                vm.SelectedHandlerComponent = null;
                                return true;
                            });

                        new ShowCommandComponentPicker
                        {
                            CurrentElement = endpoint.As<IProductElement>(),
                            WindowFactory = dialogFactoryMock.Object,
                            MessageBoxService = messageBoxServiceMock.Object
                        }.Execute();

                        tx.Commit();
                    }

                    var handler =
                        application.Design.Services.Service.First(s => s.InstanceName == "Sales")
                            .Components.Component.First(c => c.InstanceName == "SendOrderHandler");

                    // Deploy the handler component
                    using (var tx = endpoint.As<IProductElement>().BeginTransaction())
                    {
                        SetupDialogInteraction<EndpointPicker, EndpointPickerViewModel>(
                            dialogFactoryMock,
                            vm =>
                            {
                                vm.SelectedItems = new[] { "BackEnd [NServiceBus Host]" };
                                return true;
                            });

                        new ShowDeployToPicker { CurrentElement = handler.As<IProductElement>(), WindowFactory = dialogFactoryMock.Object }.Execute();
                        tx.Commit();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error");
                    throw;
                }
            });
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [HostType("VS IDE")]
        [TestMethod, TestCategory("Integration")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TemplateVersions.csv", "TemplateVersions#csv", DataAccessMethod.Sequential)]
        public void ThenTheModelIsUpdated()
        {
            var application = patternManager.Products.First(p => p.InstanceName == solution.Name).As<IApplication>();

            var frontEndEndpoint = application.Design.Endpoints.GetAll().FirstOrDefault(ep => ep.InstanceName == "FrontEnd");
            Assert.IsNotNull(frontEndEndpoint, "Cannot find endpoint FrontEnd");
            Assert.IsTrue(frontEndEndpoint.GetType().IsAssignableTo(typeof(INServiceBusMVC)), "FrontEnd is not mvc endpoint");

            var backEndEndpoint = application.Design.Endpoints.GetAll().FirstOrDefault(ep => ep.InstanceName == "BackEnd");
            Assert.IsNotNull(backEndEndpoint, "Cannot find endpoint BackEnd");
            Assert.IsInstanceOfType(backEndEndpoint, typeof(INServiceBusHost), "FrontEnd is not host endpoint");

            // TODO flesh out model test
        }

        [HostType("VS IDE")]
        [TestMethod, TestCategory("Integration")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TemplateVersions.csv", "TemplateVersions#csv", DataAccessMethod.Sequential)]
        public void ThenCodeIsGeneratedOnFrontEndEndpoints()
        {
            var endpoint =
                patternManager.Products.First(p => p.InstanceName == solution.Name).As<IApplication>()
                    .Design.Endpoints.GetAll().FirstOrDefault(e => e.InstanceName == "FrontEnd");
            Assert.IsNotNull(endpoint);

            // Project is unfolded into solution

            var endpointProject = solution.Find<IProject>(p => p.Name == solution.Name + "." + endpoint.InstanceName).FirstOrDefault();
            Assert.IsNotNull(endpointProject, "Endpoint project not found");

            // Elements linked to the project

            Assert.IsTrue(
                endpoint.As<IProductElement>().References.Any(r => r.Value == string.Format("solution://{0}/", endpointProject.Id)),
                "Endpoint project not linked");

            // Check project structure

            AssertProject.HasFolder(
                endpointProject,
                "Infrastructure",
                f => AssertProject.HasItem(f, "Logging.config",
                        i => Assert.IsTrue(File.ReadAllText(i.PhysicalPath).Contains(@"<Logging Threshold=""INFO""/>"), "missing information in logging config")),
                f => AssertProject.HasItem(f, "MessageConventions.cs"));

            // TODO flesh out checks
        }

        [HostType("VS IDE")]
        [TestMethod, TestCategory("Integration")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TemplateVersions.csv", "TemplateVersions#csv", DataAccessMethod.Sequential)]
        public void ThenSolutionCanBeBuilt()
        {
            if (!BuildSolutionAsync(dte).Result)
            {
                Assert.Fail("Build failed:{1}{0}", string.Join(Environment.NewLine, GetBuildErrors(dte)), Environment.NewLine);
            }
        }
    }
}

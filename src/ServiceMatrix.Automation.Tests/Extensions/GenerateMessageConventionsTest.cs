namespace ServiceMatrix.Automation.Tests.Extensions
{
    using NServiceBusStudio.Automation.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class GenerateMessageConventionsTest
    {
        [Test]
        public void GetMessageConventionsTest_WhenRootNamespaceHasAValue()
        {
            string rootNamespace = "MyNServiceBusSystem";
            string applicationName = "MyNServiceBusSystem";
            string projectNameForInternal = "InternalMessages";
            string projectNameForContracts = "ContractEvents";

            var generatedConventions = GenerateMessageConventions.GetMessageConventions(rootNamespace, applicationName, projectNameForInternal, projectNameForContracts);

            var expectedConventions = @"namespace MyNServiceBusSystem
{
    public class MessageConventions : IWantToRunBeforeConfiguration
    {
        public void Init()
        {
            Configure.Instance
            .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith(""MyNServiceBusSystem.InternalMessages.Commands""))
            .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith(""MyNServiceBusSystem.ContractEvents""))
            .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith(""MyNServiceBusSystem.InternalMessages.Messages""));
        }
    }
}
";
            Assert.AreEqual(expectedConventions, generatedConventions);
        }

        [Test]
        public void GetMessageConventionsTest_WhenRootNamespaceIsNull()
        {
            string rootNamespace = null;
            string applicationName = "MyNServiceBusSystem";
            string projectNameForInternal = "InternalMessages";
            string projectNameForContracts = "ContractEvents";

            var generatedConventions = GenerateMessageConventions.GetMessageConventions(rootNamespace, applicationName, projectNameForInternal, projectNameForContracts);

            var expectedConventions = @"{
    public class MessageConventions : IWantToRunBeforeConfiguration
    {
        public void Init()
        {
            Configure.Instance
            .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith(""MyNServiceBusSystem.InternalMessages.Commands""))
            .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith(""MyNServiceBusSystem.ContractEvents""))
            .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith(""MyNServiceBusSystem.InternalMessages.Messages""));
        }
    }
}
";
            Assert.AreEqual(expectedConventions, generatedConventions);
        }
    }
}
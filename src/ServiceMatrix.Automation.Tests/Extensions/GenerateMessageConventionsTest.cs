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
            var rootNamespace = "MyNServiceBusSystem";
            var applicationName = "MyNServiceBusSystem";
            var projectNameForInternal = "InternalMessages";
            var projectNameForContracts = "ContractEvents";

            var generatedConventions = GenerateMessageConventions.GetMessageConventionsV4(rootNamespace, applicationName, projectNameForInternal, projectNameForContracts);
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
            var applicationName = "System";
            var projectNameForInternal = "Internal";
            var projectNameForContracts = "Events";

            var generatedConventions = GenerateMessageConventions.GetMessageConventionsV4(rootNamespace, applicationName, projectNameForInternal, projectNameForContracts);

            var expectedConventions = @"{
    public class MessageConventions : IWantToRunBeforeConfiguration
    {
        public void Init()
        {
            Configure.Instance
            .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith(""System.Internal.Commands""))
            .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith(""System.Events""))
            .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith(""System.Internal.Messages""));
        }
    }
}
";
            Assert.AreEqual(expectedConventions, generatedConventions);
        }
    }
}
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
            var generatedConventions = GenerateMessageConventions.GetMessageConventions(rootNamespace: "MyNServiceBusSystem", applicationName: "MyNServiceBusSystem", projectNameForInternal: "InternalMessages", projectNameForContracts: "ContractEvents");
            const string expectedConventions = @"namespace MyNServiceBusSystem
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
            var generatedConventions = GenerateMessageConventions.GetMessageConventions(rootNamespace: null, applicationName: "System", projectNameForInternal: "Internal", projectNameForContracts: "Events");
            const string expectedConventions = @"{
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
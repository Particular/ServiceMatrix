using System;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Library.Automation;
using NuPattern.Modeling;
using NuPattern.Runtime;

namespace NServiceBusStudio
{
    partial class Application
    {
        private void SetupCustomAutomation()
        {
            var toolkit = PatternManager.InstalledToolkits.FirstOrDefault(it => it.Name == ToolkitConstants.ToolkitName);
            if (toolkit == null)
            {
                tracer.Warn("ServiceMatrix toolkit not found for schema update");
                return;
            }

            var schema = toolkit.Schema.As<IPatternModelSchema>();

            // check for sentinel automation to indicate automation has already been updated
            const string automationUpdated = "__automation_updated";
            if (schema.Pattern.GetAutomationSettings<ICommandSettings>(automationUpdated) != null)
            {
                tracer.Info("ServiceMatrix toolkit automation already updated");
                return;
            }

            schema.Pattern.CreateAutomationSettings<ICommandSettings>(automationUpdated);

            // update automation as needed
            foreach (var element in ((IElementSchemaContainer)schema.Pattern.Views.First()).Elements.Traverse(e => e.Elements))
            {
                switch (element.Name)
                {
                    case "NServiceBusHost":
                        SetupNServiceBusHostCustomAutomation(element);
                        break;

                    case "NServiceBusMVC":
                        SetupNServiceBusMVCCustomAutomation(element);
                        break;

                    case "Authentication":
                        SetupAuthenticationCustomAutomation(element);
                        break;

                    case "Component":
                        SetupComponentCustomAutomation(element);
                        break;
                }
            }
        }

        private void SetupNServiceBusHostCustomAutomation(IPatternElementSchema element)
        {
            var store = ((ModelElement)element).Store;

            store.TransactionManager.DoWithinTransaction(() =>
            {
                element.CreateDispatchedCodeGenerationCommands(
                    "GenerateTransportConfigCode",
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "TransportConfig.cs",
                    "~",
                    Tuple.Create(
                        "{994EBF16-BFEF-44D5-94B6-E1B681BD1F02}",
                        @"/T/T4/Endpoints/NSBH/TransportConfig.v4.tt",
                        "NSB 4.0"),
                    Tuple.Create(
                        "{FDEBE349-8384-4BF9-BC48-62A8FA613898}",
                        @"/T/T4/Endpoints/NSBH/TransportConfig.v5.tt",
                        "NSB 5.0"));

                element.CreateDispatchedCodeGenerationCommands(
                    "UnfoldEndpointConfig",
                    true,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "EndpointConfig.cs",
                    "~",
                    Tuple.Create(
                        "{99F02857-A449-4104-A00F-4BD6CF3E32C2}",
                        @"/T/T4/Endpoints/NSBH/EndpointConfig.v4.cs.tt",
                        "NSB 4.0"),
                    Tuple.Create(
                        "{BECFB792-CC7F-48F6-B384-483A83A53879}",
                        @"/T/T4/Endpoints/NSBH/EndpointConfig.v5.cs.tt",
                        "NSB 5.0"));

                element.CreateDispatchedCodeGenerationCommands(
                    "UnfoldMessageConventions",
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "MessageConventions.cs",
                    "~/Infrastructure",
                    Tuple.Create(
                        "{DD76F4D3-9F33-4A74-ACB5-D75A279AA3F1}",
                        @"/T/T4/Endpoints/NSBH/MessageConventions.v4.cs.tt",
                        "NSB 4.0"),
                    Tuple.Create(
                        "{4DB05B2E-7818-4BB9-A0C3-00A802D9AE72}",
                        @"/T/T4/Endpoints/NSBH/MessageConventions.v5.cs.tt",
                        "NSB 5.0"));
            });
        }

        private void SetupNServiceBusMVCCustomAutomation(IPatternElementSchema element)
        {
            var store = ((ModelElement)element).Store;

            store.TransactionManager.DoWithinTransaction(() =>
            {
                element.CreateDispatchedCodeGenerationCommands(
                    "GenerateTransportConfigCode",
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "TransportConfig.cs",
                    "~/Infrastructure/GeneratedCode",
                    Tuple.Create(
                        "{F915D87E-496C-46C6-A50E-B1C78E07859F}",
                        @"/T/T4/Endpoints/NSBMVC/TransportConfig.v4.tt",
                        "NSB 4.0"),
                    Tuple.Create(
                        "{939D688C-967D-45AB-80D5-E973AEBCE2F5}",
                        @"/T/T4/Endpoints/NSBMVC/TransportConfig.v5.tt",
                        "NSB 5.0"));

                element.CreateDispatchedCodeGenerationCommands(
                    "GenerateWebGlobalInitialization",
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "WebGlobalInitialization.cs",
                    "~/Infrastructure/",
                    Tuple.Create(
                        "{82A49197-D43C-4F7C-A892-73CA7E379038}",
                        @"/T/T4/Endpoints/NSBMVC/WebGlobalInitialization.v4.cs.tt",
                        "NSB 4.0"),
                    Tuple.Create(
                        "{AC85145E-C26A-4192-9D56-BFC8F2104247}",
                        @"/T/T4/Endpoints/NSBMVC/WebGlobalInitialization.v5.cs.tt",
                        "NSB 5.0"));

                element.CreateDispatchedCodeGenerationCommands(
                    "GenerateWebInitialization",
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "GeneratedWebInitialization.cs",
                    "~/Infrastructure/GeneratedCode",
                    Tuple.Create(
                        "{7DD7DFF4-1EAD-4E8F-B746-66BB26389921}",
                        @"/T/T4/Endpoints/NSBMVC/GeneratedWebInitialization.v4.cs.tt",
                        "NSB 4.0"),
                    Tuple.Create(
                        "{26AB22CC-C8A8-417B-B48E-562D5546B312}",
                        @"/T/T4/Endpoints/NSBMVC/GeneratedWebInitialization.v5.cs.tt",
                        "NSB 5.0"));

                element.CreateDispatchedCodeGenerationCommands(
                    "UnfoldMessageConventions",
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "MessageConventions.cs",
                    "~/Infrastructure",
                    Tuple.Create(
                        "{70837325-FE48-481F-A1FF-A86B1E23E0B3}",
                        @"/T/T4/Endpoints/NSBMVC/MessageConventions.v4.cs.tt",
                        "NSB 4.0"),
                    Tuple.Create(
                        "{9CF38A59-EA3C-4A19-B72C-FC7FD51A0FE3}",
                        @"/T/T4/Endpoints/NSBMVC/MessageConventions.v5.cs.tt",
                        "NSB 5.0"));
            });
        }

        private void SetupComponentCustomAutomation(IPatternElementSchema element)
        {
            var store = ((ModelElement)element).Store;

            store.TransactionManager.DoWithinTransaction(() =>
            {
                element.CreateDispatchedComponentCodeGenerationCommands(
                    "UnfoldSagaConfigureHowToFindCode",
                    true,
                    false,
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "{CodeIdentifier}ConfigureHowToFindSaga.cs",
                    "",
                    Tuple.Create(
                        "{71D09A77-97F7-4E5C-B1FC-515E77014CB0}",
                        @"/T/T4/CustomComponentSagaConfigureHowToFind.v4.tt",
                        "NSB 4.0"),
                    Tuple.Create(
                        "{03B6842B-387E-49AE-A07A-2E11DBF80474}",
                        @"/T/T4/CustomComponentSagaConfigureHowToFind.v5.tt",
                        "NSB 5.0"));
            });
        }

        private void SetupAuthenticationCustomAutomation(IPatternElementSchema element)
        {
            var store = ((ModelElement)element).Store;

            store.TransactionManager.DoWithinTransaction(() =>
            {
                element.CreateDispatchedCodeGenerationCommands(
                    "GenerateCodeAuthorizeOutgoingMessages",
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "AuthorizeOutgoingMessages.cs",
                    "{CodePath}",
                    Tuple.Create(
                        "{7974F45D-ED79-4839-9779-C38974C480B7}",
                        @"/T/T4/Security/AuthorizeOutgoingMessages.v4.tt",
                        "NSB 4.0"),
                    Tuple.Create(
                        "{C8C9EE4A-4C7C-4AA3-88BF-35422894FC2A}",
                        @"/T/T4/Security/AuthorizeOutgoingMessages.v5.tt",
                        "NSB 5.0"));

                element.CreateDispatchedCodeGenerationCommands(
                    "UnfoldCustomAuthorizeOutgoingMessagesCode",
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "AuthorizeOutgoingMessages.cs",
                    "{CustomCodePath}",
                    Tuple.Create(
                        "{6A9D6763-B1B1-4C29-A53D-076ADF17BA10}",
                        @"/T/T4/Security/CustomAuthorizeOutgoingMessages.v4.tt",
                        "NSB 4.0"),
                    Tuple.Create(
                        "{F3FA76A0-10FF-4634-A62E-77562DD18193}",
                        @"/T/T4/Security/CustomAuthorizeOutgoingMessages.v5.tt",
                        "NSB 5.0"));
            });
        }
    }
}

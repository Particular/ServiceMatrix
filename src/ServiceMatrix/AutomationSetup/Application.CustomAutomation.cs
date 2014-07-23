using System;
using System.Linq;
using NuPattern;
using NuPattern.Library.Automation;
using NuPattern.Runtime;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio
{
    public static class AutomationConfiguration
    {
        public static void SetupCustomAutomation(IPatternManager patternManager)
        {
            try
            {
                var toolkit = patternManager.InstalledToolkits.FirstOrDefault(it => it.Name == ToolkitConstants.ToolkitName);
                if (toolkit == null)
                {
                    return;
                }

                var schema = toolkit.Schema.As<IPatternModelSchema>();

                SetupCustomAutomation(schema.Pattern);
                // update automation as needed
                foreach (var element in ((IElementSchemaContainer)schema.Pattern.Views.First()).Elements.Traverse(e => e.Elements))
                {
                    SetupCustomAutomation(element);
                }
            }
            catch (Exception e)
            {

            }
        }

        private static void SetupCustomAutomation(IPatternElementSchema element)
        {
            switch (element.Name)
            {
                case "Application":
                    Setup_Application_Automation(element);
                    break;

                case "Authentication":
                    Setup_Authentication_Automation(element);
                    break;

                case "Command":
                    Setup_Command_Automation(element);
                    break;

                case "CommandLink":
                    Setup_CommandLink_Automation(element);
                    break;

                case "Component":
                    Setup_Component_Automation(element);
                    break;

                case "ContractsProject":
                    Setup_ContractsProject_Automation(element);
                    break;

                case "Endpoints":
                    Setup_Endpoints_Automation(element);
                    break;

                case "Event":
                    Setup_Event_Automation(element);
                    break;

                case "EventLink":
                    Setup_EventLink_Automation(element);
                    break;

                case "Infrastructure":
                    Setup_Infrastructure_Automation(element);
                    break;

                case "InternalMessagesProject":
                    Setup_InternalMessagesProject_Automation(element);
                    break;

                case "Libraries":
                    Setup_Libraries_Automation(element);
                    break;

                case "Library":
                    Setup_Library_Automation(element);
                    break;

                case "LibraryReference":
                    Setup_LibraryReference_Automation(element);
                    break;

                case "LibraryReferences":
                    Setup_LibraryReferences_Automation(element);
                    break;

                case "Message":
                    Setup_Message_Automation(element);
                    break;

                case "NServiceBusHost":
                    Setup_NServiceBusHost_Automation(element);
                    break;

                case "NServiceBusMVC":
                    Setup_NServiceBusMVC_Automation(element);
                    break;

                case "NServiceBusWeb":
                    Setup_NServiceBusWeb_Automation(element);
                    break;

                case "ProcessedCommandLink":
                    Setup_ProcessedCommandLink_Automation(element);
                    break;

                case "Publishes":
                    Setup_Publishes_Automation(element);
                    break;

                case "Service":
                    Setup_Service_Automation(element);
                    break;

                case "ServiceLibrary":
                    Setup_ServiceLibrary_Automation(element);
                    break;

                case "SubscribedEventLink":
                    Setup_SubscribedEventLink_Automation(element);
                    break;

                case "UseCase":
                    Setup_UseCase_Automation(element);
                    break;

                default:
                    switch (element.Id.ToString("N"))
                    {
                        case "58bd0df25250472da1694d72726325ae":
                            // ComponentLink
                            Setup_ComponentLink_58bd0df25250472da1694d72726325ae_Automation(element);
                            break;

                        case "813d58eb16cd43349bfb6ebbc5dc7ee6":
                            // ComponentLink
                            Setup_ComponentLink_813d58eb16cd43349bfb6ebbc5dc7ee6_Automation(element);
                            break;

                        case "18db2bf834144c909e4639e0475a018b":
                            // ComponentLink
                            Setup_ComponentLink_18db2bf834144c909e4639e0475a018b_Automation(element);
                            break;

                        case "4640ec7cc09c42a690deff9cd99fd6b1":
                            // Components
                            Setup_Components_4640ec7cc09c42a690deff9cd99fd6b1_Automation(element);
                            break;

                        case "7fd9877f4de14d74a0a3d3a09cc06a73":
                            // Components
                            Setup_Components_7fd9877f4de14d74a0a3d3a09cc06a73_Automation(element);
                            break;

                        case "2cb77a1ff887467cb0e3df2de031b87c":
                            // Components
                            Setup_Components_2cb77a1ff887467cb0e3df2de031b87c_Automation(element);
                            break;

                    }
                    break;
            }
        }

        private static void Setup_Application_Automation(IPatternElementSchema element)
        {
            var ActivateGuidanceCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("39affcab-a0b8-4be3-9b51-3c54698f9583"),
                    "ActivateGuidanceCommand",
                    () =>
                        new NuPattern.Library.Commands.ActivateGuidanceWorkflowCommand
                        {
                        });

            var CollapseFolders =
                element.CreateOrUpdateCommandSettings(
                    new Guid("75d3d1f8-ab84-48a5-aabe-bef2ff81d693"),
                    "CollapseFolders",
                    () =>
                        new NServiceBusStudio.Automation.Commands.CollapseFoldersCommand
                        {
                        });

            var GenerateCodeV4RecursiveCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("07a4cbaf-1f2e-4b22-a131-4909ad9e3d93"),
                    "GenerateCodeV4RecursiveCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ChildCommandReference
                        {
                            CommandNameStartsWidth = "GenerateCodeV4",
                            Recursive = BindingFor.Value<bool>("True"),
                        });

            var GenerateCodeV5RecursiveCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("00e00c01-7c18-4dd2-bc36-eac0a7c1789e"),
                    "GenerateCodeV5RecursiveCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ChildCommandReference
                        {
                            CommandNameStartsWidth = "GenerateCodeV5",
                            Recursive = BindingFor.Value<bool>("True"),
                        });

            var InstantiateGuidanceCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("c9122330-58f6-49c9-bcdf-a26c1cc94d41"),
                    "InstantiateGuidanceCommand",
                    () =>
                        new NuPattern.Library.Commands.InstantiateGuidanceWorkflowCommand
                        {
                            ActivateOnInstantiation = BindingFor.Value<bool>("True"),
                            DefaultInstanceName = "ServiceMatrix Guidance",
                            ExtensionId = "23795EC3-3DEA-4F04-9044-4056CF91A2ED",
                            SharedInstance = BindingFor.Value<bool>("True"),
                        });

            var OnApplicationLoaded =
                element.CreateOrUpdateCommandSettings(
                    new Guid("15a636ca-6bc9-4a89-8364-bc65a05596a2"),
                    "OnApplicationLoaded",
                    () =>
                        new NServiceBusStudio.Automation.Commands.OnApplicationLoadedCommand
                        {
                        });

            var ResetIsDirtyCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("a3ab5f66-c305-4b86-8cbb-aded2e14fc99"),
                    "ResetIsDirtyCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ResetIsDirtyFlagCommand
                        {
                        });

            var SetStartUpProjects =
                element.CreateOrUpdateCommandSettings(
                    new Guid("2f04ba30-d3f9-4263-9021-cd1afe2a0075"),
                    "SetStartUpProjects",
                    () =>
                        new NServiceBusStudio.Automation.Commands.SetStartUpProjects
                        {
                        });

            var ShowNewDiagramCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("a699f3b4-aef7-4d72-a057-1c3375ce33d5"),
                    "ShowNewDiagramCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowNewDiagramCommand
                        {
                        });

            var ValidateCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("fc87707a-98cd-4293-935c-85eb91b68867"),
                    "ValidateCommand",
                    () =>
                        new NuPattern.Library.Commands.ValidateElementCommand
                        {
                            ValidateDescendants = BindingFor.Value<bool>("True"),
                        });

            var GenarateCodeCommands =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "GenarateCodeCommands",
                    SetStartUpProjects,
                    CollapseFolders);

            var OnBuildFinishedCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnBuildFinishedCommand",
                    ResetIsDirtyCommand,
                    CollapseFolders);

            var OnBuildV4Command =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnBuildV4Command",
                    SetStartUpProjects,
                    ValidateCommand,
                    GenerateCodeV4RecursiveCommand,
                    CollapseFolders);

            var OnBuildV5Command =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnBuildV5Command",
                    SetStartUpProjects,
                    ValidateCommand,
                    GenerateCodeV5RecursiveCommand,
                    CollapseFolders);

            var ShowDiagramCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "ShowDiagramCommand",
                    ShowNewDiagramCommand);

            // commands referenced by automation  
            // - ActivateGuidanceCommand
            // - InstantiateGuidanceCommand
            // - OnApplicationLoaded
            // - OnBuildFinishedCommand
            // - OnBuildV4Command
            // - OnBuildV5Command
            // - ShowDiagramCommand
        }

        private static void Setup_Authentication_Automation(IPatternElementSchema element)
        {
            var CollapseFolders =
                element.CreateOrUpdateCommandSettings(
                    new Guid("076a40d9-312e-4f7d-b44a-8f74e2d6d8aa"),
                    "CollapseFolders",
                    () =>
                        new NServiceBusStudio.Automation.Commands.CollapseFoldersCommand
                        {
                        });

            var GenerateCodeV4Authentication =
                element.CreateOrUpdateCommandSettings(
                    new Guid("53a2bbd6-bf49-4f34-87dc-12d40a23db55"),
                    "GenerateCodeV4Authentication",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "Authentication.cs",
                            TargetPath = "{CodePath}",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Security/Authentication.tt"),
                        });

            var GenerateCodeV4AuthorizeOutgoingMessages =
                element.CreateOrUpdateCommandSettings(
                    new Guid("2f72fa6d-0d5e-4793-bd63-e6731ca60a9c"),
                    "GenerateCodeV4AuthorizeOutgoingMessages",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "AuthorizeOutgoingMessages.cs",
                            TargetPath = "{CodePath}",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Security/AuthorizeOutgoingMessages.v4.tt"),
                        });

            var GenerateCodeV4Endpoints =
                element.CreateOrUpdateCommandSettings(
                    new Guid("6dd267fc-9cee-41a3-9595-6f0b9cfabb21"),
                    "GenerateCodeV4Endpoints",
                    () =>
                        new NServiceBusStudio.Automation.Infrastructure.Authentication.AuthenticationAddedCommand
                        {
                        });

            var GenerateCodeV5Authentication =
                element.CreateOrUpdateCommandSettings(
                    new Guid("6b3fb128-6551-4807-9cf5-5da8699fd505"),
                    "GenerateCodeV5Authentication",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "Authentication.cs",
                            TargetPath = "{CodePath}",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Security/Authentication.tt"),
                        });

            var GenerateCodeV5AuthorizeOutgoingMessages =
                element.CreateOrUpdateCommandSettings(
                    new Guid("b93c90d0-5576-4be7-a2c9-fc6557777bec"),
                    "GenerateCodeV5AuthorizeOutgoingMessages",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "AuthorizeOutgoingMessages.cs",
                            TargetPath = "{CodePath}",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Security/AuthorizeOutgoingMessages.v5.tt"),
                        });

            var GenerateCodeV5Endpoints =
                element.CreateOrUpdateCommandSettings(
                    new Guid("e0fa87ec-ce16-4662-8fa1-55f5228a5547"),
                    "GenerateCodeV5Endpoints",
                    () =>
                        new NServiceBusStudio.Automation.Infrastructure.Authentication.AuthenticationAddedCommand
                        {
                        });

            var OpenAuthentication =
                element.CreateOrUpdateCommandSettings(
                    new Guid("83bf9ff8-7b4c-4a1c-b46d-46c568aeb158"),
                    "OpenAuthentication",
                    () =>
                        new NuPattern.Library.Commands.ActivateElementSolutionItemCommand
                        {
                            Open = BindingFor.Value<bool>("True"),
                            TargetFileName = "Authentication.cs",
                            TargetPath = "{CustomCodePath}",
                        });

            var UnfoldCustomAuthenticationCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("faab3dac-5ee3-4a8b-b9cb-1b213c5febb8"),
                    "UnfoldCustomAuthenticationCode",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "Authentication.cs",
                            TargetPath = "{CustomCodePath}",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Security/CustomAuthentication.tt"),
                        });

            var UnfoldCustomAuthorizeOutgoingMessagesCodeV4 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("ad39dc41-ba71-402c-af06-38f10df1df13"),
                    "UnfoldCustomAuthorizeOutgoingMessagesCodeV4",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "AuthorizeOutgoingMessages.cs",
                            TargetPath = "{CustomCodePath}",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Security/CustomAuthorizeOutgoingMessages.v4.tt"),
                        });

            var UnfoldCustomAuthorizeOutgoingMessagesCodeV5 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("305a05ef-5290-440e-807b-550fb1783cfa"),
                    "UnfoldCustomAuthorizeOutgoingMessagesCodeV5",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "AuthorizeOutgoingMessages.cs",
                            TargetPath = "{CustomCodePath}",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Security/CustomAuthorizeOutgoingMessages.v5.tt"),
                        });

            var OnInstantiatedV4Command =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiatedV4Command",
                    GenerateCodeV4Authentication,
                    GenerateCodeV4AuthorizeOutgoingMessages,
                    UnfoldCustomAuthenticationCode,
                    UnfoldCustomAuthorizeOutgoingMessagesCodeV4,
                    GenerateCodeV4Endpoints,
                    CollapseFolders);

            var OnInstantiatedV5Command =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiatedV5Command",
                    GenerateCodeV5Authentication,
                    GenerateCodeV5AuthorizeOutgoingMessages,
                    UnfoldCustomAuthenticationCode,
                    UnfoldCustomAuthorizeOutgoingMessagesCodeV5,
                    GenerateCodeV5Endpoints,
                    CollapseFolders);

            // commands referenced by automation  
            // - OnInstantiatedV4Command
            // - OnInstantiatedV5Command
            // - OpenAuthentication
        }

        private static void Setup_Command_Automation(IPatternElementSchema element)
        {
            var CheckForCreateCommandComponents =
                element.CreateOrUpdateCommandSettings(
                    new Guid("9bfe5976-64ce-4aef-931a-0f8ff8369bdb"),
                    "CheckForCreateCommandComponents",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ConditionalCommand
                        {
                            CommandName = "CreateCommandComponents",
                            ExpectedValue = BindingFor.Value<bool>("False"),
                            PropertyName = "DoNotAutogenerateComponents",
                        });

            var CreateCommandComponents =
                element.CreateOrUpdateCommandSettings(
                    new Guid("9c0438c7-b153-4c43-a868-e333c50d4307"),
                    "CreateCommandComponents",
                    () =>
                        new NServiceBusStudio.Automation.Commands.CreateCommandComponents
                        {
                        });

            var ExGenerateCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("baf9b6a1-32fe-4902-987a-4e60bbf7baf8"),
                    "ExGenerateCode",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NuPattern.Library.ValueProviders.ExpressionValueProvider
                                {
                                    Expression = "{Root.InstanceName}.{Root.ProjectNameInternalMessages}\\GeneratedCode\\{Parent.Parent.Parent.InstanceName}",
                                }),
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/CommandDefinition.tt"),
                        });

            var OpenCodeFile =
                element.CreateOrUpdateCommandSettings(
                    new Guid("128181df-4a4f-4a1f-a1be-117cdc564d8b"),
                    "OpenCodeFile",
                    () =>
                        new NuPattern.Library.Commands.ActivateArtifactCommand
                        {
                            Open = BindingFor.Value<bool>("True"),
                        });

            var UnfoldCommandFile =
                element.CreateOrUpdateCommandSettings(
                    new Guid("e0cce5ad-fee6-4711-89d3-01a04457a8f0"),
                    "UnfoldCommandFile",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NuPattern.Library.ValueProviders.ExpressionValueProvider
                                {
                                    Expression = "{Root.InstanceName}.{Root.ProjectNameInternalMessages}\\{Parent.Parent.Parent.InstanceName}",
                                }),
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/CustomCommandDefinition.tt"),
                        });

            var UnfoldCodeCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "UnfoldCodeCommand",
                    UnfoldCommandFile);

            var OnInstantiateCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateCommand",
                    CheckForCreateCommandComponents,
                    UnfoldCodeCommand);

            // commands referenced by automation  
            // - OnInstantiateCommand
            // - OpenCodeFile
        }

        private static void Setup_CommandLink_Automation(IPatternElementSchema element)
        {
            var AddEndpointProjectReferences =
                element.CreateOrUpdateCommandSettings(
                    new Guid("8b0a62f2-5123-43fc-a68e-53c8d2153c69"),
                    "AddEndpointProjectReferences",
                    () =>
                        new NServiceBusStudio.Automation.Commands.AddEndpointProjectReferences
                        {
                        });

            var OnInstantiateCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateCommand",
                    AddEndpointProjectReferences);

            // commands referenced by automation  
            // - OnInstantiateCommand
        }

        private static void Setup_Component_Automation(IPatternElementSchema element)
        {
            var DeployToCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("ccd80bf1-1638-4cb5-ae70-a1539dbaf6de"),
                    "DeployToCommand",
                    () =>
                        new AbstractEndpoint.Automation.Commands.ShowDeployToPicker
                        {
                        });

            var OpenConfigureHowToFindSaga =
                element.CreateOrUpdateCommandSettings(
                    new Guid("7e43a8c5-79b3-49e7-a02b-2cf932a45e93"),
                    "OpenConfigureHowToFindSaga",
                    () =>
                        new NuPattern.Library.Commands.ActivateElementSolutionItemCommand
                        {
                            Open = BindingFor.Value<bool>("True"),
                            TargetFileName = "{CodeIdentifier}ConfigureHowToFindSaga.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NServiceBusStudio.Automation.ValueProviders.GetEndpointPathValueProvider
                                {
                                    AddInfrastructureFolder = BindingFor.Value<bool>("False"),
                                }),
                        });

            var OpenCustomCodeFile =
                element.CreateOrUpdateCommandSettings(
                    new Guid("db0a6380-180b-410f-982b-f3d1589111f3"),
                    "OpenCustomCodeFile",
                    () =>
                        new NuPattern.Library.Commands.ActivateElementSolutionItemCommand
                        {
                            Open = BindingFor.Value<bool>("True"),
                            TargetFileName = "{CodeIdentifier}.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NServiceBusStudio.Automation.ValueProviders.GetEndpointPathValueProvider
                                {
                                    AddInfrastructureFolder = BindingFor.Value<bool>("False"),
                                }),
                        });

            var PublishCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("171d4d7f-9c83-4eea-92c7-63f1bd8ad9c4"),
                    "PublishCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowEventTypePicker
                        {
                            //CurrentComponent = "",
                        });

            var RaiseOnInstantiated =
                element.CreateOrUpdateCommandSettings(
                    new Guid("47f2a720-e7a1-4568-addc-9cae1c53a38b"),
                    "RaiseOnInstantiated",
                    () =>
                        new NServiceBusStudio.Automation.Commands.RaisesOnInstantiatedComponent
                        {
                        });

            var ReplyCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("978e9f5e-74d7-47cc-9724-1b3c3efc4220"),
                    "ReplyCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowReplyWithPicker
                        {
                        });

            var SelectSagaStarter =
                element.CreateOrUpdateCommandSettings(
                    new Guid("ef04bcff-4db1-4b31-b84d-dbece3964571"),
                    "SelectSagaStarter",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowComponentSagaStarterPicker
                        {
                        });

            var SendCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("72a376e2-aab4-498b-9448-3544ffc8790a"),
                    "SendCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowCommandTypePicker
                        {
                        });

            var SetUnfoldedCustomCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("2705891b-8aa1-4a03-b335-5d83c3c2693a"),
                    "SetUnfoldedCustomCode",
                    () =>
                        new NServiceBusStudio.Automation.Commands.SetUnfoldedCustomCode
                        {
                        });

            var ShowComponentReferencePicker =
                element.CreateOrUpdateCommandSettings(
                    new Guid("b8293535-22b5-4e2a-90e3-d60fa296b4c3"),
                    "ShowComponentReferencePicker",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowComponentReferencePicker
                        {
                        });

            var SubscribeCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("3e8f946a-4da7-4322-8a92-83e4ea41ec48"),
                    "SubscribeCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowEventSubscriptionPicker
                        {
                        });

            var UnfoldCustomHandlers =
                element.CreateOrUpdateCommandSettings(
                    new Guid("36939d09-f7c3-45f5-881c-3908f1476ab4"),
                    "UnfoldCustomHandlers",
                    () =>
                        new NuPattern.Library.Commands.GenerateComponentCodeCommand
                        {
                            CheckIsDeployed = BindingFor.Value<bool>("True"),
                            CheckIsNotUnfoldedCustomCode = BindingFor.Value<bool>("True"),
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NServiceBusStudio.Automation.ValueProviders.GetEndpointPathValueProvider
                                {
                                    AddInfrastructureFolder = BindingFor.Value<bool>("False"),
                                }),
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/CustomComponentHandlers.tt"),
                        });

            var UnfoldHandlers =
                element.CreateOrUpdateCommandSettings(
                    new Guid("0fa0f1e3-2a8d-4e12-b629-9012e20354ad"),
                    "UnfoldHandlers",
                    () =>
                        new NuPattern.Library.Commands.GenerateComponentCodeCommand
                        {
                            CheckIsDeployed = BindingFor.Value<bool>("True"),
                            CheckIsNotUnfoldedCustomCode = BindingFor.Value<bool>("False"),
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NServiceBusStudio.Automation.ValueProviders.GetEndpointPathValueProvider
                                {
                                    AddInfrastructureFolder = BindingFor.Value<bool>("True"),
                                }),
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/ComponentHandlers.tt"),
                        });

            var UnfoldSagaConfigureHowToFindCodeV4 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("8938e043-f81d-49a7-8c9b-35c02b437c67"),
                    "UnfoldSagaConfigureHowToFindCodeV4",
                    () =>
                        new NuPattern.Library.Commands.GenerateComponentCodeCommand
                        {
                            CheckIsDeployed = BindingFor.Value<bool>("True"),
                            CheckIsNotUnfoldedCustomCode = BindingFor.Value<bool>("False"),
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}ConfigureHowToFindSaga.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NServiceBusStudio.Automation.ValueProviders.GetEndpointPathValueProvider
                                {
                                    AddInfrastructureFolder = BindingFor.Value<bool>("False"),
                                }),
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/CustomComponentSagaConfigureHowToFind.v4.tt"),
                        });

            var UnfoldSagaConfigureHowToFindCodeV5 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("708c7729-43e8-4da2-a9dd-da57d5634b7d"),
                    "UnfoldSagaConfigureHowToFindCodeV5",
                    () =>
                        new NuPattern.Library.Commands.GenerateComponentCodeCommand
                        {
                            CheckIsDeployed = BindingFor.Value<bool>("True"),
                            CheckIsNotUnfoldedCustomCode = BindingFor.Value<bool>("False"),
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}ConfigureHowToFindSaga.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NServiceBusStudio.Automation.ValueProviders.GetEndpointPathValueProvider
                                {
                                    AddInfrastructureFolder = BindingFor.Value<bool>("False"),
                                }),
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/CustomComponentSagaConfigureHowToFind.v5.tt"),
                        });

            var UnfoldSagaDataCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("b2adde29-7d8c-4c77-9640-3aaa7ba96752"),
                    "UnfoldSagaDataCode",
                    () =>
                        new NuPattern.Library.Commands.GenerateComponentCodeCommand
                        {
                            CheckIsDeployed = BindingFor.Value<bool>("True"),
                            CheckIsNotUnfoldedCustomCode = BindingFor.Value<bool>("False"),
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}SagaData.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NServiceBusStudio.Automation.ValueProviders.GetEndpointPathValueProvider
                                {
                                    AddInfrastructureFolder = BindingFor.Value<bool>("False"),
                                }),
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/CustomComponentSagaHandlers.tt"),
                        });

            var ConfigureSagaCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "ConfigureSagaCommand",
                    SelectSagaStarter,
                    OpenConfigureHowToFindSaga);

            var GenerateCodeV4Handlers =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "GenerateCodeV4Handlers",
                    UnfoldHandlers,
                    UnfoldCustomHandlers,
                    SetUnfoldedCustomCode);

            var GenerateCodeV5Handlers =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "GenerateCodeV5Handlers",
                    UnfoldHandlers,
                    UnfoldCustomHandlers,
                    SetUnfoldedCustomCode);

            var IsSagaEqualsTrueCommandV4 =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "IsSagaEqualsTrueCommandV4",
                    UnfoldSagaDataCode,
                    UnfoldSagaConfigureHowToFindCodeV4);

            var IsSagaEqualsTrueCommandV5 =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "IsSagaEqualsTrueCommandV5",
                    UnfoldSagaDataCode,
                    UnfoldSagaConfigureHowToFindCodeV5);

            var OnInstantiatedCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiatedCommand",
                    RaiseOnInstantiated);

            var OpenCommandV4 =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OpenCommandV4",
                    GenerateCodeV4Handlers,
                    OpenCustomCodeFile);

            var OpenCommandV5 =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OpenCommandV5",
                    IsSagaEqualsTrueCommandV5,
                    OpenCustomCodeFile);

            // commands referenced by automation  
            // - ConfigureSagaCommand
            // - DeployToCommand
            // - IsSagaEqualsTrueCommandV4
            // - IsSagaEqualsTrueCommandV5
            // - OnInstantiatedCommand
            // - OpenCommandV4
            // - OpenCommandV5
            // - OpenCustomCodeFile
            // - PublishCommand
            // - ReplyCommand
            // - SendCommand
            // - ShowComponentReferencePicker
            // - SubscribeCommand
        }

        private static void Setup_ComponentLink_58bd0df25250472da1694d72726325ae_Automation(IPatternElementSchema element)
        {
            //var OpenComponentCode =
            //    element.CreateOrUpdateCommandSettings(
            //        new Guid("6be99fc8-1ae6-4933-b7bd-6ebfe9f9d5aa"),
            //        "OpenComponentCode",
            //        () =>
            //            new NServiceBusHost.Automation.Commands.OpenComponentCodeCommand
            //            {
            //            });

            //var UnfoldDiagramFile =
            //    element.CreateOrUpdateCommandSettings(
            //        new Guid("0531cb25-41cb-4753-88e2-f1e7239cbc7a"),
            //        "UnfoldDiagramFile",
            //        () =>
            //            new NServiceBusHost.Automation.Commands.ExecuteUnfoldEndpointDiagram
            //            {
            //            });

            // commands referenced by automation  
            // - OpenComponentCode
            // - UnfoldDiagramFile
        }

        private static void Setup_ComponentLink_813d58eb16cd43349bfb6ebbc5dc7ee6_Automation(IPatternElementSchema element)
        {
            var OpenComponentCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("2e149ab4-4132-44a5-9cc6-03fffc6b01d5"),
                    "OpenComponentCode",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBWeb.OpenComponentCodeCommand
                        {
                        });

            var UnfoldDiagramFile =
                element.CreateOrUpdateCommandSettings(
                    new Guid("30f05b9e-8643-4684-a1ec-5e8b6c92bd87"),
                    "UnfoldDiagramFile",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.ExecuteUnfoldEndpointDiagram
                        {
                        });

            // commands referenced by automation  
            // - OpenComponentCode
            // - UnfoldDiagramFile
        }

        private static void Setup_ComponentLink_18db2bf834144c909e4639e0475a018b_Automation(IPatternElementSchema element)
        {
            var OpenComponentCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("58349be1-1440-4209-bfa3-253acd195d02"),
                    "OpenComponentCode",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBMVC.OpenComponentCodeCommand
                        {
                        });

            var UnfoldDiagramFile =
                element.CreateOrUpdateCommandSettings(
                    new Guid("35c41611-4be4-45ec-8cd0-0ca3bca21446"),
                    "UnfoldDiagramFile",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.ExecuteUnfoldEndpointDiagram
                        {
                        });

            // commands referenced by automation  
            // - OpenComponentCode
            // - UnfoldDiagramFile
        }

        private static void Setup_Components_4640ec7cc09c42a690deff9cd99fd6b1_Automation(IPatternElementSchema element)
        {
            var ShowComponentLinkDialog =
                element.CreateOrUpdateCommandSettings(
                    new Guid("5673e075-0cf9-4be4-90df-ab5af5e5f62c"),
                    "ShowComponentLinkDialog",
                    () =>
                        new AbstractEndpoint.Automation.Commands.ShowComponentLinkPicker
                        {
                        });

            // commands referenced by automation  
            // - ShowComponentLinkDialog
        }

        private static void Setup_Components_7fd9877f4de14d74a0a3d3a09cc06a73_Automation(IPatternElementSchema element)
        {
            var ShowComponentLinkDialog =
                element.CreateOrUpdateCommandSettings(
                    new Guid("31c6940e-2946-4c99-85a8-befeb4bed22e"),
                    "ShowComponentLinkDialog",
                    () =>
                        new AbstractEndpoint.Automation.Commands.ShowComponentLinkPicker
                        {
                        });

            // commands referenced by automation  
            // - ShowComponentLinkDialog
        }

        private static void Setup_Components_2cb77a1ff887467cb0e3df2de031b87c_Automation(IPatternElementSchema element)
        {
            var ShowComponentLinkDialog =
                element.CreateOrUpdateCommandSettings(
                    new Guid("2e90a7c8-65ba-4419-80b1-a24552bb7848"),
                    "ShowComponentLinkDialog",
                    () =>
                        new AbstractEndpoint.Automation.Commands.ShowComponentLinkPicker
                        {
                        });

            // commands referenced by automation  
            // - ShowComponentLinkDialog
        }

        private static void Setup_ContractsProject_Automation(IPatternElementSchema element)
        {
            var InstallNuGetPackages =
                element.CreateOrUpdateCommandSettings(
                    new Guid("aafa86ef-7796-447d-a33a-6b8290bd832a"),
                    "InstallNuGetPackages",
                    () =>
                        new NServiceBusStudio.Automation.Commands.AddInfrastructureProjectReferences
                        {
                        });

            var UnfoldProject =
                element.CreateOrUpdateCommandSettings(
                    new Guid("81aa53e1-760a-4798-9c5c-6c96227fb32c"),
                    "UnfoldProject",
                    () =>
                        new NuPattern.Library.Commands.UnfoldVsTemplateCommand
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetFileName = BindingFor.ValueProvider<string>(
                                new NuPattern.Library.ValueProviders.ExpressionValueProvider
                                {
                                    Expression = "{InstanceName}",
                                }),
                            TargetPath = "",
                            TemplateUri = BindingFor.Value<Uri>("template://project/CSharp/a31ec8d9600f-Application.Design.ContractsProject"),
                        });

            var InstantiateCommands =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "InstantiateCommands",
                    UnfoldProject,
                    InstallNuGetPackages);

            // commands referenced by automation  
            // - InstantiateCommands
        }

        private static void Setup_Endpoints_Automation(IPatternElementSchema element)
        {
            var DeployUnhostedComponentsCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("db53572c-43ed-4c33-a456-e61c864fe0ba"),
                    "DeployUnhostedComponentsCommand",
                    () =>
                        new AbstractEndpoint.Automation.Commands.ShowDeployUnhostedComponentsPicker
                        {
                        });

            // commands referenced by automation  
            // - DeployUnhostedComponentsCommand
        }

        private static void Setup_Event_Automation(IPatternElementSchema element)
        {
            var AddSubscriberCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("342de258-1635-4a98-9b5c-05c86921714b"),
                    "AddSubscriberCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowSubscriberPicker
                        {
                        });

            var ExGenerateCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("bedce582-4714-46bd-9def-d09e712ddab3"),
                    "ExGenerateCode",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NuPattern.Library.ValueProviders.ExpressionValueProvider
                                {
                                    Expression = "{Root.InstanceName}.{Root.ProjectNameContracts}\\GeneratedCode\\{Parent.Parent.Parent.InstanceName}",
                                }),
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/EventDefinition.tt"),
                        });

            var OpenCodeFile =
                element.CreateOrUpdateCommandSettings(
                    new Guid("bb6ccc74-ad9f-4e2f-b90b-e2ac1e3b2204"),
                    "OpenCodeFile",
                    () =>
                        new NuPattern.Library.Commands.ActivateArtifactCommand
                        {
                            Open = BindingFor.Value<bool>("True"),
                        });

            var UnfoldEventFile =
                element.CreateOrUpdateCommandSettings(
                    new Guid("0757e840-a92f-4f7c-ade6-a9bc878796d4"),
                    "UnfoldEventFile",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NuPattern.Library.ValueProviders.ExpressionValueProvider
                                {
                                    Expression = "{Root.InstanceName}.{Root.ProjectNameContracts}\\{Parent.Parent.Parent.InstanceName}",
                                }),
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/CustomEventDefinition.tt"),
                        });

            var OnInstantiateCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateCommand",
                    UnfoldEventFile);

            var UnfoldCodeCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "UnfoldCodeCommand",
                    UnfoldEventFile);

            // commands referenced by automation  
            // - AddSubscriberCommand
            // - OnInstantiateCommand
            // - OpenCodeFile
        }

        private static void Setup_EventLink_Automation(IPatternElementSchema element)
        {
            var AddEndpointProjectReferences =
                element.CreateOrUpdateCommandSettings(
                    new Guid("6d30de92-1971-4167-9d32-fef38994085e"),
                    "AddEndpointProjectReferences",
                    () =>
                        new NServiceBusStudio.Automation.Commands.AddEndpointProjectReferences
                        {
                        });

            var OnInstantiateCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateCommand",
                    AddEndpointProjectReferences);

            // commands referenced by automation  
            // - OnInstantiateCommand
        }

        private static void Setup_Infrastructure_Automation(IPatternElementSchema element)
        {
            var AddAuthenticationCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("48b725fa-ffcc-46a9-8324-75f7d6e87c7e"),
                    "AddAuthenticationCommand",
                    () =>
                        new NServiceBusStudio.Automation.Infrastructure.Authentication.AddAuthenticationCommand
                        {
                        });

            var GenerateProjectCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("ce3e2712-139a-4703-aff3-43c8cb3bfb65"),
                    "GenerateProjectCommand",
                    () =>
                        new NuPattern.Library.Commands.UnfoldVsTemplateCommand
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetFileName = "{Parent.Parent.InstanceName}.Infrastructure",
                            TargetPath = "",
                            TemplateUri = BindingFor.Value<Uri>("template://project/CSharp/7c13f051a9fc-Application.Design.Infrastructure"),
                        });

            var InstallNuGetPackages =
                element.CreateOrUpdateCommandSettings(
                    new Guid("68cd8225-7eee-4960-aa6e-ae7870db24e9"),
                    "InstallNuGetPackages",
                    () =>
                        new NServiceBusStudio.Automation.Commands.AddInfrastructureProjectReferences
                        {
                        });

            // commands referenced by automation  
            // - AddAuthenticationCommand
        }

        private static void Setup_InternalMessagesProject_Automation(IPatternElementSchema element)
        {
            var InstallNuGetPackages =
                element.CreateOrUpdateCommandSettings(
                    new Guid("09591e8d-4531-4085-8494-c9fee80a976f"),
                    "InstallNuGetPackages",
                    () =>
                        new NServiceBusStudio.Automation.Commands.AddInfrastructureProjectReferences
                        {
                        });

            var UnfoldProject =
                element.CreateOrUpdateCommandSettings(
                    new Guid("ec5a8e4b-3562-4f18-b490-a2f881905d27"),
                    "UnfoldProject",
                    () =>
                        new NuPattern.Library.Commands.UnfoldVsTemplateCommand
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetFileName = BindingFor.ValueProvider<string>(
                                new NuPattern.Library.ValueProviders.ExpressionValueProvider
                                {
                                    Expression = "{InstanceName}",
                                }),
                            TargetPath = "",
                            TemplateUri = BindingFor.Value<Uri>("template://project/CSharp/b206ddeb388a-Application.Design.InternalMessagesProject"),
                        });

            var InstantiateCommands =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "InstantiateCommands",
                    UnfoldProject,
                    InstallNuGetPackages);

            // commands referenced by automation  
            // - InstantiateCommands
        }

        private static void Setup_Libraries_Automation(IPatternElementSchema element)
        {
            var AddLibraryProjectReferences =
                element.CreateOrUpdateCommandSettings(
                    new Guid("40735865-b53c-4880-8ef2-f31dd9a9a581"),
                    "AddLibraryProjectReferences",
                    () =>
                        new NServiceBusStudio.Automation.Commands.AddLibraryProjectReferences
                        {
                        });

            var InstallNuGetPackages =
                element.CreateOrUpdateCommandSettings(
                    new Guid("952f230b-dc48-4d68-8f5f-81b9801123fb"),
                    "InstallNuGetPackages",
                    () =>
                        new NServiceBusStudio.Automation.Commands.AddInfrastructureProjectReferences
                        {
                        });

            var UnfordLibrariesTemplate =
                element.CreateOrUpdateCommandSettings(
                    new Guid("5e6b5fec-870f-4f44-9a76-d9ffbdf9f49d"),
                    "UnfordLibrariesTemplate",
                    () =>
                        new NuPattern.Library.Commands.UnfoldVsTemplateCommand
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetFileName = "{Parent.Parent.InstanceName}.{Root.ProjectNameCode}",
                            TargetPath = "",
                            TemplateUri = BindingFor.Value<Uri>("template://project/CSharp/485e80115c58-Application.Design.Libraries"),
                        });

            var OnInstantiatedCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiatedCommand",
                    InstallNuGetPackages);

            var UnfoldLibrariesProject =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "UnfoldLibrariesProject",
                    UnfordLibrariesTemplate,
                    AddLibraryProjectReferences,
                    InstallNuGetPackages);

            // commands referenced by automation  
            // - OnInstantiatedCommand
        }

        private static void Setup_Library_Automation(IPatternElementSchema element)
        {
            var CheckForProject =
                element.CreateOrUpdateCommandSettings(
                    new Guid("a8863083-9d34-4cba-98d6-ef0895998733"),
                    "CheckForProject",
                    () =>
                        new NServiceBusStudio.Automation.Commands.LibraryAddedCommand
                        {
                        });

            var OnActivationCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("1f795dcd-0bb5-4f76-95f4-ac2c8b1a7800"),
                    "OnActivationCommand",
                    () =>
                        new NuPattern.Library.Commands.ActivateArtifactCommand
                        {
                            Open = BindingFor.Value<bool>("True"),
                        });

            var UnfoldLibraryCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("cb26f4c4-865f-4bd7-8260-b04f8ab85b6c"),
                    "UnfoldLibraryCode",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}.cs",
                            TargetPath = "{FilePath}",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Library.tt"),
                        });

            var OnInstantiatedCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiatedCommand",
                    CheckForProject,
                    UnfoldLibraryCode);

            // commands referenced by automation  
            // - OnActivationCommand
            // - OnInstantiatedCommand
        }

        private static void Setup_LibraryReference_Automation(IPatternElementSchema element)
        {
            var OnDeletedCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("2bbc9968-2571-4db4-827a-9b554fc0ddbb"),
                    "OnDeletedCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.RemoveLibraryCodeLinkCommand
                        {
                        });

            var OnInstantiatedCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("d8b82ee4-5d50-42fb-988a-53eb54cbf127"),
                    "OnInstantiatedCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.CreateLibraryCodeLinkCommand
                        {
                        });

            // commands referenced by automation  
            // - OnDeletedCommand
            // - OnInstantiatedCommand
        }

        private static void Setup_LibraryReferences_Automation(IPatternElementSchema element)
        {
            var CreateLibraryCodeLinks =
                element.CreateOrUpdateCommandSettings(
                    new Guid("7d8db3c0-ecf0-4b71-896b-155ac6be7359"),
                    "CreateLibraryCodeLinks",
                    () =>
                        new NServiceBusStudio.Automation.Commands.CreateLibraryCodeLinksCommand
                        {
                        });

            var RemoveLibraryCodeLinks =
                element.CreateOrUpdateCommandSettings(
                    new Guid("f2ebb7f9-e048-43d5-8d1f-1db2ebc03bfc"),
                    "RemoveLibraryCodeLinks",
                    () =>
                        new NServiceBusStudio.Automation.Commands.RemoveLibraryCodeLinksCommand
                        {
                        });

            // commands referenced by automation  
        }

        private static void Setup_Message_Automation(IPatternElementSchema element)
        {
            var OpenMessageFile =
                element.CreateOrUpdateCommandSettings(
                    new Guid("be4bb218-fc37-41ad-ad65-0b496dfa8bfc"),
                    "OpenMessageFile",
                    () =>
                        new NuPattern.Library.Commands.ActivateArtifactCommand
                        {
                            Open = BindingFor.Value<bool>("True"),
                            //TargetFileName = "",
                            //TargetPath = "",
                        });

            var UnfoldMessageFile =
                element.CreateOrUpdateCommandSettings(
                    new Guid("b12110b5-ccbe-41de-8939-88f03ed42017"),
                    "UnfoldMessageFile",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("True"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}.cs",
                            TargetPath = BindingFor.ValueProvider<string>(
                                new NuPattern.Library.ValueProviders.ExpressionValueProvider
                                {
                                    Expression = "{Root.InstanceName}.{Root.ProjectNameInternalMessages}\\{Parent.Parent.Parent.InstanceName}",
                                }),
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/CustomMessageDefinition.tt"),
                        });

            var OnInstantiateCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateCommand",
                    UnfoldMessageFile);

            // commands referenced by automation  
            // - OnInstantiateCommand
            // - OpenMessageFile
        }

        private static void Setup_NServiceBusHost_Automation(IPatternElementSchema element)
        {
            var AddAuthenticationCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("39746ce7-26a0-4946-9ba5-1b0c3c847099"),
                    "AddAuthenticationCommand",
                    () =>
                        new NServiceBusStudio.Automation.Infrastructure.Authentication.AddAuthenticationCommand
                        {
                        });

            var AddProjectReferences =
                element.CreateOrUpdateCommandSettings(
                    new Guid("3e692b85-1fa6-48e5-8883-4247848912af"),
                    "AddProjectReferences",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.AddEndpointProjectReferences
                        {
                        });

            var GenerateAuditConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("1decf8a2-8fda-4ca0-9410-a54735fde0c9"),
                    "GenerateAuditConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("True"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "AuditConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config/",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/AuditConfig.config.tt"),
                        });

            var GenerateDefaultAppConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("45fca2b9-4ca6-47d5-a720-54ca0239e02a"),
                    "GenerateDefaultAppConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "App.config",
                            TargetPath = "",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/DefaultApp.config.tt"),
                        });

            var GenerateEndpointConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("163abd93-cedc-4e41-8a7d-67eedfac41aa"),
                    "GenerateEndpointConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "EndpointConfig.generated.cs",
                            TargetPath = "~/EndpointConfig.cs",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/EndpointConfig.generated.cs.tt"),
                        });

            var GenerateMasterNodeConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("0c7a14be-6b82-476d-ac17-a46f8871d884"),
                    "GenerateMasterNodeConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "MasterNodeConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/MasterNodeConfig.config.tt"),
                        });

            var GenerateMessageForwardingInCaseOfFaultConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("a99928a4-309c-4c2f-ab3f-0459aed6a3c6"),
                    "GenerateMessageForwardingInCaseOfFaultConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "MessageForwardingInCaseOfFaultConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/MessageForwardingInCaseOfFaultConfig.config.tt"),
                        });

            var GenerateSecondLevelRetriesConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("61a98a15-e465-4876-96f9-e3079b9d03f8"),
                    "GenerateSecondLevelRetriesConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "SecondLevelRetriesConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/SecondLevelRetriesConfig.config.tt"),
                        });

            var GenerateTransportConfigCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("15F56D6E-DC15-44F2-BB6B-91604C1CDA08"),
                    "GenerateTransportConfigCode",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "TransportConfig.cs",
                            TargetPath = "~",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/TransportConfig.tt"),
                        });

            var GenerateTransportConfigConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("24589f3f-059f-4d5a-9b9b-a3897fff2449"),
                    "GenerateTransportConfigConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "TransportConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/TransportConfig.config.tt"),
                        });

            var GenerateTransportConnectionStringConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("99be7198-69a8-4174-bb1e-35190f5b6496"),
                    "GenerateTransportConnectionStringConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "TransportConnectionString.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/TransportConnectionString.config.tt"),
                        });

            var GenerateUnicastBusConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("e4f4bcc6-41e9-4237-be6f-7fd91e691ced"),
                    "GenerateUnicastBusConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "UnicastBusConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/UnicastBusConfig.config.tt"),
                        });

            var InstallNuGetPackages =
                element.CreateOrUpdateCommandSettings(
                    new Guid("a2fc8c0a-8aec-4aa2-b67a-d1dfbf662bac"),
                    "InstallNuGetPackages",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.AddNugetReferencesCommand
                        {
                            IgnoreHost = BindingFor.Value<bool>("False"),
                        });

            var ProcessAfterUnfoldedProject =
                element.CreateOrUpdateCommandSettings(
                    new Guid("66faa173-534b-4ca4-a662-b78085d7fc78"),
                    "ProcessAfterUnfoldedProject",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.ProcessRootAfterUnfoldedProject
                        {
                        });

            var PublishEventCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("20a6e3e7-d8f7-4fdf-8087-e6aeff8decbe"),
                    "PublishEventCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowEventComponentPicker
                        {
                        });

            var RaisesOnInstantiated =
                element.CreateOrUpdateCommandSettings(
                    new Guid("f11f5ae6-fa07-49e5-b192-6f360ddcf9f8"),
                    "RaisesOnInstantiated",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.RaiseOnInstantiated
                        {
                        });

            var SendCommandCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("cffe2ae5-e4d9-4411-82fa-1d3d2fc9c4ba"),
                    "SendCommandCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowCommandComponentPicker
                        {
                        });

            var UnfoldEndpointConfig =
                element.CreateOrUpdateDispatcherCommandSettings(
                    "UnfoldEndpointConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("True"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = BindingFor.Value<string>(""),
                            TargetBuildAction = BindingFor.Value<string>(""),
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = BindingFor.Value<string>("EndpointConfig.cs"),
                            TargetPath = BindingFor.Value<string>("~"),
                        },
                    Tuple.Create(
                        new Guid("f0d12193-eea0-4677-bd94-cde70b531417"),
                        "NSB 4.0",
                        "t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/EndpointConfig.v4.cs.tt"),
                    Tuple.Create(
                        new Guid("2f27bba1-783b-4f38-85bd-5d3c3bc08493"),
                        "NSB 5.0",
                        "t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/EndpointConfig.v5.cs.tt"));

            var UnfoldLoggingConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("f640e8f1-1189-43f8-8d95-3bbd26ce574d"),
                    "UnfoldLoggingConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "Logging.config",
                            TargetPath = "~/Infrastructure",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/Logging.config.tt"),
                        });

            var UnfoldMessageConventions =
                element.CreateOrUpdateDispatcherCommandSettings(
                    "UnfoldMessageConventions",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "MessageConventions.cs",
                            TargetPath = "~/Infrastructure",
                        },
                    Tuple.Create(
                        new Guid("68894bb3-5b54-41e0-8972-b4186984c290"),
                        "NSB 4.0",
                        "t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/MessageConventions.v4.cs.tt"),
                    Tuple.Create(
                        new Guid("18bee843-361d-4180-b033-27e4f370aecb"),
                        "NSB 5.0",
                        "t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBH/MessageConventions.v5.cs.tt"));

            var UnfoldProject =
                element.CreateOrUpdateCommandSettings(
                    new Guid("6e5255cd-6f4a-4583-a9b1-9be69ef1cbd5"),
                    "UnfoldProject",
                    () =>
                        new NuPattern.Library.Commands.UnfoldVsTemplateCommand
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetFileName = "{Root.InstanceName}.{InstanceName}",
                            TargetPath = "",
                            TemplateUri = BindingFor.Value<Uri>("template://project/CSharp/3e6035065cf5-Application.Design.Endpoints.NServiceBusHost"),
                        });

            var GenerateCodeConfigSections =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "GenerateCodeConfigSections",
                    GenerateMasterNodeConfig,
                    GenerateMessageForwardingInCaseOfFaultConfig,
                    GenerateTransportConfigConfig,
                    GenerateUnicastBusConfig,
                    GenerateEndpointConfig,
                    GenerateTransportConfigCode,
                    GenerateTransportConnectionStringConfig,
                    GenerateSecondLevelRetriesConfig,
                    GenerateAuditConfig);

            var UnfoldCodeCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "UnfoldCodeCommand",
                    GenerateCodeConfigSections,
                    AddProjectReferences,
                    GenerateEndpointConfig,
                    GenerateDefaultAppConfig);

            var OnInstantiateCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateCommand",
                    UnfoldProject,
                    InstallNuGetPackages,
                    UnfoldMessageConventions,
                    UnfoldLoggingConfig,
                    UnfoldEndpointConfig,
                    UnfoldCodeCommand,
                    RaisesOnInstantiated,
                    ProcessAfterUnfoldedProject);

            // commands referenced by automation  
            // - AddAuthenticationCommand
            // - OnInstantiateV4Command
            // - OnInstantiateV5Command
            // - PublishEventCommand
            // - SendCommandCommand
        }

        private static void Setup_NServiceBusMVC_Automation(IPatternElementSchema element)
        {
            var AddAuthenticationCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("20ff6e2a-5a97-48d7-8fb7-6d001d744860"),
                    "AddAuthenticationCommand",
                    () =>
                        new NServiceBusStudio.Automation.Infrastructure.Authentication.AddAuthenticationCommand
                        {
                        });

            var AddProjectReferences =
                element.CreateOrUpdateCommandSettings(
                    new Guid("803c9b2d-3d78-4c53-acf6-57b57d202930"),
                    "AddProjectReferences",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.AddEndpointProjectReferences
                        {
                        });

            var GenerateAuditConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("8fb1dadd-4c90-456f-a54c-0ec5df9d9a9a"),
                    "GenerateAuditConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("True"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "AuditConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config/",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/AuditConfig.config.tt"),
                        });

            var GenerateMessageForwardingConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("338298e4-094c-4fa3-bb2e-1eb7d8e1097e"),
                    "GenerateMessageForwardingConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "MessageForwardingInCaseOfFaultConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/MessageForwardingInCaseOfFaultConfig.config.tt"),
                        });

            var GenerateTestMessagesController =
                element.CreateOrUpdateCommandSettings(
                    new Guid("7459da8d-a7e6-4547-8019-cbebb69ee7f0"),
                    "GenerateTestMessagesController",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "TestMessagesController.generated.cs",
                            TargetPath = "~/Controllers",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/TestMessagesController.cs.tt"),
                        });

            var GenerateTestMessagesView =
                element.CreateOrUpdateCommandSettings(
                    new Guid("e79472a8-53c0-4f08-9d5e-4498f6fab3b4"),
                    "GenerateTestMessagesView",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "Index.cshtml",
                            TargetPath = "~/Views/TestMessages",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/TestMessagesView.cshtml.tt"),
                        });

            var GenerateTransportConfigCodeV4 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("ceb76ec1-66d0-43d7-883c-524960335c61"),
                    "GenerateTransportConfigCodeV4",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "TransportConfig.cs",
                            TargetPath = "~/Infrastructure/GeneratedCode",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/TransportConfig.v4.tt"),
                        });

            var GenerateTransportConfigCodeV5 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("58653a3f-6329-4ab4-aac3-6f1bbe199a15"),
                    "GenerateTransportConfigCodeV5",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "TransportConfig.cs",
                            TargetPath = "~/Infrastructure/GeneratedCode",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/TransportConfig.v5.tt"),
                        });

            var GenerateTransportConfigConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("71d2d518-0a28-43c8-97bc-b3a1daa1d044"),
                    "GenerateTransportConfigConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "TransportConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/TransportConfig.config.tt"),
                        });

            var GenerateTransportConnectionStringConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("bb9daf67-ea65-4b21-a8d9-bc4a84536d2e"),
                    "GenerateTransportConnectionStringConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "TransportConnectionString.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/TransportConnectionString.config.tt"),
                        });

            var GenerateUnicastBusConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("f1d05d84-fb55-49f4-8bac-0b7b21615f9e"),
                    "GenerateUnicastBusConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "UnicastBusConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/UnicastBusConfig.config.tt"),
                        });

            var GenerateWebGlobalInitializationV4 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("87d1c6b4-d238-4a4a-9436-e8dabc2846f3"),
                    "GenerateWebGlobalInitializationV4",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "WebGlobalInitialization.cs",
                            TargetPath = "~/Infrastructure/",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/WebGlobalInitialization.v4.cs.tt"),
                        });

            var GenerateWebGlobalInitializationV5 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("e5bb4f8e-1d26-42dd-b108-ae23e184a546"),
                    "GenerateWebGlobalInitializationV5",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "WebGlobalInitialization.cs",
                            TargetPath = "~/Infrastructure/",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/WebGlobalInitialization.v5.cs.tt"),
                        });

            var GenerateWebInitializationV4 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("55056fbd-f4ca-46fb-8c77-0f54707eaf66"),
                    "GenerateWebInitializationV4",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "GeneratedWebInitialization.cs",
                            TargetPath = "~/Infrastructure/GeneratedCode",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/GeneratedWebInitialization.v4.cs.tt"),
                        });

            var GenerateWebInitializationV5 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("24512a68-85ba-4bb9-9a57-29d35dbbf360"),
                    "GenerateWebInitializationV5",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "GeneratedWebInitialization.cs",
                            TargetPath = "~/Infrastructure/GeneratedCode",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/GeneratedWebInitialization.v5.cs.tt"),
                        });

            var InstallNuGetPackages =
                element.CreateOrUpdateCommandSettings(
                    new Guid("028b4628-fc8e-409c-a743-7be007bcd645"),
                    "InstallNuGetPackages",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.AddNugetReferencesCommand
                        {
                            IgnoreHost = BindingFor.Value<bool>("True"),
                        });

            var ProcessAfterUnfoldingProject =
                element.CreateOrUpdateCommandSettings(
                    new Guid("80776373-8131-4f13-bf2f-43fce486c245"),
                    "ProcessAfterUnfoldingProject",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.ProcessRootAfterUnfoldedProject
                        {
                        });

            var PublishEventCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("8e1a6a25-7063-4543-973f-26203721274b"),
                    "PublishEventCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowEventComponentPicker
                        {
                        });

            var RaiseOnInstantiated =
                element.CreateOrUpdateCommandSettings(
                    new Guid("42fbe5dc-3cc8-4347-8842-f83c2765d6b2"),
                    "RaiseOnInstantiated",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.RaiseOnInstantiated
                        {
                        });

            var SendCommandCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("dd6f1bee-4a1b-410d-951c-929648b6b2b5"),
                    "SendCommandCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowCommandComponentPicker
                        {
                        });

            var UnfoldLoggingConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("4dd3672b-f688-467d-8462-b7073b8e4ebf"),
                    "UnfoldLoggingConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "Logging.config",
                            TargetPath = "~/Infrastructure",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/Logging.config.tt"),
                        });

            var UnfoldMessageConventionsV4 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("60149b6b-52bd-4fac-9259-1a8ba480851a"),
                    "UnfoldMessageConventionsV4",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "MessageConventions.cs",
                            TargetPath = "~/Infrastructure",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/MessageConventions.v4.cs.tt"),
                        });

            var UnfoldMessageConventionsV5 =
                element.CreateOrUpdateCommandSettings(
                    new Guid("6325016c-d508-4d4d-9d83-6e02efa7cf95"),
                    "UnfoldMessageConventionsV5",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "MessageConventions.cs",
                            TargetPath = "~/Infrastructure",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBMVC/MessageConventions.v5.cs.tt"),
                        });

            var UnfoldProject =
                element.CreateOrUpdateCommandSettings(
                    new Guid("8cd30cfb-d028-480d-82df-61d6b009df17"),
                    "UnfoldProject",
                    () =>
                        new NuPattern.Library.Commands.UnfoldVsTemplateCommand
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetFileName = "{Root.InstanceName}.{InstanceName}",
                            TargetPath = "",
                            TemplateUri = BindingFor.Value<Uri>("template://project/CSharp/8266b53c7228-Application.Design.Endpoints.NServiceBusMVC"),
                        });

            var GenerateCodeV4ConfigSection =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "GenerateCodeV4ConfigSection",
                    GenerateMessageForwardingConfig,
                    GenerateTransportConfigConfig,
                    GenerateUnicastBusConfig,
                    GenerateWebInitializationV4,
                    GenerateWebGlobalInitializationV4,
                    GenerateTransportConfigCodeV4,
                    GenerateTransportConnectionStringConfig,
                    GenerateAuditConfig,
                    GenerateTestMessagesController,
                    GenerateTestMessagesView);

            var GenerateCodeV5ConfigSection =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "GenerateCodeV5ConfigSection",
                    GenerateMessageForwardingConfig,
                    GenerateTransportConfigConfig,
                    GenerateUnicastBusConfig,
                    GenerateWebInitializationV5,
                    GenerateWebGlobalInitializationV5,
                    GenerateTransportConfigCodeV5,
                    GenerateTransportConnectionStringConfig,
                    GenerateAuditConfig,
                    GenerateTestMessagesController,
                    GenerateTestMessagesView);

            var OnInstantiateV4Command =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateV4Command",
                    UnfoldProject,
                    InstallNuGetPackages,
                    AddProjectReferences,
                    UnfoldMessageConventionsV4,
                    UnfoldLoggingConfig,
                    GenerateCodeV4ConfigSection,
                    RaiseOnInstantiated,
                    ProcessAfterUnfoldingProject);

            var OnInstantiateV5Command =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateV5Command",
                    UnfoldProject,
                    InstallNuGetPackages,
                    AddProjectReferences,
                    UnfoldMessageConventionsV5,
                    UnfoldLoggingConfig,
                    GenerateCodeV5ConfigSection,
                    RaiseOnInstantiated,
                    ProcessAfterUnfoldingProject);

            // commands referenced by automation  
            // - AddAuthenticationCommand
            // - OnInstantiateV4Command
            // - OnInstantiateV5Command
            // - PublishEventCommand
            // - SendCommandCommand
        }

        private static void Setup_NServiceBusWeb_Automation(IPatternElementSchema element)
        {
            var AddAuthenticationCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("14a17875-0b8e-44db-85c5-9046ebfe940f"),
                    "AddAuthenticationCommand",
                    () =>
                        new NServiceBusStudio.Automation.Infrastructure.Authentication.AddAuthenticationCommand
                        {
                        });

            var AddProjectReferences =
                element.CreateOrUpdateCommandSettings(
                    new Guid("6bf6c994-3b22-4d89-b93c-f5cfc2077d74"),
                    "AddProjectReferences",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.AddEndpointProjectReferences
                        {
                        });

            var GenerateAuditConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("9e830ae4-ca06-4f05-ad32-2cf36904e377"),
                    "GenerateAuditConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("True"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "AuditConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config/",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/AuditConfig.config.tt"),
                        });

            var GenerateMessageForwardingConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("07f9c0cf-994d-4978-a9b9-5e70e8c03dce"),
                    "GenerateMessageForwardingConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "MessageForwardingInCaseOfFaultConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/MessageForwardingInCaseOfFaultConfig.config.tt"),
                        });

            var GenerateTestMessagesAspx =
                element.CreateOrUpdateCommandSettings(
                    new Guid("0e9cbc74-ebd5-4b66-8dfa-bfaf9f540cc3"),
                    "GenerateTestMessagesAspx",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("True"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "TestMessages.aspx",
                            TargetPath = "~",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/TestMessages.aspx.tt"),
                        });

            var GenerateTestMessagesAspxCs =
                element.CreateOrUpdateCommandSettings(
                    new Guid("8076b123-b5b0-46dd-8c1d-621239ff8fee"),
                    "GenerateTestMessagesAspxCs",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("True"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "TestMessages.aspx.generated.cs",
                            TargetPath = "~/TestMessages.aspx",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/TestMessages.aspx.cs.tt"),
                        });

            var GenerateTestMessagesAspxDesignerCs =
                element.CreateOrUpdateCommandSettings(
                    new Guid("f40ddb7d-bf2d-46f3-ad73-f34fa5af702e"),
                    "GenerateTestMessagesAspxDesignerCs",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("True"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "TestMessages.aspx.designer.cs",
                            TargetPath = "~/TestMessages.aspx",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/TestMessages.aspx.designer.cs.tt"),
                        });

            var GenerateTransportConfigCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("83de9acc-c878-4659-bbca-eda21bcd6a6f"),
                    "GenerateTransportConfigCode",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "TransportConfig.cs",
                            TargetPath = "~/Infrastructure/GeneratedCode",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/TransportConfig.tt"),
                        });

            var GenerateTransportConfigConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("8ead16bf-7a2b-42d6-a414-2182f54b4763"),
                    "GenerateTransportConfigConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "TransportConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/TransportConfig.config.tt"),
                        });

            var GenerateTransportConnectionStringConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("f86ce38e-ee17-41f0-8945-346e957940f9"),
                    "GenerateTransportConnectionStringConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "TransportConnectionString.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/TransportConnectionString.config.tt"),
                        });

            var GenerateUnicastBusConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("d23181a1-6b27-4314-b3a5-33021fc0eb57"),
                    "GenerateUnicastBusConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "UnicastBusConfig.config",
                            TargetPath = "~/Infrastructure/GeneratedCode/Config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/UnicastBusConfig.config.tt"),
                        });

            var GenerateWebConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("4bc12ca6-a46c-4e82-94b0-129018e87271"),
                    "GenerateWebConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "Web.config",
                            TargetPath = "~",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/Web.config.tt"),
                        });

            var GenerateWebDebugConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("f5f8f224-0afb-40ad-8f97-f913ecfde19c"),
                    "GenerateWebDebugConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "Web.Debug.config",
                            TargetPath = "~/Web.config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/Web.Debug.config.tt"),
                        });

            var GenerateWebGlobalInitialization =
                element.CreateOrUpdateCommandSettings(
                    new Guid("16906dd4-d11d-4d5f-ad82-ae41994017d8"),
                    "GenerateWebGlobalInitialization",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "WebGlobalInitialization.cs",
                            TargetPath = "~/Infrastructure/",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/WebGlobalInitialization.cs.tt"),
                        });

            var GenerateWebInitialization =
                element.CreateOrUpdateCommandSettings(
                    new Guid("5733ebbc-2a45-4add-bf19-4744bc480fcc"),
                    "GenerateWebInitialization",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "GeneratedWebInitialization.cs",
                            TargetPath = "~/Infrastructure/GeneratedCode",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/GeneratedWebInitialization.cs.tt"),
                        });

            var GenerateWebReleaseConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("074e1cb0-8b59-4e7a-b1fa-9571cd2b2830"),
                    "GenerateWebReleaseConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "Web.Release.Debug",
                            TargetPath = "~/Web.config",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/Web.Release.config.tt"),
                        });

            var InstallNuGetPackages =
                element.CreateOrUpdateCommandSettings(
                    new Guid("373f5296-2275-4818-895e-c3ac9fd06ad2"),
                    "InstallNuGetPackages",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.AddNugetReferencesCommand
                        {
                            IgnoreHost = BindingFor.Value<bool>("True"),
                        });

            var ProcessAfterUnfoldingProject =
                element.CreateOrUpdateCommandSettings(
                    new Guid("dc6634a9-a147-4285-ac38-d053a40ba00f"),
                    "ProcessAfterUnfoldingProject",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.ProcessRootAfterUnfoldedProject
                        {
                        });

            var PublishEventCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("2adc6882-c200-405d-8bdf-5f3c29b4a072"),
                    "PublishEventCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowEventComponentPicker
                        {
                        });

            var RaiseOnInstantiated =
                element.CreateOrUpdateCommandSettings(
                    new Guid("a713ee03-f59d-476b-a0fb-844d5e753217"),
                    "RaiseOnInstantiated",
                    () =>
                        new NServiceBusStudio.Automation.Commands.Endpoints.NSBH.RaiseOnInstantiated
                        {
                        });

            var SendCommandCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("5382586e-5e44-45c2-96e0-e6f5952fba72"),
                    "SendCommandCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowCommandComponentPicker
                        {
                        });

            var UnfoldLoggingConfig =
                element.CreateOrUpdateCommandSettings(
                    new Guid("fda1c817-b676-4500-9438-5082c5bbfcdb"),
                    "UnfoldLoggingConfig",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Content",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("PreserveNewest"),
                            TargetFileName = "Logging.config",
                            TargetPath = "~/Infrastructure",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/Logging.config.tt"),
                        });

            var UnfoldMessageConventions =
                element.CreateOrUpdateCommandSettings(
                    new Guid("9f696e71-25da-4688-b7a9-709c0c59e901"),
                    "UnfoldMessageConventions",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "MessageConventions.cs",
                            TargetPath = "~/Infrastructure",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Endpoints/NSBWeb/MessageConventions.cs.tt"),
                        });

            var UnfoldProject =
                element.CreateOrUpdateCommandSettings(
                    new Guid("4f45fc70-0514-411c-9dbb-f8b30e2b5bb3"),
                    "UnfoldProject",
                    () =>
                        new NuPattern.Library.Commands.UnfoldVsTemplateCommand
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetFileName = "{Root.InstanceName}.{InstanceName}",
                            TargetPath = "",
                            TemplateUri = BindingFor.Value<Uri>("template://project/CSharp/4d7de2cfe295-Application.Design.Endpoints.NServiceBusWeb"),
                        });

            var GenerateCodeConfigSection =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "GenerateCodeConfigSection",
                    GenerateMessageForwardingConfig,
                    GenerateTransportConfigConfig,
                    GenerateUnicastBusConfig,
                    GenerateAuditConfig,
                    GenerateWebInitialization,
                    GenerateWebGlobalInitialization,
                    GenerateTransportConfigCode,
                    GenerateTransportConnectionStringConfig,
                    GenerateTestMessagesAspx,
                    GenerateTestMessagesAspxCs,
                    GenerateTestMessagesAspxDesignerCs);

            var GenerateInitialCodeCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "GenerateInitialCodeCommand",
                    GenerateWebConfig,
                    GenerateWebDebugConfig,
                    GenerateWebReleaseConfig,
                    AddProjectReferences);

            var OnInstantiateCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateCommand",
                    UnfoldProject,
                    InstallNuGetPackages,
                    UnfoldMessageConventions,
                    UnfoldLoggingConfig,
                    GenerateInitialCodeCommand,
                    GenerateCodeConfigSection,
                    RaiseOnInstantiated,
                    ProcessAfterUnfoldingProject);

            // commands referenced by automation  
            // - AddAuthenticationCommand
            // - OnInstantiateCommand
            // - PublishEventCommand
            // - SendCommandCommand
        }

        private static void Setup_ProcessedCommandLink_Automation(IPatternElementSchema element)
        {
            var AddEndpointProjectReferences =
                element.CreateOrUpdateCommandSettings(
                    new Guid("6ecd8a73-3a79-455e-9bf6-00259c6c65fb"),
                    "AddEndpointProjectReferences",
                    () =>
                        new NServiceBusStudio.Automation.Commands.AddEndpointProjectReferences
                        {
                        });

            var OnInstantiateCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateCommand",
                    AddEndpointProjectReferences);

            // commands referenced by automation  
            // - OnInstantiateCommand
        }

        private static void Setup_Publishes_Automation(IPatternElementSchema element)
        {
            var PublishCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("f1959529-6eef-475b-b4b0-6eb6e274bdda"),
                    "PublishCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowEventTypePicker
                        {
                            //CurrentComponent = "",
                        });

            var SendCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("74775974-a991-4646-a4ca-8968ce5e212a"),
                    "SendCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowCommandTypePicker
                        {
                        });

            // commands referenced by automation  
            // - PublishCommand
            // - SendCommand
        }

        private static void Setup_Service_Automation(IPatternElementSchema element)
        {
            var PublishCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("77ce2b85-89d4-4f3f-8cae-baf31df6219f"),
                    "PublishCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowEventTypePickerFromService
                        {
                        });

            var SendCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("928b38d7-aa0e-4b75-86d1-178b2ecc54cb"),
                    "SendCommand",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowCommandTypePickerFromService
                        {
                        });

            // commands referenced by automation  
            // - PublishCommand
            // - SendCommand
        }

        private static void Setup_ServiceLibrary_Automation(IPatternElementSchema element)
        {
            var CheckForProject =
                element.CreateOrUpdateCommandSettings(
                    new Guid("c26419cb-c0b4-433b-b767-e0dcabfa9030"),
                    "CheckForProject",
                    () =>
                        new NServiceBusStudio.Automation.Commands.LibraryAddedCommand
                        {
                        });

            var OnActivationCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("caf88ece-6cf4-43ce-a929-80b47a89cfde"),
                    "OnActivationCommand",
                    () =>
                        new NuPattern.Library.Commands.ActivateArtifactCommand
                        {
                            Open = BindingFor.Value<bool>("True"),
                        });

            var UnfoldLibraryCode =
                element.CreateOrUpdateCommandSettings(
                    new Guid("d4a9b7ec-2c77-4c47-bb26-ddbc494b0a16"),
                    "UnfoldLibraryCode",
                    () =>
                        new NuPattern.Library.Commands.GenerateProductCodeCommandCustom
                        {
                            SanitizeName = BindingFor.Value<bool>("False"),
                            SyncName = BindingFor.Value<bool>("False"),
                            Tag = "",
                            TargetBuildAction = "Compile",
                            TargetCopyToOutput = BindingFor.Value<CopyToOutput>("DoNotCopy"),
                            TargetFileName = "{CodeIdentifier}.cs",
                            TargetPath = "{FilePath}",
                            TemplateUri = BindingFor.Value<Uri>("t4://extension/23795EC3-3DEA-4F04-9044-4056CF91A2ED/T/T4/Library.tt"),
                        });

            var OnInstantiatedCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiatedCommand",
                    CheckForProject,
                    UnfoldLibraryCode);

            // commands referenced by automation  
            // - OnActivationCommand
            // - OnInstantiatedCommand
        }

        private static void Setup_SubscribedEventLink_Automation(IPatternElementSchema element)
        {
            var AddEndpointProjectReferences =
                element.CreateOrUpdateCommandSettings(
                    new Guid("ba69a23d-7427-40bb-9d2c-57ae6080a8e9"),
                    "AddEndpointProjectReferences",
                    () =>
                        new NServiceBusStudio.Automation.Commands.AddEndpointProjectReferences
                        {
                        });

            var OnInstantiateCommand =
                element.CreateOrUpdateAggregatorCommandSettings(
                    "OnInstantiateCommand",
                    AddEndpointProjectReferences);

            // commands referenced by automation  
            // - OnInstantiateCommand
        }

        private static void Setup_UseCase_Automation(IPatternElementSchema element)
        {
            var AddStartingEndpointCommand =
                element.CreateOrUpdateCommandSettings(
                    new Guid("7538aca2-1e1c-48e7-97d0-f106ad715a10"),
                    "AddStartingEndpointCommand",
                    () =>
                        new AbstractEndpoint.Automation.Commands.ShowStartedByPicker
                        {
                        });

            //var EditScriptCommand =
            //    element.CreateOrUpdateCommandSettings(
            //        new Guid("3244fc4b-1b10-4792-95f9-070472bf8571"),
            //        "EditScriptCommand",
            //        () =>
            //            new NServiceBusStudio.Automation.Commands.ShowUseCaseEditorCommand
            //            {
            //            });

            var ShowUseCaseCommandPicker =
                element.CreateOrUpdateCommandSettings(
                    new Guid("99b2b439-4644-45c6-8536-dc3a218e2db9"),
                    "ShowUseCaseCommandPicker",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowUseCaseCommandPickerCommand
                        {
                        });

            var ShowUseCaseComponentPicker =
                element.CreateOrUpdateCommandSettings(
                    new Guid("8491f92d-7854-4f78-8d79-23dc373784ed"),
                    "ShowUseCaseComponentPicker",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowUseCaseComponentPickerCommand
                        {
                        });

            var ShowUseCaseEventPicker =
                element.CreateOrUpdateCommandSettings(
                    new Guid("60928d78-1e83-4c4d-b1da-686722871d7d"),
                    "ShowUseCaseEventPicker",
                    () =>
                        new NServiceBusStudio.Automation.Commands.ShowUseCaseEventPickerCommand
                        {
                        });

            // commands referenced by automation  
            // - AddStartingEndpointCommand
            // - ShowUseCaseCommandPicker
            // - ShowUseCaseComponentPicker
            // - ShowUseCaseEventPicker
        }

    }
}

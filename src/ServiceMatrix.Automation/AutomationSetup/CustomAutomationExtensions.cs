using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Newtonsoft.Json;
using NuPattern.Library.Automation;
using NuPattern.Runtime;
using NuPattern.Runtime.Bindings;

namespace NServiceBusStudio
{
    public static class CustomAutomationExtensions
    {
        public static void CreateDispatchedCodeGenerationCommands(
            this IPatternElementSchema element,
            string baseName,
            bool sanitizeName,
            bool syncName,
            string tag,
            string targetBuildAction,
            string targetCopyToOutput,
            string targetFileName,
            string targetPath,
            params Tuple<string, string, string>[] templatesPerVersion)
        {
            var references = new List<object>();

            foreach (var template in templatesPerVersion)
            {
                var codeGenerationCommand = element.CreateCodeGenerationCommandSettings(
                   Guid.ParseExact(template.Item1, "B"),
                   template.Item3 + baseName,
                   sanitizeName,
                   syncName,
                   tag,
                   targetBuildAction,
                   targetCopyToOutput,
                   targetFileName,
                   targetPath,
                   @"t4://extension/" + ToolkitConstants.VsixIdentifier + template.Item2);

                references.Add(new { CommandId = codeGenerationCommand.Id, NsbVersionPattern = template.Item3 });
            }

            element.UpdateDispatcherCommandSettings(baseName, references);
        }

        public static ICommandSettings CreateCodeGenerationCommandSettings(
            this IPatternElementSchema element,
            Guid id,
            string name,
            bool sanitizeName,
            bool syncName,
            string tag,
            string targetBuildAction,
            string targetCopyToOutput,
            string targetFileName,
            string targetPath,
            string templateUri)
        {
            return CreateCommandSettings(
                element,
                id,
                name,
                "NuPattern.Library.Commands.GenerateProductCodeCommandCustom",
                new PropertyBindingSettings { Name = "SanitizeName", Value = sanitizeName ? "True" : "False" },
                new PropertyBindingSettings { Name = "SyncName", Value = syncName ? "True" : "False" },
                new PropertyBindingSettings { Name = "Tag", Value = tag },
                new PropertyBindingSettings { Name = "TargetBuildAction", Value = targetBuildAction },
                new PropertyBindingSettings { Name = "TargetCopyToOutput", Value = targetCopyToOutput },
                new PropertyBindingSettings { Name = "TargetFileName", Value = targetFileName },
                new PropertyBindingSettings { Name = "TargetPath", Value = targetPath },
                new PropertyBindingSettings { Name = "TemplateUri", Value = templateUri });
        }

        public static void CreateDispatchedComponentCodeGenerationCommands(
            this IPatternElementSchema element,
            string baseName,
            bool checkIsDeployed,
            bool checkIsNotUnfoldedCustomCode,
            bool sanitizeName,
            bool syncName,
            string tag,
            string targetBuildAction,
            string targetCopyToOutput,
            string targetFileName,
            string targetPath,
            params Tuple<string, string, string>[] templatesPerVersion)
        {
            var references = new List<object>();

            foreach (var template in templatesPerVersion)
            {
                var codeGenerationCommand = element.CreateComponentCodeGenerationCommandSettings(
                   Guid.ParseExact(template.Item1, "B"),
                   template.Item3 + baseName,
                   checkIsDeployed,
                   checkIsNotUnfoldedCustomCode,
                   sanitizeName,
                   syncName,
                   tag,
                   targetBuildAction,
                   targetCopyToOutput,
                   targetFileName,
                   targetPath,
                   @"t4://extension/" + ToolkitConstants.VsixIdentifier + template.Item2);

                references.Add(new { CommandId = codeGenerationCommand.Id, NsbVersionPattern = template.Item3 });
            }

            element.UpdateDispatcherCommandSettings(baseName, references);
        }

        public static ICommandSettings CreateComponentCodeGenerationCommandSettings(
            this IPatternElementSchema element,
            Guid id,
            string name,
            bool checkIsDeployed,
            bool checkIsNotUnfoldedCustomCode,
            bool sanitizeName,
            bool syncName,
            string tag,
            string targetBuildAction,
            string targetCopyToOutput,
            string targetFileName,
            string targetPath,
            string templateUri)
        {
            return CreateCommandSettings(
                element,
                id,
                name,
                "NuPattern.Library.Commands.GenerateComponentCodeCommand",
                new PropertyBindingSettings { Name = "CheckIsDeployed", Value = checkIsDeployed ? "True" : "False" },
                new PropertyBindingSettings { Name = "CheckIsNotUnfoldedCustomCode", Value = checkIsNotUnfoldedCustomCode ? "True" : "False" },
                new PropertyBindingSettings { Name = "SanitizeName", Value = sanitizeName ? "True" : "False" },
                new PropertyBindingSettings { Name = "SyncName", Value = syncName ? "True" : "False" },
                new PropertyBindingSettings { Name = "Tag", Value = tag },
                new PropertyBindingSettings { Name = "TargetBuildAction", Value = targetBuildAction },
                new PropertyBindingSettings { Name = "TargetCopyToOutput", Value = targetCopyToOutput },
                new PropertyBindingSettings { Name = "TargetFileName", Value = targetFileName },
                new PropertyBindingSettings { Name = "TargetPath", Value = targetPath },
                new PropertyBindingSettings { Name = "TemplateUri", Value = templateUri });
        }

        public static ICommandSettings UpdateDispatcherCommandSettings(
            this IPatternElementSchema element,
            string name,
            object references)
        {
            var generateTransportConfigCode = element.GetAutomationSettings<ICommandSettings>(name);
            generateTransportConfigCode.TypeId = "NServiceBusStudio.Automation.Commands.TargetNsbVersionDispatcherCommand";
            generateTransportConfigCode.Properties.Clear();
            generateTransportConfigCode.Properties.Add(
                new PropertyBindingSettings
                {
                    Name = "TargetNsbVersion",
                    ValueProvider = new ValueProviderBindingSettings
                    {
                        TypeId = "NuPattern.Library.ValueProviders.ElementPropertyValueProvider",
                        Properties = { new PropertyBindingSettings { Name = "PropertyName", Value = "TargetNsbVersion" } }
                    }
                });
            generateTransportConfigCode.Properties.Add(new PropertyBindingSettings { Name = "CommandReferenceList", Value = JsonConvert.SerializeObject(references) });

            return generateTransportConfigCode;
        }

        public static TSettings CreateAutomationSettings<TSettings>(
            this IPatternElementSchema container,
            Guid id,
            string settingsName,
            Action<IAutomationSettingsSchema> initializer = null)
            where TSettings : class, IAutomationSettings
        {
            var automationSettings = default(TSettings);

            container.CreateAutomationSettingsSchema(aes =>
            {
                var aesMel = (ModelElement)aes;
                var classInfo = aesMel.Store.DomainDataDirectory.DomainClasses.First(c => typeof(TSettings).IsAssignableFrom(c.ImplementationClass));
                var extensionElement = (ExtensionElement)((ModelElement)aes).Partition.ElementFactory.CreateElement(classInfo, new[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, id) });
                ModelElement.AddExtension(aesMel, extensionElement);
                automationSettings = (TSettings)(object)extensionElement;

                aes.Name = settingsName;
                aes.AutomationType = classInfo.DisplayName;
                aes.Classification = automationSettings.Classification;

                if (initializer != null)
                    initializer(aes);
            });

            return automationSettings;
        }

        private static ICommandSettings CreateCommandSettings(
            IPatternElementSchema element,
            Guid id,
            string name,
            string typeId,
            params IPropertyBindingSettings[] properties)
        {
            return
                element.CreateAutomationSettings<ICommandSettings>(
                    id,
                    name,
                    s =>
                    {
                        var commandSettings = s.GetExtensions<ICommandSettings>().First();
                        commandSettings.TypeId = typeId;
                        foreach (var propBindingSetting in properties)
                        {
                            commandSettings.Properties.Add(propBindingSetting);
                        }
                    });
        }
    }
}

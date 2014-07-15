using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using NuPattern;
using NuPattern.Library.Automation;
using NuPattern.Runtime;
using NuPattern.Runtime.Bindings;

namespace NServiceBusStudio.Automation.Commands
{
    using Newtonsoft.Json;

    [DisplayName("Setup code generation commands for NSB Host")]
    [Description("Setup code generation commands for NSB Host")]
    [Category("Service Matrix")]
    [CLSCompliant(false)]
    public class SetupHostCodeGenerationCommands : NuPattern.Runtime.Command
    {
        Type commandAutomationType = Type.GetType("NuPattern.Library.Automation.CommandAutomation, NuPattern.Library", false);

        private static IEnumerable<ICommandSettings> commandSettings;

        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IPatternManager PatternManager
        {
            get;
            set;
        }

        private IEnumerable<ICommandSettings> CommandSettings
        {
            get
            {
                return commandSettings ?? (commandSettings = BuildCommandSettings());
            }
        }

        public override void Execute()
        {
            this.ValidateObject();

            foreach (var settings in CommandSettings)
            {
                CurrentElement.AddAutomationExtension((IAutomationExtension)Activator.CreateInstance(commandAutomationType, CurrentElement, settings));
            }
        }

        // TODO add support for dispatcher
        private IEnumerable<ICommandSettings> BuildCommandSettings()
        {
            var toolkit = PatternManager.InstalledToolkits.First(t => t.Name == ToolkitConstants.ToolkitName);
            var owner = toolkit.Schema.FindAll<IPatternElementSchema>().First(pe => pe.Name == "NServiceBusHost");

            var v4GenerateTransportConfigCode =
                new CodeGenerationCommandSettings(
                    owner,
                    "V4GenerateTransportConfigCode",
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "TransportConfig.cs",
                    "~",
                    @"t4://extension/" + ToolkitConstants.VsixIdentifier + @"/T/T4/Endpoints/NSBH/TransportConfig.v4.tt");

            var v5GenerateTransportConfigCode =
                new CodeGenerationCommandSettings(
                    owner,
                    "V5GenerateTransportConfigCode",
                    false,
                    false,
                    "",
                    "Compile",
                    "DoNotCopy",
                    "TransportConfig.cs",
                    "~",
                    @"t4://extension/" + ToolkitConstants.VsixIdentifier + @"/T/T4/Endpoints/NSBH/TransportConfig.v5.tt");

            var generateTransportConfigCode =
                new DispatcherCommandSettings(
                    owner,
                    "GenerateTransportConfigCode",
                    Guid.Parse("4dfafba4-8e2b-4437-8c84-495041cb219f"),
                    new[]
                    {
                        new { CommandId = v4GenerateTransportConfigCode.Id, NsbVersionPattern = "NSB 4.0" },
                        new { CommandId = v5GenerateTransportConfigCode.Id, NsbVersionPattern = "NSB 5.0" }
                    });

            yield return v4GenerateTransportConfigCode;
            yield return v5GenerateTransportConfigCode;
            yield return generateTransportConfigCode;
        }
    }

    class CodeGenerationCommandSettings : ICommandSettings
    {
        public CodeGenerationCommandSettings(IPatternElementSchema owner, string name, bool sanitizeName, bool syncName, string tag, string targetBuildAction, string targetCopyToOutput, string targetFileName, string targetPath, string templateUri)
        {
            Owner = owner;
            Name = name;
            Id = Guid.NewGuid();

            Properties = new List<IPropertyBindingSettings>
            {
                new PropertyBindingSettings{ Name = "SanitizeName", Value = sanitizeName ? "True" : "False" }, 
                new PropertyBindingSettings{ Name = "SyncName", Value = syncName ? "True" : "False" }, 
                new PropertyBindingSettings{ Name = "Tag", Value = tag }, 
                new PropertyBindingSettings{ Name = "TargetBuildAction", Value = targetBuildAction }, 
                new PropertyBindingSettings{ Name = "TargetCopyToOutput", Value = targetCopyToOutput }, 
                new PropertyBindingSettings{ Name = "TargetFileName", Value = targetFileName }, 
                new PropertyBindingSettings{ Name = "TargetPath", Value = targetPath }, 
                new PropertyBindingSettings{ Name = "TemplateUri", Value = templateUri }, 
            };
        }

        public IDisposable SubscribeChanged(Expression<Func<ICommandSettings, object>> propertyExpression, Action<ICommandSettings> callbackAction)
        {
            throw new NotSupportedException();
        }

        public string TypeId
        {
            get { return "NuPattern.Library.Commands.GenerateProductCodeCommandCustom"; }
            set { throw new NotSupportedException(); }
        }

        public IList<IPropertyBindingSettings> Properties { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AutomationSettingsClassification Classification
        {
            get { return AutomationSettingsClassification.General; }
        }

        public IAutomationExtension CreateAutomation(IProductElement owner)
        {
            throw new NotSupportedException();
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public IPatternElementSchema Owner { get; private set; }
    }

    class DispatcherCommandSettings : ICommandSettings
    {
        public DispatcherCommandSettings(IPatternElementSchema owner, string name, Guid id, IEnumerable<object> references)
        {
            Owner = owner;
            Name = name;
            Id = id;

            Properties = new List<IPropertyBindingSettings>
            {
                new PropertyBindingSettings{ Name = "CommandReferenceList", Value = JsonConvert.SerializeObject(references) }, 
            };
        }

        public IDisposable SubscribeChanged(Expression<Func<ICommandSettings, object>> propertyExpression, Action<ICommandSettings> callbackAction)
        {
            throw new NotSupportedException();
        }

        public string TypeId
        {
            get { return "NServiceBusStudio.Automation.Commands"; }
            set { throw new NotSupportedException(); }
        }

        public IList<IPropertyBindingSettings> Properties { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AutomationSettingsClassification Classification
        {
            get { return AutomationSettingsClassification.General; }
        }

        public IAutomationExtension CreateAutomation(IProductElement owner)
        {
            throw new NotSupportedException();
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public IPatternElementSchema Owner { get; private set; }
    }
}
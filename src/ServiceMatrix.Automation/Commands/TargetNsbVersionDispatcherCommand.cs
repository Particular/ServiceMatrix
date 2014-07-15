using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using NuPattern.Runtime;
using NuPattern.Runtime.Automation;

namespace NServiceBusStudio.Automation.Commands
{
    using System.Globalization;
    using NuPattern.Library.Automation;
    using NuPattern.Runtime.Bindings.Design;

    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    [DisplayName("Execute commands conditionally based on the target framework version")]
    [Description("Execute commands conditionally based on the target framework version.")]
    [Category("Service Matrix")]
    [CLSCompliant(false)]
    public class TargetNsbVersionDispatcherCommand : NuPattern.Runtime.Command
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        [TypeConverter(typeof(TargetNsbVersionPatternConditionalCommandReferencesConverter))]
        public Collection<TargetNsbVersionPatternConditionalCommandReference> CommandReferenceList { get; set; }

        public string TargetNsbVersion { get; set; }

        /// <summary>
        /// Executes the referenced commmand or commands.
        /// </summary>
        public override void Execute()
        {
            var matchingReference = CommandReferenceList.FirstOrDefault(r => Regex.IsMatch(TargetNsbVersion, r.NsbVersionPattern));
            if (matchingReference != null)
            {
                var commandAutomation =
                    CurrentElement.AutomationExtensions.First().ResolveAutomationReference<IAutomationExtension>(matchingReference.CommandId);
                if (commandAutomation != null)
                {
                    commandAutomation.Execute();
                }
            }
        }

        public void AddReference(TargetNsbVersionPatternConditionalCommandReference reference)
        {
            (CommandReferenceList ?? (CommandReferenceList = new Collection<TargetNsbVersionPatternConditionalCommandReference>())).Add(reference);
        }
    }

    class TargetNsbVersionPatternConditionalCommandReferencesConverter : DesignCollectionConverter<TargetNsbVersionPatternConditionalCommandReference>
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var baseResult = base.ConvertFrom(context, culture, value);
            //var references = baseResult as Collection<TargetNsbVersionPatternConditionalCommandReference>;
            //if (references != null)
            //{
            //    // Assign commandSettings (design-time only)
            //    if (context != null && context.Instance != null)
            //    {
            //        var settings = (ICommandSettings)context.Instance;

            //        var values = (Collection<TargetNsbVersionPatternConditionalCommandReference>)baseResult;
            //        return values.Select(val => new CommandReference(settings) { CommandId = val.CommandId });
            //    }

            //    return references;
            //}

            return baseResult;
        }
    }
}
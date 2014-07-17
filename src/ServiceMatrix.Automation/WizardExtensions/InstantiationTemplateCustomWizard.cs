using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using NuPattern.Library.TemplateWizards;

namespace NServiceBusStudio.Automation.WizardExtensions
{
    public class InstantiationTemplateCustomWizard : InstantiationTemplateWizard
    {
        public override void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            // This is done to trick NuPattern into using the v4 template when unfolding the v5 template.
            // The template is used by NuPattern to identify the toolkit and product elements associated to the template,
            // and there can only one template associated to the application
            base.RunStarted(automationObject, replacementsDictionary, runKind, ApplicationTemplateWorkaroundHelper.ReplaceCustomParameters(customParams));
        }
    }
}

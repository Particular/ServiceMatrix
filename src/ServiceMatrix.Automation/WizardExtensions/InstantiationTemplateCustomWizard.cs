using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using NuPattern.Library.TemplateWizards;

namespace NServiceBusStudio.Automation.WizardExtensions
{
    public class InstantiationTemplateCustomWizard : InstantiationTemplateWizard
    {
        public override void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            base.RunStarted(automationObject, replacementsDictionary, runKind, ApplicationTemplateWorkaroundHelper.ReplaceCustomParameters(customParams));
        }
    }
}

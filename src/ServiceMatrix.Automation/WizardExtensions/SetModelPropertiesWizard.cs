using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using NuPattern.Library;
using NuPattern.VisualStudio.TemplateWizards;

namespace NServiceBusStudio.Automation.WizardExtensions
{
    public class SetModelPropertiesWizard : TemplateWizard
    {
        string targetFrameworkVersion;

        public override void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            base.RunStarted(automationObject, replacementsDictionary, runKind, customParams);

            replacementsDictionary.TryGetValue("$targetframeworkversion$", out targetFrameworkVersion);
        }

        public override void ProjectFinishedGenerating(Project project)
        {
            base.ProjectFinishedGenerating(project);

            if (!string.IsNullOrEmpty(targetFrameworkVersion) && UnfoldScope.IsActive)
            {
                var application = UnfoldScope.Current.Automation.Owner.As<IApplication>();
                if (application != null)
                {
                    application.TargetFrameworkVersion = targetFrameworkVersion;
                }
            }
        }
    }
}
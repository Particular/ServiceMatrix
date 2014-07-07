using System.Collections.Generic;
using System.ComponentModel.Composition;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using NuPattern.Library;
using NuPattern.Runtime;
using NuPattern.VisualStudio.TemplateWizards;

namespace NServiceBusStudio.Automation.WizardExtensions
{
    public class SetTargetFrameworkWizard : TemplateWizard
    {
        string targetFrameworkVersion;

        /// <summary>
        /// Gets the pattern manager.
        /// </summary>
        [Import]
        public IPatternManager PatternManager { get; internal set; }

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
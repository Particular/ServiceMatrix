using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using NuPattern.Library;
using NuPattern.VisualStudio.TemplateWizards;

namespace NServiceBusStudio.Automation.WizardExtensions
{
    public class SetModelPropertiesWizard : TemplateWizard
    {
        string targetFrameworkVersion;
        string targetNsbVersion;

        public override void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            base.RunStarted(automationObject, replacementsDictionary, runKind, customParams);

            replacementsDictionary.TryGetValue("$targetframeworkversion$", out targetFrameworkVersion);
            var nServiceBusVersionElement = TemplateSchema.WizardData.SelectMany(wd => wd.Elements.Where(e => e.LocalName == "NServiceBus")).FirstOrDefault();
            if (nServiceBusVersionElement != null)
            {
                targetNsbVersion = nServiceBusVersionElement.GetAttribute("Version");
            }
        }

        public override void ProjectFinishedGenerating(Project project)
        {
            base.ProjectFinishedGenerating(project);

            if (!string.IsNullOrEmpty(targetFrameworkVersion) && UnfoldScope.IsActive)
            {
                var application = UnfoldScope.Current.Automation.Owner.As<IApplication>();
                if (application != null)
                {
                    if (targetFrameworkVersion != null)
                    {
                        application.TargetFrameworkVersion = targetFrameworkVersion;
                    }

                    if (targetNsbVersion != null)
                    {
                        application.TargetNsbVersion = targetNsbVersion;
                    }
                }
            }
        }
    }
}
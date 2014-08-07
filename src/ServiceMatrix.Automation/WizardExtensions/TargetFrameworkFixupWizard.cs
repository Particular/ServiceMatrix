using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace NServiceBusStudio.Automation.WizardExtensions
{
    public class TargetFrameworkFixupWizard : IWizard
    {
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            // Override the target framework to work around the issue with loading a 4.5 template
            const string targetFrameworkVersionKey = "$targetframeworkversion$";
            string targetFrameworkVersion;
            if (!replacementsDictionary.TryGetValue(targetFrameworkVersionKey, out targetFrameworkVersion) || targetFrameworkVersion == "4.0")
            {
                replacementsDictionary[targetFrameworkVersionKey] = "4.5";
            }
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }
    }
}
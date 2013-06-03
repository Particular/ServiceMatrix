using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.WizardExtensions
{
    public class SetValidCreationContextWizardExtension: IWizard
    {
        public static string ValidCreationContextKey = "NServiceBusStudio-ValidCreationContext";

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            CallContext.SetData(SetValidCreationContextWizardExtension.ValidCreationContextKey, true);
        }

        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}

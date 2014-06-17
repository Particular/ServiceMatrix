using EnvDTE;
using NServiceBusStudio.Automation.Model;
using NuPattern.Library;

namespace NServiceBusStudio.Automation.WizardExtensions
{
    public class SetV5ModelPropertiesWizard : SetModelPropertiesWizard
    {
        public override void ProjectFinishedGenerating(Project project)
        {
            base.ProjectFinishedGenerating(project);

            if (UnfoldScope.IsActive)
            {
                var application = UnfoldScope.Current.Automation.Owner.As<IApplication>();
                if (application != null)
                {
                    application.TargetNsbVersion = TargetNsbVersion.Version5;
                }
            }
        }
    }
}
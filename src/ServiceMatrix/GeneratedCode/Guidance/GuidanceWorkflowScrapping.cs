using NuPattern.Runtime.Guidance.Workflow;

namespace NServiceBusStudio.Guidance
{
    public class GuidanceWorkflowScrapping
    {
        public Initial Generate()
        {
            var initial = new Initial
            {
                Name = "ServiceMatrix Guidance",
            };

            var guidanceaction = new GuidanceAction
            {
                Name = "ServiceMatrix Online Documentation",
                Link = "http://docs.particular.net/servicematrix/"
            };
            initial.ConnectTo(guidanceaction);

            var final = new Final
            {
                Name = "Final",
            };
            guidanceaction.ConnectTo(final);

            return initial;
        }
    }
}

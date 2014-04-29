namespace NServiceBusStudio.Automation.ValueProviders
{
    using NuPattern.Runtime;
    public class ToolkitVersionValueProvider : ValueProvider
    {
        public override object Evaluate()
        {
            return ToolkitConstants.Version;
        }
    }
}

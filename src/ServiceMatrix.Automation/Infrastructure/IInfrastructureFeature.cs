namespace NServiceBusStudio.Automation.Infrastructure
{
    using System;
    using NuPattern.Runtime;

    public interface IInfrastructureFeature
    {
        void Initialize(IApplication application, IProductElement infrastructure, IServiceProvider serviceProvider, IPatternManager patternManager);
    }
}

using System;
using System.Collections.Generic;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Infrastructure
{
    public class InfrastructureManager
    {
        private IApplication application;
        public List<IInfrastructureFeature> Features = new List<IInfrastructureFeature>();

        public IPatternManager PatternManager { get; set; }

        public InfrastructureManager(IApplication application, IServiceProvider serviceProvider, IPatternManager patternManager)
        {
            PatternManager = patternManager;
            this.application = application;

            // Build features collection
            Features.Add(new Authentication.AuthenticationFeature());

            // Initialize features
            var infrastructure = application.Design.Infrastructure.As<IProductElement>();
            Features.ForEach(f => f.Initialize(this.application, infrastructure, serviceProvider, PatternManager));
        }
    }
}


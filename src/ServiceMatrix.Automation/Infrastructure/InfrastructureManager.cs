using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;
using AbstractEndpoint;
using NuPattern.Library.Automation;
using Microsoft.VisualStudio.Modeling;
using System.Windows.Threading;
using System.ComponentModel.Composition;
using NServiceBusStudio.Automation.Extensions;
using NuPattern.Library.Commands;

namespace NServiceBusStudio.Automation.Infrastructure
{
    using NServiceBusStudio.Automation.Infrastructure.Authentication;

    public class InfrastructureManager
    {
        private IApplication application;
        public List<IInfrastructureFeature> Features = new List<IInfrastructureFeature>();
        private IServiceProvider serviceProvider;

        public IPatternManager PatternManager { get; set; }

        public InfrastructureManager(IApplication application, IServiceProvider serviceProvider, IPatternManager patternManager)
        {
            PatternManager = patternManager;
            this.serviceProvider = serviceProvider;
            this.application = application;

            // Build features collection
            Features.Add(new AuthenticationFeature());
            Features.Add(new UseCaseFeature());

            // Initialize features
            var infrastructure = application.Design.Infrastructure.As<IProductElement>();
            Features.ForEach(f => f.Initialize(this.application, infrastructure, serviceProvider, PatternManager));
        }
    }
}


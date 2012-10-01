using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace NServiceBusStudio.Automation.Infrastructure
{
    public interface IInfrastructureFeature
    {
        void Initialize(IApplication application, IProductElement infrastructure, IServiceProvider serviceProvider, IPatternManager patternManager);
    }
}

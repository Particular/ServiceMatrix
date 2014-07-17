namespace NServiceBusStudio.Automation.ValueProviders
{
    using System;
    using System.ComponentModel;
    using Model;
    using NuPattern.Runtime;

    [CLSCompliant(false)]
    [Category("General")]
    [Description("Get the default value for Target NSB Version")]
    public class TargetNsbVersionValueProvider : ValueProvider
    {
        public override object Evaluate()
        {
            return TargetNsbVersion.Version4;
        }
    }
}

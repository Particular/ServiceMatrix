using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Library.Automation;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Infrastructure
{
    public class EmptySettings : ICommandSettings
    {
        public IPropertyBindingSettings CreatePropertySettings(Action<IPropertyBindingSettings> initializer = null)
        {
            return null;
        }

        public IEnumerable<IPropertyBindingSettings> Properties
        {
            get { return new IPropertyBindingSettings[] { }; }
        }

        public IDisposable SubscribeChanged(System.Linq.Expressions.Expression<Func<ICommandSettings, object>> propertyExpression, Action<ICommandSettings> callbackAction)
        {
            return null;
        }

        public string TypeId { get; set; }

        IList<IPropertyBindingSettings> IBindingSettings.Properties
        {
            get { throw new NotImplementedException(); }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public AutomationSettingsClassification Classification
        {
            get { throw new NotImplementedException(); }
        }

        public IAutomationExtension CreateAutomation(IProductElement owner)
        {
            throw new NotImplementedException();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IPatternElementSchema Owner { get; set; }
    }
}

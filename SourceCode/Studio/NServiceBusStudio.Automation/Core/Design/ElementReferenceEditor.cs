using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Design;
using Microsoft.VisualStudio.Patterning.Runtime;
using System.ComponentModel;

namespace NServiceBusStudio.Core.Design
{
    public class ElementReferenceEditor<TOwner, TReference, TStrategy> : StandardValuesEditor
        where TOwner : class, IToolkitInterface
        where TReference : class, IToolkitInterface
        where TStrategy : IElementReferenceStrategy<TOwner, TReference>, new()
    {
        private IElementReferenceStrategy<TOwner, TReference> strategy = new TStrategy();

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var selectedValue = base.EditValue(context, provider, value);
            var referencedValue = selectedValue as TReference;

            if (referencedValue != null)
            {
                try
                {
                    var owner = context.Instance.As<TOwner>();
                    if (owner != null)
                    {
                        var reference = this.strategy.GetReference(owner);
                        reference.Value = (referencedValue == this.strategy.NullValue) ? null : referencedValue;

                        return context.PropertyDescriptor.Converter.ConvertToString(context, reference);
                    }
                }
                catch (NotSupportedException)
                {
                }

                return referencedValue.As<Microsoft.VisualStudio.Patterning.Runtime.IElement>().InstanceName;
            }

            return selectedValue;
        }
    }
}

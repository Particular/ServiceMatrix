namespace NServiceBusStudio.Core.Design
{
    using System;
    using System.ComponentModel;
    using NuPattern.ComponentModel.Design;
    using NuPattern.Runtime.ToolkitInterface;
    using NuPattern.Runtime;

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
                        var reference = strategy.GetReference(owner);
                        reference.Value = (referencedValue == strategy.NullValue) ? null : referencedValue;

                        return context.PropertyDescriptor.Converter.ConvertToString(context, reference);
                    }
                }
                catch (NotSupportedException)
                {
                }

                return referencedValue.As<IElement>().InstanceName;
            }

            return selectedValue;
        }
    }
}

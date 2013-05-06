using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using NuPattern.Runtime;
using NuPattern.Runtime.ToolkitInterface;
using NuPattern.ComponentModel;


namespace NServiceBusStudio.Core.Design
{
    public class ElementReferenceConverter<TOwner, TReference, TStrategy> : StringConverter
        where TOwner : class, IToolkitInterface
        where TReference : class, IToolkitInterface
        where TStrategy : IElementReferenceStrategy<TOwner, TReference>, new()
    {
        private IElementReferenceStrategy<TOwner, TReference> strategy = new TStrategy();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (context != null)
            {
                try
                {
                    var owner = context.Instance.As<TOwner>();
                    if (owner != null)
                    {
                        var reference = this.strategy.GetReference(owner);
                        // Force sync the display name with the current message name.
                        // This is only important for UEX, the APIs will always 
                        // use the referenced message by id.
                        reference.Refresh();

                        return reference.Value == null ? reference.NoneText : reference.Value.As<NuPattern.Runtime.IElement>().InstanceName;
                    }
                }
                catch (NotSupportedException)
                {
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            try
            {
                var owner = context.Instance.As<TOwner>();
                if (owner != null)
                {
                    var reference = this.strategy.GetReference(owner);
                    var standardValues = this.strategy.GetStandardValues(owner).ToList();

                    standardValues.Insert(0, new StandardValue(
                        reference.NoneText,
                        this.strategy.NullValue,
                        reference.NoneDescription));

                    return new StandardValuesCollection(standardValues);
                }
            }
            catch (NotSupportedException)
            {
                var standardValues = new List<StandardValue>();

                return new StandardValuesCollection(standardValues);
            }

            return base.GetStandardValues(context);
        }
    }
}

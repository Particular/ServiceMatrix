using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace NServiceBusStudio.Automation.ViewModels.Validation
{
    using System.Reflection;
    using Properties;

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class NotEqualToAttribute : ValidationAttribute
    {
        public NotEqualToAttribute(string otherProperty)
            : base(Resources.NotEqualToAttribute_MustNotMatch)
        {
            if (otherProperty == null)
            {
                throw new ArgumentNullException("otherProperty");
            }

            OtherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty);
        }

        private static string GetDisplayNameForProperty(Type containerType, string propertyName)
        {
            var typeDescriptor = GetTypeDescriptor(containerType).GetProperties().Find(propertyName, true);
            if (typeDescriptor == null)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Resources.ValidationCommon_PropertyNotFound, containerType.FullName, propertyName));
            }

            var attributes = typeDescriptor.Attributes.Cast<Attribute>().ToArray();
            var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault<DisplayAttribute>();
            if (displayAttribute != null)
            {
                return displayAttribute.GetName();
            }

            var displayNameAttribute = attributes.OfType<DisplayNameAttribute>().FirstOrDefault<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                return displayNameAttribute.DisplayName;
            }

            return propertyName;
        }

        private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                PropertyInfo property;
                if (validationContext == null || (property = validationContext.ObjectType.GetProperty(OtherProperty)) == null)
                {
                    return new ValidationResult(
                        string.Format(CultureInfo.CurrentCulture, Resources.NotEqualToAttribute_UnknownProperty, OtherProperty));
                }

                var propertyValue = property.GetValue(validationContext.ObjectInstance, null);
                if (!Equals(value, propertyValue))
                {
                    return ValidationResult.Success;
                }

                if (OtherPropertyDisplayName == null)
                {
                    OtherPropertyDisplayName = GetDisplayNameForProperty(validationContext.ObjectType, OtherProperty);
                }

                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        public string OtherProperty { get; private set; }

        public string OtherPropertyDisplayName { get; private set; }

        public override bool RequiresValidationContext
        {
            get
            {
                return true;
            }
        }
    }
}

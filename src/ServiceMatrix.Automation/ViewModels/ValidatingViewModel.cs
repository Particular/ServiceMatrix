using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    /// <summary>
    /// Provides the functionality to offer custom error information that a user interface can bind to.
    /// </summary>
    public abstract class ValidatingViewModel : ViewModel, IDataErrorInfo
    {
        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>Returns <c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        protected bool IsValid
        {
            get
            {
                return Validator.TryValidateObject(this, new ValidationContext(this), new List<ValidationResult>(), true);
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>An error message indicating what is wrong with this object. The default is
        /// an empty string ("").</returns>
        public virtual string Error
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                if (Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true))
                {
                    return string.Empty;
                }

                return string.Join(Environment.NewLine, validationResults.Select(r => r.ErrorMessage));
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        public virtual string this[string columnName]
        {
            get
            {
                var property = TypeDescriptor.GetProperties(this)[columnName];
                if (property == null)
                {
                    OnPropertyChanged(() => Error);
                    return string.Empty;
                }

                var validationResults = new List<ValidationResult>();
                if (Validator.TryValidateProperty(property.GetValue(this), new ValidationContext(this) { MemberName = columnName }, validationResults))
                {
                    OnPropertyChanged(() => Error);
                    return string.Empty;
                }

                OnPropertyChanged(() => Error);
                return string.Join(Environment.NewLine, validationResults.Select(r => r.ErrorMessage));
            }
        }
    }
}
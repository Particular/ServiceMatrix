using System;
using System.ComponentModel;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;
using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    /// <summary>
    /// Provides the functionality to offer custom error information that a user interface can bind to.
    /// </summary>
    public abstract class ValidatingViewModel : ViewModel, IDataErrorInfo
    {
        IValidator validator;

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>An error message indicating what is wrong with this object. The default is
        /// an empty string ("").</returns>
        public virtual string Error
        {
            get
            {
                var result = Validator.Validate(this);
                if (result.IsValid)
                {
                    return string.Empty;
                }

                return string.Join(Environment.NewLine, result.Errors.Select(vf => vf.ErrorMessage));
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
                var result = Validator.Validate(new ValidationContext(this, new PropertyChain(), new MemberNameValidatorSelector(new[] { columnName })));
                OnPropertyChanged(() => Error);

                if (result.IsValid)
                {
                    return string.Empty;
                }

                return string.Join(Environment.NewLine, result.Errors.Select(vf => vf.ErrorMessage));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>Returns <c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        protected bool IsValid
        {
            get
            {
                return Validator.Validate(new ValidationContext(this)).IsValid;
            }
        }

        IValidator Validator
        {
            get
            {
                return validator ?? (validator = CreateValidator());
            }
        }

        protected abstract IValidator CreateValidator();
    }
}
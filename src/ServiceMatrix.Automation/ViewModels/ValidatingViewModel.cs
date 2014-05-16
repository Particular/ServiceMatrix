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
        private Dictionary<string, Tuple<PropertyDescriptor, ValidationAttribute[]>> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationViewModel"/> class.
        /// </summary>
        protected ValidatingViewModel()
        {
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>Returns <c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(this.Error);
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
                var errors = from info in this.GetValidators()
                             from validator in info.Item2
                             where !validator.IsValid(info.Item1.GetValue(this))
                             select validator.FormatErrorMessage(info.Item1.Name);
                return string.Join(Environment.NewLine, errors.ToArray());
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
                var validatorInfo = this.GetValidators(columnName);
                if (validatorInfo == null)
                {
                    this.OnPropertyChanged(() => this.Error);
                    return string.Empty;
                }

                var columnValue = validatorInfo.Item1.GetValue(this);

                var errors = validatorInfo.Item2
                    .Where(validator => !validator.IsValid(columnValue))
                    .Select(validator => validator.FormatErrorMessage(columnName));

                this.OnPropertyChanged(() => this.Error);
                return string.Join(Environment.NewLine, errors.ToArray());
            }
        }

        private void EnsureValidators()
        {
            if (this.validators == null)
            {
                this.validators = (from property in TypeDescriptor.GetProperties(this).Cast<PropertyDescriptor>()
                                   let validators = property.Attributes.OfType<ValidationAttribute>().ToArray()
                                   where validators.Length != 0
                                   select new Tuple<PropertyDescriptor, ValidationAttribute[]>(property, validators))
                                  .ToDictionary(propInfo => propInfo.Item1.Name);
            }
        }

        private ICollection<Tuple<PropertyDescriptor, ValidationAttribute[]>> GetValidators()
        {
            this.EnsureValidators();
            return this.validators.Values;
        }

        private Tuple<PropertyDescriptor, ValidationAttribute[]> GetValidators(string columnName)
        {
            this.EnsureValidators();

            Tuple<PropertyDescriptor, ValidationAttribute[]> info;
            this.validators.TryGetValue(columnName, out info);
            return info;
        }
    }
}
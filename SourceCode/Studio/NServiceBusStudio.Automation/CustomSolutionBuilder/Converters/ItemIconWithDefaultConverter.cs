using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Windows.Data;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Converters
{
    /// <summary>
    /// Returns the element icon from NamedElementSchema or the default IconPath if the Icon doesn't exist
    /// </summary>
    public class ItemIconWithDefaultConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var model = value as IProductElementViewModel;
            if (model != null)
            {
                var element = model.Model.Info;
                if (element != null)
                {
                    if (!string.IsNullOrEmpty(element.Icon))
                    {
                        return element.Icon;
                    }
                }

                return model.IconPath;
            }

            return null;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


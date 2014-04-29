namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Converters
{
    using System;
    using System.Windows.Data;
    using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;
    using System.Globalization;

    /// <summary>
    /// Returns the element icon from NamedElementSchema or the default IconPath if the Icon doesn't exist
    /// </summary>
    public class ItemIconWithDefaultConverter : IValueConverter
    {
        const string defaultIconPath = "pack://application:,,,/NuPattern.Runtime.Core;component/Resources/NodeCollection.png";

        /// <summary>
        /// Converts a value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var model = value as LogicalViewModel.LogicalViewModelNode;
            var iconPath = default(string);

            if (model != null)
            {
                if (model.InnerViewModel.Data != null &&
                    model.InnerViewModel.Data.Info != null &&
                    !string.IsNullOrEmpty(model.InnerViewModel.Data.Info.Icon))
                {
                    iconPath = model.InnerViewModel.Data.Info.Icon;
                }
                else 
                {
                    iconPath = model.IconPath;
                }
            }

            if (iconPath != null)
            {
                // Replacing path for NuPattern Icon resources
                iconPath = iconPath.Replace("../../Resources/", "pack://application:,,,/NuPattern.Runtime.Core;component/Resources/");
            }


            return iconPath ?? defaultIconPath;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


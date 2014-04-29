namespace ServiceMatrix.Diagramming.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;
    using System.Linq;

    /// <summary>
    /// Converts a <see cref="string"/> or <see cref="Uri"/> to a cached <see cref="BitmapImage"/>.
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    [ValueConversion(typeof(Uri), typeof(BitmapImage))]
    public sealed class DiagramUriToCachedImageConverter : IValueConverter
    {
        public const string imagesPath = "pack://application:,,,/Particular.ServiceMatrix.Automation;component/Diagramming/Styles/Images/MenuItems/";

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri imageUrl;
            //value = "pack://application:,,,/Particular.ServiceMatrix.Automation;component/Diagramming/Styles/Images/MenuItems/CommandProperties.png"
            
            var path = value as string;
            
            if (path != null)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return DependencyProperty.UnsetValue;
                }
                else
                {
                    imageUrl = new Uri(path, UriKind.RelativeOrAbsolute);
                }
            }
            else
            {
                imageUrl = value as Uri;
            }

            if (imageUrl != null)
            {
                try
                {
                    var imageName = imageUrl.Segments.Last();
                    imageUrl = new Uri(imagesPath + imageName, UriKind.RelativeOrAbsolute);

                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = imageUrl;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    image.Freeze();
                    return image;
                }
                catch (Exception)
                {
                    return DependencyProperty.UnsetValue;
                }
            }

            return DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
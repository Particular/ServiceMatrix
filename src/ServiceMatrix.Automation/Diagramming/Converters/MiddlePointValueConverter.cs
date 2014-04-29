namespace ServiceMatrix.Diagramming.Converters
{
    using System;
    using System.Windows.Data;
    using System.Globalization;

    public class MiddlePointValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                throw new ArgumentException("MiddlePointValueConverter (value)");
            }

            try
            {
                var fromPoint = (double)values[0];
                var toPoint = (double)values[1];

                return fromPoint +
                       (toPoint - fromPoint) / 2;
            }
            catch (Exception)
            {
                return 0.0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

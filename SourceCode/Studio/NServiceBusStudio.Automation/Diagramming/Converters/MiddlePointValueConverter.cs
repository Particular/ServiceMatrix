using Mindscape.WpfDiagramming.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ServiceMatrix.Diagramming.Converters
{
    public class MiddlePointValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}

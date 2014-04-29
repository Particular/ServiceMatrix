using Mindscape.WpfDiagramming.Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ServiceMatrix.Diagramming.Converters
{
    using System.Globalization;

    public class StartSegmentValueConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var segments = (ObservableCollection<DiagramConnectionSegment>)value;

            if (parameter == null)
            {
                return segments.First().StartPoint;
            }
            else if (parameter.ToString() == "X")
            {
                return segments.First().StartPoint.X;
            }
            else
            {
                return segments.First().StartPoint.Y;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
